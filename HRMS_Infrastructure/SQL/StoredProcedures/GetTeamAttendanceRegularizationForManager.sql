-- ============================================================================
-- GetTeamAttendanceRegularizationForManager
--
-- Backs the new "Team Attendance Approval" ESS page: a reporting manager sees
-- and approves ONLY their own direct reports' attendance regularization
-- requests, across ALL reasons (unlike GetAttendanceRegularizationApproval,
-- which restricts manager visibility to just 2 reason types and routes
-- everything else to HR — left untouched so the existing approval page keeps
-- its current behavior).
-- ============================================================================
CREATE PROCEDURE [dbo].[GetTeamAttendanceRegularizationForManager]
    @SearchBy NVARCHAR(20) = NULL,
    @SearchValue NVARCHAR(100) = NULL,
    @FromDate DATE = NULL,
    @ToDate DATE = NULL,
    @Status NVARCHAR(20) = NULL,
    @LoggedInUserId NVARCHAR(128) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Att.AttendanceRegularizationId,
        Att.EmpId,
        Att.FullName,
        Users.EmployeeCode,
        Att.ForDate,
        Att.CreatedDate,
        Att.ShiftTime,
        Att.InTime,
        Att.OutTime,
        Att.Duration,
        Att.[Day],
        Com.Reason,
        Att.Remark,
        Att.IsApproved,
        Att.IsPending,
        Att.IsRejected,
        Appr.FullName AS [UpdatedBy],
        Att.UpdatedDate
    FROM
        AttendanceRegularization Att
    LEFT JOIN
        AspNetUsers Users ON Users.Id = Att.EmpId
    LEFT JOIN
        AspNetUsers Appr ON Appr.Id = Att.UpdatedBy
    LEFT JOIN
        CommonReasons Com ON Com.ReasonId = Att.Reason
    WHERE
        Att.IsDeleted = 0
        AND Att.IsEnabled = 1
        AND Users.IsBlocked = 0
        AND Users.IsDeleted = 0
        AND Users.IsEnabled = 1
        AND Users.ReportingManagerId = @LoggedInUserId
        AND (
            @FromDate IS NULL OR CAST(Att.ForDate AS DATE) >= @FromDate
        )
        AND (
            @ToDate IS NULL OR CAST(Att.ForDate AS DATE) <= @ToDate
        )
        AND (
            @Status IS NULL
            OR (@Status = 'Approved' AND Att.IsApproved = 1)
            OR (@Status = 'Pending' AND Att.IsPending = 1)
            OR (@Status = 'Rejected' AND Att.IsRejected = 1)
        )
        AND (
            @SearchBy IS NULL
            OR (@SearchBy = 'EmpCode' AND Users.EmployeeCode LIKE '%' + @SearchValue + '%')
            OR (@SearchBy = 'EmpName' AND Att.FullName LIKE '%' + @SearchValue + '%')
            OR (@SearchBy = 'Reason' AND Com.Reason LIKE '%' + @SearchValue + '%')
        )
    ORDER BY
        Att.ForDate DESC;
END
GO
