-- Fixes a cross-company gap in GetDateWiseAttendanceDetails: a manager's
-- direct reports in a DIFFERENT company than the manager's own were being
-- silently excluded by the blanket `emp.CompanyId = @CompanyId` filter,
-- even though GetEmployeesByReportingManager (which populates the page's
-- own Employee Code dropdown) has no such restriction and already lists
-- them. Confirmed via codebase search that both callers of this procedure
-- (MemberInOutRecord.cshtml, EmployeeandTeam.cshtml) always pass the
-- logged-in manager's own non-zero EmployeeId, never 0.
--
-- The CompanyId filter is now scoped ONLY to the @EmployeeId = 0 branch
-- (a broader/company-wide query with no manager scoping at all), which
-- preserves that safety net. When a specific manager's EmployeeId is
-- passed, ReportingManagerId/Id matching already fully determines the
-- result set (only that manager's own record + their real direct
-- reports), so gating it further by company was incorrect, not protective.

CREATE OR ALTER PROCEDURE [dbo].[GetDateWiseAttendanceDetails]
    @FromDate DATE,
    @ToDate DATE,
    @EmployeeId INT = 0,
    @CompanyId INT = 0,
    @MemberId INT = 0
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ad.EmployeeId,
        FORMAT(ad.InTime, 'hh:mm tt') AS InTime,
        FORMAT(ad.OutTime, 'hh:mm tt') AS OutTime,
        ad.Workinghours,
        ad.AttendanceStatus,
        ad.ShiftDate,
        ad.SalaryDay,
        emp.FullName,
        b.BranchName,
        emp.EmployeeCode,
        ad.CreatedOn,
        cd.CompanyId,
        cd.CompanyName
    FROM attendanceDetails ad
    LEFT JOIN AspNetUsers emp ON emp.Id = ad.EmployeeId
    LEFT JOIN Branch b ON b.branchId = emp.BranchId
    LEFT JOIN CompanyDetails cd ON cd.CompanyId = emp.CompanyId
    WHERE
        emp.IsEnabled = 1
        AND emp.IsDeleted = 0
        AND CAST(ad.ShiftDate AS DATE) >= @FromDate
        AND CAST(ad.ShiftDate AS DATE) <= @ToDate
        AND (
            (@EmployeeId = 0 AND emp.CompanyId = @CompanyId)
            OR emp.ReportingManagerId = @EmployeeId
            OR emp.Id = @EmployeeId
        )
        AND (@MemberId = 0 OR emp.Id = @MemberId)
        ORDER BY ShiftDate DESC;
END
