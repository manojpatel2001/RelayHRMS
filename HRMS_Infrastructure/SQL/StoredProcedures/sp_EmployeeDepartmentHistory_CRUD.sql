-- Combined INSERT/UPDATE/DELETE/GET procedure for EmployeeDepartmentHistory,
-- called from HRMS_Infrastructure\Repository\Employee\EmployeeDepartmentHistoryRepository.cs
-- via Dapper, discriminated by @Operation. Mirrors sp_NewsAnnouncement_CRUD.sql.

CREATE OR ALTER PROCEDURE [dbo].[sp_EmployeeDepartmentHistory_CRUD]
    @Operation VARCHAR(10),
    @EmployeeDepartmentHistoryId INT = NULL,
    @EmployeeId INT = NULL,
    @OldDepartmentId INT = NULL,
    @NewDepartmentId INT = NULL,
    @EffectiveDate DATE = NULL,
    @Reason VARCHAR(500) = NULL,
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
            INSERT INTO [dbo].[EmployeeDepartmentHistory]
            (EmployeeId, OldDepartmentId, NewDepartmentId, EffectiveDate, Reason, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@EmployeeId, @OldDepartmentId, @NewDepartmentId, @EffectiveDate, @Reason, @IsEnabled, GETDATE(), @CreatedBy);

            SELECT 1 AS Success, 'Department history created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[EmployeeDepartmentHistory]
            SET
                OldDepartmentId = ISNULL(@OldDepartmentId, OldDepartmentId),
                NewDepartmentId = ISNULL(@NewDepartmentId, NewDepartmentId),
                EffectiveDate = ISNULL(@EffectiveDate, EffectiveDate),
                Reason = ISNULL(@Reason, Reason),
                IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(),
                UpdatedBy = @UpdatedBy
            WHERE EmployeeDepartmentHistoryId = @EmployeeDepartmentHistoryId;

            SELECT 1 AS Success, 'Department history updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[EmployeeDepartmentHistory]
            SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy
            WHERE EmployeeDepartmentHistoryId = @EmployeeDepartmentHistoryId;

            SELECT 1 AS Success, 'Department history deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, 'Something went wrong!' AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetEmployeeDepartmentHistoryByEmployeeId]
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[EmployeeDepartmentHistory]
    WHERE EmployeeId = @EmployeeId AND IsDeleted = 0
    ORDER BY EffectiveDate DESC;
END
