CREATE OR ALTER PROCEDURE [dbo].[GetAllUserCompanyPermissions]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ucm.UserCompanyPermissionId,
        ucm.EmployeeId,
        au.EmployeeCode,
        au.FullName AS EmployeeName,
        ucm.CompanyId,
        cd.CompanyName,
        ucm.IsAdmin,
        ucm.IsEnabled,
        COUNT(*) OVER (PARTITION BY ucm.EmployeeId) AS TotalCompanies
    FROM UserCompanyPermission ucm
    INNER JOIN AspNetUsers au
        ON au.Id = ucm.EmployeeId
        AND au.IsDeleted = 0
        AND au.IsEnabled = 1
    INNER JOIN CompanyDetails cd
        ON cd.CompanyId = ucm.CompanyId
        AND cd.IsDeleted = 0
    WHERE ucm.IsDeleted = 0
    ORDER BY au.FullName, cd.CompanyName;
END
