-- Combined INSERT/UPDATE/DELETE/GET procedure for EmployeeTraining,
-- called from HRMS_Infrastructure\Repository\Employee\EmployeeTrainingRepository.cs
-- via Dapper, discriminated by @Operation. Mirrors sp_NewsAnnouncement_CRUD.sql.

CREATE OR ALTER PROCEDURE [dbo].[sp_EmployeeTraining_CRUD]
    @Operation VARCHAR(10),
    @EmployeeTrainingId INT = NULL,
    @EmployeeId INT = NULL,
    @TrainingName VARCHAR(200) = NULL,
    @TrainingType VARCHAR(100) = NULL,
    @StartDate DATE = NULL,
    @EndDate DATE = NULL,
    @Provider VARCHAR(200) = NULL,
    @Status VARCHAR(20) = NULL,
    @Score VARCHAR(20) = NULL,
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
            INSERT INTO [dbo].[EmployeeTraining]
            (EmployeeId, TrainingName, TrainingType, StartDate, EndDate, Provider, Status, Score, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@EmployeeId, @TrainingName, @TrainingType, @StartDate, @EndDate, @Provider, ISNULL(@Status, 'Scheduled'), @Score, @IsEnabled, GETDATE(), @CreatedBy);

            SELECT 1 AS Success, 'Training record created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[EmployeeTraining]
            SET
                TrainingName = ISNULL(@TrainingName, TrainingName),
                TrainingType = ISNULL(@TrainingType, TrainingType),
                StartDate = ISNULL(@StartDate, StartDate),
                EndDate = ISNULL(@EndDate, EndDate),
                Provider = ISNULL(@Provider, Provider),
                Status = ISNULL(@Status, Status),
                Score = ISNULL(@Score, Score),
                IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(),
                UpdatedBy = @UpdatedBy
            WHERE EmployeeTrainingId = @EmployeeTrainingId;

            SELECT 1 AS Success, 'Training record updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[EmployeeTraining]
            SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy
            WHERE EmployeeTrainingId = @EmployeeTrainingId;

            SELECT 1 AS Success, 'Training record deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, 'Something went wrong!' AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetEmployeeTrainingByEmployeeId]
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[EmployeeTraining]
    WHERE EmployeeId = @EmployeeId AND IsDeleted = 0
    ORDER BY StartDate DESC;
END
