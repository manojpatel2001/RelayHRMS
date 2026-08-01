-- ============================================================================
-- sp_GetRecentActivity
--
-- Backs the ESS Dashboard "Recent Activity" panel. Unions, for a given
-- employee, their own recent submissions AND any approver actions they took
-- on a subordinate's request, across 5 sources:
--   Leave (LeaveApplication), Attendance Regularization (AttendanceRegularization),
--   Comp-Off (Comp_Off_Details), Exit Application (ExitApplications),
--   Attendance punches (AttendanceLog).
--
-- Approver attribution:
--   - Leave / Comp-Off have a dedicated ApprovedBy (+ ApprovalDate) column,
--     stamped only on an actual approve/reject action.
--   - Attendance Regularization / Exit Application have no dedicated
--     approver column; UpdatedBy/UpdatedDate is reused for both self-edits
--     and approver actions, so those two blocks additionally require
--     EmpId/Employeeid <> @EmployeeId (i.e. it must be someone else's
--     record) to avoid misattributing a self-edit as an approver action.
--
-- Punch in/out direction (Mode is NOT "IN"/"OUT" — it stores the punch
-- source, e.g. "Web"/"Device") is inferred the same way
-- GetEmployeeTodayInOutStatus.sql does for "today": punches alternate
-- in/out/in/out per employee per calendar day, ordered by PunchDateTime.
--
-- Window: last 30 days, top 20 rows, newest first.
-- ============================================================================
CREATE OR ALTER PROCEDURE [dbo].[sp_GetRecentActivity]
    @EmployeeId INT,
    @CompanyId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Since DATETIME = DATEADD(DAY, -30, GETDATE());

    ;WITH Activity AS (

        -- Leave — own submissions
        SELECT 'Leave' AS ActivityType,
               la.CreatedDate AS ActivityDate,
               'Leave Request' AS Title,
               CONCAT(ISNULL(la.ApplicationType,''), ' - ', CONVERT(varchar,la.FromDate,105), ' to ', CONVERT(varchar,la.Todate,105)) AS Description,
               ISNULL(la.LeaveStatus,'Pending') AS Status,
               'Self' AS Role,
               CAST(NULL AS nvarchar(200)) AS RelatedEmployeeName,
               la.LeaveApplicationid AS SourceId
        FROM LeaveApplication la
        WHERE la.EmplooyeId = @EmployeeId AND la.CompId = @CompanyId AND ISNULL(la.IsDeleted,0) = 0
          AND la.CreatedDate >= @Since

        UNION ALL

        -- Leave — approver actions
        SELECT 'Leave',
               la.ApprovalDate,
               CASE WHEN la.LeaveStatus = 'Approved' THEN 'Leave Approved' ELSE 'Leave Rejected' END,
               CONCAT(CONVERT(varchar,la.FromDate,105), ' to ', CONVERT(varchar,la.Todate,105)),
               ISNULL(la.LeaveStatus,'Pending'),
               'Approver',
               emp.FullName,
               la.LeaveApplicationid
        FROM LeaveApplication la
        JOIN AspNetUsers emp ON emp.Id = la.EmplooyeId
        WHERE la.ApprovedBy = CAST(@EmployeeId AS nvarchar(50)) AND la.CompId = @CompanyId AND ISNULL(la.IsDeleted,0) = 0
          AND la.ApprovalDate IS NOT NULL AND la.ApprovalDate >= @Since
          AND la.LeaveStatus IN ('Approved','Rejected')

        UNION ALL

        -- Attendance Regularization — own submissions
        SELECT 'AttendanceRegularization',
               ar.CreatedDate,
               'Attendance Regularization',
               CONCAT(CONVERT(varchar,ar.ForDate,105), ' - ', ISNULL(ar.Reason,'')),
               ISNULL(ar.Status,'Pending'),
               'Self',
               NULL,
               ar.AttendanceRegularizationId
        FROM AttendanceRegularization ar
        WHERE ar.EmpId = @EmployeeId AND ISNULL(ar.IsDeleted,0) = 0
          AND ar.CreatedDate >= @Since

        UNION ALL

        -- Attendance Regularization — approver actions (UpdatedBy is reused for
        -- self-edits too, so require it to be someone else's record)
        SELECT 'AttendanceRegularization',
               ar.UpdatedDate,
               CASE WHEN ar.Status = 'Approved' THEN 'Attendance Regularization Approved'
                    WHEN ar.Status = 'Rejected' THEN 'Attendance Regularization Rejected'
                    ELSE 'Attendance Regularization Updated' END,
               CONVERT(varchar,ar.ForDate,105),
               ISNULL(ar.Status,'Pending'),
               'Approver',
               emp.FullName,
               ar.AttendanceRegularizationId
        FROM AttendanceRegularization ar
        JOIN AspNetUsers emp ON emp.Id = ar.EmpId
        WHERE ar.UpdatedBy = CAST(@EmployeeId AS nvarchar(50)) AND ar.EmpId <> @EmployeeId
          AND ISNULL(ar.IsDeleted,0) = 0
          AND ar.UpdatedDate IS NOT NULL AND ar.UpdatedDate >= @Since
          AND ar.Status IN ('Approved','Rejected','ProceedToHRD')

        UNION ALL

        -- Comp-Off — own submissions
        SELECT 'CompOff',
               cod.CreatedDate,
               'Comp-Off Request',
               CONCAT(ISNULL(cod.Comp_Off_Type,''), ' - ', CONVERT(varchar,cod.ApplicationDate,105)),
               ISNULL(cod.Application_Status,'Pending'),
               'Self',
               NULL,
               cod.Comp_Off_Detailsid
        FROM Comp_Off_Details cod
        WHERE cod.Emp_Id = @EmployeeId AND cod.Cmp_Id = @CompanyId AND ISNULL(cod.IsDeleted,0) = 0
          AND cod.CreatedDate >= @Since

        UNION ALL

        -- Comp-Off — approver actions
        SELECT 'CompOff',
               cod.ApprovalDate,
               CASE WHEN cod.Application_Status = 'Approved' THEN 'Comp-Off Approved' ELSE 'Comp-Off Rejected' END,
               CONVERT(varchar,cod.ApplicationDate,105),
               ISNULL(cod.Application_Status,'Pending'),
               'Approver',
               emp.FullName,
               cod.Comp_Off_Detailsid
        FROM Comp_Off_Details cod
        JOIN AspNetUsers emp ON emp.Id = cod.Emp_Id
        WHERE cod.ApprovedBy = CAST(@EmployeeId AS nvarchar(50)) AND cod.Cmp_Id = @CompanyId AND ISNULL(cod.IsDeleted,0) = 0
          AND cod.ApprovalDate IS NOT NULL AND cod.ApprovalDate >= @Since
          AND cod.Application_Status IN ('Approved','Rejected')

        UNION ALL

        -- Exit Application — own submission
        SELECT 'Exit',
               ea.CreatedDate,
               'Exit Application',
               CONCAT('Resignation dated ', CONVERT(varchar,ea.ResignationDate,105)),
               CASE WHEN ea.IsApproved = 1 THEN 'Approved' WHEN ea.IsRejected = 1 THEN 'Rejected' ELSE 'Pending' END,
               'Self',
               NULL,
               ea.ExitApplicationID
        FROM ExitApplications ea
        WHERE ea.Employeeid = @EmployeeId AND ISNULL(ea.IsDeleted,0) = 0
          AND ea.CreatedDate >= @Since

        UNION ALL

        -- Exit Application — approver actions (UpdatedBy reused for self-edits too)
        SELECT 'Exit',
               ea.UpdatedDate,
               CASE WHEN ea.IsApproved = 1 THEN 'Exit Application Approved'
                    WHEN ea.IsRejected = 1 THEN 'Exit Application Rejected'
                    ELSE 'Exit Application Updated' END,
               CONCAT('Resignation dated ', CONVERT(varchar,ea.ResignationDate,105)),
               CASE WHEN ea.IsApproved = 1 THEN 'Approved' WHEN ea.IsRejected = 1 THEN 'Rejected' ELSE 'Pending' END,
               'Approver',
               emp.FullName,
               ea.ExitApplicationID
        FROM ExitApplications ea
        JOIN AspNetUsers emp ON emp.Id = ea.Employeeid
        WHERE ea.UpdatedBy = @EmployeeId AND ea.Employeeid <> @EmployeeId
          AND ISNULL(ea.IsDeleted,0) = 0
          AND ea.UpdatedDate IS NOT NULL AND ea.UpdatedDate >= @Since
          AND (ea.IsApproved = 1 OR ea.IsRejected = 1)

        UNION ALL

        -- Attendance punches — own punches only (no approver concept).
        -- In/out direction inferred by alternating parity per employee per
        -- calendar day, same convention as GetEmployeeTodayInOutStatus.
        SELECT 'Punch',
               al.PunchDateTime,
               CASE WHEN ROW_NUMBER() OVER (PARTITION BY al.EmployeeId, CAST(al.PunchDateTime AS DATE) ORDER BY al.PunchDateTime ASC) % 2 = 1
                    THEN 'Punched In' ELSE 'Punched Out' END,
               ISNULL(al.LocationName,''),
               'Recorded',
               'Self',
               NULL,
               al.Id
        FROM AttendanceLog al
        WHERE al.EmployeeId = @EmployeeId AND al.CompanyId = @CompanyId
          AND al.PunchDateTime >= @Since

    )
    SELECT TOP 20 *
    FROM Activity
    ORDER BY ActivityDate DESC;
END
