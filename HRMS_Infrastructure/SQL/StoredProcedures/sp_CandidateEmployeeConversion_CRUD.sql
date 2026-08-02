-- CRUD for CandidateEmployeeConversion, called from CandidateEmployeeConversionRepository
-- via Dapper. This table is a durable audit/link only — it does not touch the legacy
-- Employee/Identity tables, it just records the outcome of a conversion attempt.

CREATE OR ALTER PROCEDURE [dbo].[sp_CandidateEmployeeConversion_CRUD]
    @Operation VARCHAR(10),
    @CandidateEmployeeConversionId INT = NULL,
    @CandidateApplicationId INT = NULL,
    @EmployeeId INT = NULL,
    @EmployeeCode VARCHAR(50) = NULL,
    @AlfaEmployeeCode VARCHAR(50) = NULL,
    @ConversionStatus VARCHAR(20) = NULL,
    @FailureReason VARCHAR(500) = NULL,
    @CreatedBy INT = NULL,
    @UpdatedBy INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @Operation IN ('INSERT', 'CREATE')
        BEGIN
            INSERT INTO [dbo].[CandidateEmployeeConversion]
            (CandidateApplicationId, EmployeeId, EmployeeCode, AlfaEmployeeCode, ConversionStatus, FailureReason, ConvertedDate, ConvertedBy, CreatedDate, CreatedBy)
            VALUES
            (@CandidateApplicationId, @EmployeeId, @EmployeeCode, @AlfaEmployeeCode, ISNULL(@ConversionStatus, 'Pending'), @FailureReason,
             CASE WHEN @ConversionStatus = 'Completed' THEN GETDATE() ELSE NULL END, @CreatedBy, GETDATE(), @CreatedBy);

            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Conversion record saved successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[CandidateEmployeeConversion]
            SET EmployeeId = @EmployeeId, EmployeeCode = @EmployeeCode, AlfaEmployeeCode = @AlfaEmployeeCode,
                ConversionStatus = ISNULL(@ConversionStatus, ConversionStatus),
                FailureReason = @FailureReason,
                ConvertedDate = CASE WHEN @ConversionStatus = 'Completed' THEN GETDATE() ELSE ConvertedDate END,
                ConvertedBy = ISNULL(@UpdatedBy, ConvertedBy),
                UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE CandidateEmployeeConversionId = @CandidateEmployeeConversionId;

            SELECT 1 AS Success, 'Conversion record updated successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetCandidateEmployeeConversionByApplicationId]
    @CandidateApplicationId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 * FROM [dbo].[CandidateEmployeeConversion] WHERE CandidateApplicationId = @CandidateApplicationId ORDER BY CreatedDate DESC;
END
GO
