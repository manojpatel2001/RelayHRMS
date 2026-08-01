-- ============================================================================
-- sp_GetEmployeeLifecycleTimeline
--
-- Backs the Admin Panel "Employee History / Employee Lifecycle" page. Unions a
-- single employee's full career history across 23 categories, oldest data first
-- unless the caller re-sorts client-side (ORDER BY ActivityDate DESC here, newest
-- on top, matching the pattern already used for sp_GetRecentActivity.sql).
--
-- Unlike sp_GetRecentActivity (ESS dashboard "recent activity", 30-day/top-20
-- window, self + approver-actions across 5 sources), this has NO time window —
-- full career — and covers all 23 lifecycle categories:
--   Joining, Documents, Probation, Confirmation, Salary Revisions, Promotions,
--   Transfers, Department Changes, Reporting Manager Changes, Shift History,
--   Attendance (monthly summary, not one row per punch), Leave, Payroll
--   (monthly), Resignation, Notice Period, Full & Final Settlement, Exit,
--   plus the 5 categories with brand-new tables built for this feature:
--   Recruitment, Performance Reviews, Trainings, Certifications, Awards,
--   Disciplinary Actions (Promotions/Department Changes/Shift History also use
--   new tables, listed under their category name above).
--
-- ActivityType values map to front-end icon/color lookups (see ACTIVITY_ICONS-
-- style object in the Employee History page JS) — kept as plain category labels,
-- not stored colors/icons, same reasoning as sp_GetRecentActivity.
-- ============================================================================
CREATE OR ALTER PROCEDURE [dbo].[sp_GetEmployeeLifecycleTimeline]
    @EmployeeId INT,
    @CompanyId INT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Timeline AS (

        -- Joining
        SELECT 'Joining' AS ActivityType, u.DateOfJoining AS ActivityDate,
               'Joined the Organization' AS Title,
               CASE WHEN u.OfferDate IS NOT NULL THEN CONCAT('Offer accepted on ', CONVERT(varchar,u.OfferDate,105)) ELSE '' END AS Description,
               'Completed' AS Status, u.Id AS SourceId
        FROM AspNetUsers u
        WHERE u.Id = @EmployeeId AND u.DateOfJoining IS NOT NULL

        UNION ALL

        -- Documents
        SELECT 'Documents', ad.CreatedDate, 'Document Uploaded',
               ISNULL(ad.Comment,''),
               CASE WHEN ad.DateOfExpiry IS NOT NULL AND ad.DateOfExpiry < GETDATE() THEN 'Expired' ELSE 'Valid' END,
               ad.AttachmentDetailsId
        FROM AttachmentDetails ad
        WHERE ad.EmployeeId = @EmployeeId AND ISNULL(ad.IsDeleted,0) = 0

        UNION ALL

        -- Probation
        SELECT 'Probation', pp.ProbationEvaluationDate, 'Probation Evaluation',
               ISNULL(pp.RemarksOfApprover,''), CASE WHEN pp.Rating IS NOT NULL THEN CONCAT('Rating: ', pp.Rating) ELSE 'Pending' END,
               pp.ProbationPerformanceId
        FROM ProbationPerformance pp
        WHERE pp.EmployeeId = @EmployeeId AND ISNULL(pp.IsActive,1) = 1

        UNION ALL

        -- Confirmation (derived from AspNetUsers.ConfirmDate — no dedicated table)
        SELECT 'Confirmation', u.ConfirmDate, 'Employment Confirmed',
               'Probation period completed', 'Confirmed', u.Id
        FROM AspNetUsers u
        WHERE u.Id = @EmployeeId AND u.ConfirmDate IS NOT NULL

        UNION ALL

        -- Salary Revisions
        SELECT 'SalaryRevision', esh.EffectiveFromDate, 'Salary Revised',
               CONCAT('Gross: ', ISNULL(esh.OldGrossSalary,0), ' -> ', ISNULL(esh.NewGrossSalary,0)),
               'Effective', esh.Id
        FROM EmployeeSalaryHistory esh
        WHERE esh.EmployeeId = @EmployeeId AND ISNULL(esh.IsActive,1) = 1

        UNION ALL

        -- Promotions (Designation/Grade history — new table)
        SELECT 'Promotion', edh.EffectiveDate, 'Designation Changed',
               CONCAT(ISNULL(od.DesignationName,'-'), ' -> ', ISNULL(nd.DesignationName,'-'),
                      CASE WHEN og.GradeName IS NOT NULL OR ng.GradeName IS NOT NULL
                           THEN CONCAT(' | Grade: ', ISNULL(og.GradeName,'-'), ' -> ', ISNULL(ng.GradeName,'-')) ELSE '' END,
                      CASE WHEN edh.Reason IS NOT NULL THEN CONCAT(' - ', edh.Reason) ELSE '' END),
               'Effective', edh.EmployeeDesignationHistoryId
        FROM EmployeeDesignationHistory edh
        LEFT JOIN Designation od ON od.DesignationId = edh.OldDesignationId
        LEFT JOIN Designation nd ON nd.DesignationId = edh.NewDesignationId
        LEFT JOIN Grade og ON og.GradeId = edh.OldGradeId
        LEFT JOIN Grade ng ON ng.GradeId = edh.NewGradeId
        WHERE edh.EmployeeId = @EmployeeId AND edh.IsDeleted = 0

        UNION ALL

        -- Transfers (branch + reporting person)
        SELECT 'Transfer', et.EffectiveDate, 'Branch Transfer',
               CONCAT(ISNULL(ob.BranchName,'-'), ' -> ', ISNULL(nb.BranchName,'-'),
                      CASE WHEN et.Reason IS NOT NULL THEN CONCAT(' - ', et.Reason) ELSE '' END),
               'Effective', et.TransferId
        FROM EmployeeTransfer et
        LEFT JOIN Branch ob ON ob.BranchId = et.CurrentBranchId
        LEFT JOIN Branch nb ON nb.BranchId = et.TransferBranchId
        WHERE et.EmployeeId = @EmployeeId AND ISNULL(et.IsDeleted,0) = 0

        UNION ALL

        -- Department Changes (new table)
        SELECT 'DepartmentChange', edh2.EffectiveDate, 'Department Changed',
               CONCAT(ISNULL(od2.DepartmentName,'-'), ' -> ', ISNULL(nd2.DepartmentName,'-'),
                      CASE WHEN edh2.Reason IS NOT NULL THEN CONCAT(' - ', edh2.Reason) ELSE '' END),
               'Effective', edh2.EmployeeDepartmentHistoryId
        FROM EmployeeDepartmentHistory edh2
        LEFT JOIN Department od2 ON od2.DepartmentId = edh2.OldDepartmentId
        LEFT JOIN Department nd2 ON nd2.DepartmentId = edh2.NewDepartmentId
        WHERE edh2.EmployeeId = @EmployeeId AND edh2.IsDeleted = 0

        UNION ALL

        -- Reporting Manager Changes
        SELECT 'ManagerChange', rmd.EffectedDate, 'Reporting Manager Changed',
               ISNULL(mgr.FullName,''), 'Effective', rmd.ReportingManagerDetailsId
        FROM ReportingManagerDetails rmd
        LEFT JOIN AspNetUsers mgr ON mgr.Id = rmd.ReportingManagerId
        WHERE rmd.EmployeeId = @EmployeeId AND ISNULL(rmd.IsDeleted,0) = 0

        UNION ALL

        -- Shift History (new table)
        SELECT 'ShiftChange', esh2.EffectiveDate, 'Shift Changed',
               CONCAT(ISNULL(os.ShiftName,'-'), ' -> ', ISNULL(ns.ShiftName,'-'),
                      CASE WHEN esh2.Reason IS NOT NULL THEN CONCAT(' - ', esh2.Reason) ELSE '' END),
               'Effective', esh2.EmployeeShiftHistoryId
        FROM EmployeeShiftHistory esh2
        LEFT JOIN ShiftMaster os ON os.ShiftID = esh2.OldShiftMasterId
        LEFT JOIN ShiftMaster ns ON ns.ShiftID = esh2.NewShiftMasterId
        WHERE esh2.EmployeeId = @EmployeeId AND esh2.IsDeleted = 0

        UNION ALL

        -- Attendance — monthly punch-day summary, not one row per punch
        SELECT 'Attendance',
               DATEFROMPARTS(YEAR(al.PunchDateTime), MONTH(al.PunchDateTime), 1) AS ActivityDate,
               CONCAT('Attendance - ', DATENAME(MONTH, al.PunchDateTime), ' ', YEAR(al.PunchDateTime)) AS Title,
               CONCAT(COUNT(DISTINCT CAST(al.PunchDateTime AS DATE)), ' day(s) with recorded punches'),
               'Recorded', MIN(al.Id)
        FROM AttendanceLog al
        WHERE al.EmployeeId = @EmployeeId AND al.CompanyId = @CompanyId
        GROUP BY YEAR(al.PunchDateTime), MONTH(al.PunchDateTime), DATENAME(MONTH, al.PunchDateTime)

        UNION ALL

        -- Leave
        SELECT 'Leave', la.CreatedDate, 'Leave Request',
               CONCAT(ISNULL(la.ApplicationType,''), ' - ', CONVERT(varchar,la.FromDate,105), ' to ', CONVERT(varchar,la.Todate,105)),
               ISNULL(la.LeaveStatus,'Pending'), la.LeaveApplicationid
        FROM LeaveApplication la
        WHERE la.EmplooyeId = @EmployeeId AND la.CompId = @CompanyId AND ISNULL(la.IsDeleted,0) = 0

        UNION ALL

        -- Payroll — one row per processed month
        SELECT 'Payroll', DATEFROMPARTS(sd.Year, sd.MonthNumber, 1), CONCAT('Payroll - ', sd.MonthName, ' ', sd.Year),
               CONCAT('Net Salary: ', ISNULL(sd.NetSalary,0)), 'Processed', sd.Id
        FROM SalaryDetails sd
        WHERE sd.EmployeeId = @EmployeeId

        UNION ALL

        -- Resignation
        SELECT 'Resignation', CAST(ea.ResignationDate AS DATETIME), 'Resignation Submitted',
               ISNULL(ea.ReasonForResignation,''),
               CASE WHEN ea.IsApproved = 1 THEN 'Approved' WHEN ea.IsRejected = 1 THEN 'Rejected' ELSE 'Pending' END,
               ea.ExitApplicationID
        FROM ExitApplications ea
        WHERE ea.Employeeid = @EmployeeId AND ISNULL(ea.IsDeleted,0) = 0

        UNION ALL

        -- Notice Period
        SELECT 'NoticePeriod', CAST(ea2.LastWorkingDate AS DATETIME), 'Notice Period',
               CONCAT(ea2.NoticePeriodDays, ' day(s) notice, last working day ', CONVERT(varchar,ea2.LastWorkingDate,105)),
               CASE WHEN ea2.ShortFallDays > 0 THEN CONCAT(ea2.ShortFallDays, ' day(s) shortfall') ELSE 'Served' END,
               ea2.ExitApplicationID
        FROM ExitApplications ea2
        WHERE ea2.Employeeid = @EmployeeId AND ISNULL(ea2.IsDeleted,0) = 0 AND ea2.LastWorkingDate IS NOT NULL

        UNION ALL

        -- Full & Final Settlement (derived — no dedicated table; last processed
        -- payroll row on/after the exit's last working date, for an approved exit)
        SELECT 'FnFSettlement', DATEFROMPARTS(sd2.Year, sd2.MonthNumber, 1), 'Full & Final Settlement',
               CONCAT('Net Settlement: ', ISNULL(sd2.NetSalary,0)), 'Processed', sd2.Id
        FROM SalaryDetails sd2
        JOIN ExitApplications ea3 ON ea3.Employeeid = sd2.EmployeeId AND ISNULL(ea3.IsDeleted,0) = 0 AND ea3.IsApproved = 1
        WHERE sd2.EmployeeId = @EmployeeId
          AND DATEFROMPARTS(sd2.Year, sd2.MonthNumber, 1) = (
              SELECT MAX(DATEFROMPARTS(sd3.Year, sd3.MonthNumber, 1))
              FROM SalaryDetails sd3 WHERE sd3.EmployeeId = @EmployeeId
          )

        UNION ALL

        -- Exit / Relieving (derived from ExitApplications completion state)
        SELECT 'Exit', CAST(ea4.LastWorkingDate AS DATETIME), 'Exit / Relieved',
               CASE WHEN ea4.IsNOCFormFilled = 1 THEN 'NOC completed' ELSE 'NOC pending' END,
               CASE WHEN ea4.IsApproved = 1 THEN 'Relieved' ELSE 'In Progress' END,
               ea4.ExitApplicationID
        FROM ExitApplications ea4
        WHERE ea4.Employeeid = @EmployeeId AND ISNULL(ea4.IsDeleted,0) = 0 AND ea4.IsApproved = 1

        UNION ALL

        -- Recruitment (new table)
        SELECT 'Recruitment', ISNULL(erd.ApplicationDate, erd.CreatedDate), 'Recruitment',
               CONCAT(ISNULL(erd.Source,''), CASE WHEN erd.InterviewerName IS NOT NULL THEN CONCAT(' - Interviewed by ', erd.InterviewerName) ELSE '' END),
               CASE WHEN erd.OfferAcceptedDate IS NOT NULL THEN 'Offer Accepted' WHEN erd.OfferDate IS NOT NULL THEN 'Offer Extended' ELSE 'In Process' END,
               erd.EmployeeRecruitmentDetailsId
        FROM EmployeeRecruitmentDetails erd
        WHERE erd.EmployeeId = @EmployeeId AND erd.IsDeleted = 0

        UNION ALL

        -- Performance Reviews (new table)
        SELECT 'PerformanceReview', epr.ReviewDate, 'Performance Review',
               CONCAT(ISNULL(epr.Strengths,''), CASE WHEN epr.Rating IS NOT NULL THEN CONCAT(' | Rating: ', epr.Rating) ELSE '' END),
               ISNULL(epr.Status,'Draft'), epr.EmployeePerformanceReviewId
        FROM EmployeePerformanceReview epr
        WHERE epr.EmployeeId = @EmployeeId AND epr.IsDeleted = 0

        UNION ALL

        -- Trainings (new table)
        SELECT 'Training', et2.StartDate, CONCAT('Training - ', et2.TrainingName),
               ISNULL(et2.Provider,''), ISNULL(et2.Status,'Scheduled'), et2.EmployeeTrainingId
        FROM EmployeeTraining et2
        WHERE et2.EmployeeId = @EmployeeId AND et2.IsDeleted = 0

        UNION ALL

        -- Certifications (new table)
        SELECT 'Certification', ec.IssueDate, CONCAT('Certification - ', ec.CertificationName),
               ISNULL(ec.IssuingBody,''),
               CASE WHEN ec.ExpiryDate IS NOT NULL AND ec.ExpiryDate < GETDATE() THEN 'Expired' ELSE 'Valid' END,
               ec.EmployeeCertificationId
        FROM EmployeeCertification ec
        WHERE ec.EmployeeId = @EmployeeId AND ec.IsDeleted = 0

        UNION ALL

        -- Awards (new table)
        SELECT 'Award', ea5.AwardDate, CONCAT('Award - ', ea5.AwardName),
               ISNULL(ea5.Description,''), ISNULL(ea5.AwardCategory,'Recognition'), ea5.EmployeeAwardId
        FROM EmployeeAward ea5
        WHERE ea5.EmployeeId = @EmployeeId AND ea5.IsDeleted = 0

        UNION ALL

        -- Disciplinary Actions (new table, reuses WarningMaster as type lookup)
        SELECT 'DisciplinaryAction', ewh.IssueDate, CONCAT('Warning - ', ISNULL(wm.WarningName,'')),
               ISNULL(ewh.IncidentDescription,''), ISNULL(ewh.Status,'Issued'), ewh.EmployeeWarningHistoryId
        FROM EmployeeWarningHistory ewh
        LEFT JOIN WarningMaster wm ON wm.WarningMasterId = ewh.WarningMasterId
        WHERE ewh.EmployeeId = @EmployeeId AND ewh.IsDeleted = 0

    )
    SELECT *
    FROM Timeline
    WHERE ActivityDate IS NOT NULL
    ORDER BY ActivityDate DESC;
END
