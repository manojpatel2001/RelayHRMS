-- Combined INSERT/UPDATE/DELETE/GET procedure for EmployeeDesignationHistory (Promotions),
-- called from HRMS_Infrastructure\Repository\Employee\EmployeeDesignationHistoryRepository.cs
-- via Dapper, discriminated by @Operation. Mirrors sp_NewsAnnouncement_CRUD.sql.

CREATE OR ALTER PROCEDURE [dbo].[sp_EmployeeDesignationHistory_CRUD]
    @Operation VARCHAR(10),
    @EmployeeDesignationHistoryId INT = NULL,
    @EmployeeId INT = NULL,
    @OldDesignationId INT = NULL,
    @NewDesignationId INT = NULL,
    @OldGradeId INT = NULL,
    @NewGradeId INT = NULL,
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
            INSERT INTO [dbo].[EmployeeDesignationHistory]
            (EmployeeId, OldDesignationId, NewDesignationId, OldGradeId, NewGradeId, EffectiveDate, Reason, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@EmployeeId, @OldDesignationId, @NewDesignationId, @OldGradeId, @NewGradeId, @EffectiveDate, @Reason, @IsEnabled, GETDATE(), @CreatedBy);

            SELECT 1 AS Success, 'Designation history created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[EmployeeDesignationHistory]
            SET
                OldDesignationId = ISNULL(@OldDesignationId, OldDesignationId),
                NewDesignationId = ISNULL(@NewDesignationId, NewDesignationId),
                OldGradeId = ISNULL(@OldGradeId, OldGradeId),
                NewGradeId = ISNULL(@NewGradeId, NewGradeId),
                EffectiveDate = ISNULL(@EffectiveDate, EffectiveDate),
                Reason = ISNULL(@Reason, Reason),
                IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(),
                UpdatedBy = @UpdatedBy
            WHERE EmployeeDesignationHistoryId = @EmployeeDesignationHistoryId;

            SELECT 1 AS Success, 'Designation history updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[EmployeeDesignationHistory]
            SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy
            WHERE EmployeeDesignationHistoryId = @EmployeeDesignationHistoryId;

            SELECT 1 AS Success, 'Designation history deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, 'Something went wrong!' AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetEmployeeDesignationHistoryByEmployeeId]
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[EmployeeDesignationHistory]
    WHERE EmployeeId = @EmployeeId AND IsDeleted = 0
    ORDER BY EffectiveDate DESC;
END
