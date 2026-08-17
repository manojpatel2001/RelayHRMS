CREATE OR ALTER PROCEDURE [dbo].[GetUserCompanyPermissionsByEmployeeId]
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ucm.UserCompanyPermissionId,
        ucm.EmployeeId,
        ucm.CompanyId,
        cd.CompanyName,
        ucm.IsAdmin,
        ucm.IsEnabled,
        (
            SELECT STRING_AGG(au.FullName, ', ')
            FROM UserCompanyPermission other_ucm
            INNER JOIN AspNetUsers au
                ON au.Id = other_ucm.EmployeeId
                AND au.IsDeleted = 0
                AND au.IsEnabled = 1
            WHERE other_ucm.CompanyId = ucm.CompanyId
              AND other_ucm.IsDeleted = 0
              AND other_ucm.EmployeeId <> ucm.EmployeeId
        ) AS OtherAssignedUsers
    FROM UserCompanyPermission ucm
    INNER JOIN CompanyDetails cd ON cd.CompanyId = ucm.CompanyId AND cd.IsDeleted = 0
    WHERE ucm.EmployeeId = @EmployeeId AND ucm.IsDeleted = 0
    ORDER BY cd.CompanyName;
END
