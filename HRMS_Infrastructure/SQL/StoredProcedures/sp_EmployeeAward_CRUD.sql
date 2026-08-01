-- Combined INSERT/UPDATE/DELETE/GET procedure for EmployeeAward,
-- called from HRMS_Infrastructure\Repository\Employee\EmployeeAwardRepository.cs
-- via Dapper, discriminated by @Operation. Mirrors sp_NewsAnnouncement_CRUD.sql.

CREATE OR ALTER PROCEDURE [dbo].[sp_EmployeeAward_CRUD]
    @Operation VARCHAR(10),
    @EmployeeAwardId INT = NULL,
    @EmployeeId INT = NULL,
    @AwardName VARCHAR(200) = NULL,
    @AwardCategory VARCHAR(100) = NULL,
    @AwardDate DATE = NULL,
    @AwardedBy VARCHAR(200) = NULL,
    @Description VARCHAR(500) = NULL,
    @IsEnabled BIT = 1,
    @CreatedBy INT = NULL,
    @UpdatedBy INT = NULL,
    @DeletedBy INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Operation IN ('INSERT', 'CREATE')
        BEGIN
            INSERT INTO [dbo].[EmployeeAward]
            (EmployeeId, AwardName, AwardCategory, AwardDate, AwardedBy, Description, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@EmployeeId, @AwardName, @AwardCategory, @AwardDate, @AwardedBy, @Description, @IsEnabled, GETDATE(), @CreatedBy);

            SELECT 1 AS Success, 'Award created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[EmployeeAward]
            SET
                AwardName = ISNULL(@AwardName, AwardName),
                AwardCategory = ISNULL(@AwardCategory, AwardCategory),
                AwardDate = ISNULL(@AwardDate, AwardDate),
                AwardedBy = ISNULL(@AwardedBy, AwardedBy),
                Description = ISNULL(@Description, Description),
                IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(),
                UpdatedBy = @UpdatedBy
            WHERE EmployeeAwardId = @EmployeeAwardId;

            SELECT 1 AS Success, 'Award updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[EmployeeAward]
            SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy
            WHERE EmployeeAwardId = @EmployeeAwardId;

            SELECT 1 AS Success, 'Award deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, 'Something went wrong!' AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetEmployeeAwardByEmployeeId]
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[EmployeeAward]
    WHERE EmployeeId = @EmployeeId AND IsDeleted = 0
    ORDER BY AwardDate DESC;
END
