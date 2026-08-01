-- Combined INSERT/UPDATE/DELETE/GET procedure for EmployeeShiftHistory,
-- called from HRMS_Infrastructure\Repository\Employee\EmployeeShiftHistoryRepository.cs
-- via Dapper, discriminated by @Operation. Mirrors sp_NewsAnnouncement_CRUD.sql.

CREATE OR ALTER PROCEDURE [dbo].[sp_EmployeeShiftHistory_CRUD]
    @Operation VARCHAR(10),
    @EmployeeShiftHistoryId INT = NULL,
    @EmployeeId INT = NULL,
    @OldShiftMasterId INT = NULL,
    @NewShiftMasterId INT = NULL,
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
            INSERT INTO [dbo].[EmployeeShiftHistory]
            (EmployeeId, OldShiftMasterId, NewShiftMasterId, EffectiveDate, Reason, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@EmployeeId, @OldShiftMasterId, @NewShiftMasterId, @EffectiveDate, @Reason, @IsEnabled, GETDATE(), @CreatedBy);

            SELECT 1 AS Success, 'Shift history created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[EmployeeShiftHistory]
            SET
                OldShiftMasterId = ISNULL(@OldShiftMasterId, OldShiftMasterId),
                NewShiftMasterId = ISNULL(@NewShiftMasterId, NewShiftMasterId),
                EffectiveDate = ISNULL(@EffectiveDate, EffectiveDate),
                Reason = ISNULL(@Reason, Reason),
                IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(),
                UpdatedBy = @UpdatedBy
            WHERE EmployeeShiftHistoryId = @EmployeeShiftHistoryId;

            SELECT 1 AS Success, 'Shift history updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[EmployeeShiftHistory]
            SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy
            WHERE EmployeeShiftHistoryId = @EmployeeShiftHistoryId;

            SELECT 1 AS Success, 'Shift history deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, 'Something went wrong!' AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetEmployeeShiftHistoryByEmployeeId]
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[EmployeeShiftHistory]
    WHERE EmployeeId = @EmployeeId AND IsDeleted = 0
    ORDER BY EffectiveDate DESC;
END
