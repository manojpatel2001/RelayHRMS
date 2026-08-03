-- Adds Status (Pending/Approved/Rejected/all) and Branch filtering to the
-- manager-scoped "Leave Approval" stored procedure, mirroring the pattern
-- already used by SP_GetLeaveApplicationsForApprovalAdmin. Previously this
-- procedure hardcoded WHERE la.LeaveStatus = 'Pending' with no way to view
-- Approved/Rejected history or filter by branch from the Employee Panel's
-- own "Leave Approval" page.
--
-- @LeaveStatus defaults to 'Pending' so any caller that omits it keeps the
-- exact previous behavior. Confirmed via codebase search that this
-- procedure is called from exactly one place (LeaveApplicationRepository.
-- GetLeaveApplicationsforApprove), so this change is self-contained.

CREATE OR ALTER PROCEDURE [dbo].[SP_GetLeaveApplicationsForAproval]
    @SearchType  NVARCHAR(50)  = NULL,   -- Accepts 'Employee Name', 'Employee Code', 'Leave Name'
    @SearchFor   NVARCHAR(100) = NULL,
    @EmpId       INT,
    @CompId      INT,
    @LeaveStatus NVARCHAR(20)  = 'Pending',  -- 'Pending' | 'Approved' | 'Rejected' | NULL = all
    @BranchId    INT           = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SET @SearchType  = NULLIF(@SearchType,  '');
    SET @SearchFor   = NULLIF(@SearchFor,   '');
    SET @LeaveStatus = NULLIF(@LeaveStatus, '');
    SET @BranchId    = NULLIF(@BranchId,    0);

    SELECT
        la.LeaveApplicationid,
        leave.leave_Name AS LeaveTypeName,
        Emp.FullName AS EmployeeName,
        Emp.EmployeeCode,
        Reporting.FullName AS ReportingPersonName,
        la.FromDate,
        la.Todate,
        la.No_Of_Date,
        la.Reason,
        la.ApplicationType,
        la.LeaveStatus,
        B.BranchName,
        la.Day
    FROM LeaveApplication la
    LEFT JOIN AspNetUsers Emp ON la.EmplooyeId = Emp.Id
    LEFT JOIN AspNetUsers Reporting ON la.ReportingManagerId = Reporting.Id
    LEFT JOIN LeaveMaster leave ON la.LeaveType = leave.leave_TypeId
    LEFT JOIN Branch B ON B.BranchId = Emp.BranchId
    WHERE
        la.ReportingManagerId = @EmpId
        AND Emp.Isenabled = 1 AND Emp.Isdeleted = 0
        AND Reporting.Isenabled = 1 AND Reporting.Isdeleted = 0

        AND (
            @LeaveStatus IS NULL
            OR (@LeaveStatus = 'Pending'  AND LOWER(la.LeaveStatus) = 'pending')
            OR (@LeaveStatus = 'Approved' AND LOWER(la.LeaveStatus) = 'approved')
            OR (@LeaveStatus = 'Rejected' AND LOWER(la.LeaveStatus) = 'rejected')
        )

        AND (@BranchId IS NULL OR Emp.BranchId = @BranchId)

        AND (
             (@SearchType IS NULL AND @SearchFor IS NULL)
            OR (@SearchType IS NOT NULL AND @SearchFor IS NULL)
            OR (@SearchType = 'Employee Name' AND Emp.FullName LIKE '%' + @SearchFor + '%')
            OR (@SearchType = 'Employee Code' AND Emp.EmployeeCode LIKE '%' + @SearchFor + '%')
            OR (@SearchType = 'Leave Name' AND leave.leave_Name LIKE '%' + @SearchFor + '%')
        )
    ORDER BY la.FromDate DESC
END
