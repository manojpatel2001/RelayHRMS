CREATE OR ALTER PROCEDURE [dbo].[GetUserCompanyPermissionsByCompanyId]
    @CompanyId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ucm.UserCompanyPermissionId,
        ucm.EmployeeId,
        au.EmployeeCode,
        au.FullName AS EmployeeName,
        ucm.CompanyId,
        ucm.IsAdmin,
        ucm.IsEnabled
    FROM UserCompanyPermission ucm
    INNER JOIN AspNetUsers au
        ON au.Id = ucm.EmployeeId
        AND au.IsDeleted = 0
        AND au.IsEnabled = 1
    WHERE ucm.CompanyId = @CompanyId AND ucm.IsDeleted = 0
    ORDER BY au.FullName;
END
