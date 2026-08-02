-- CRUD for OfferSalaryFitment + OfferSalaryBreakupComponent (the per-offer CTC Builder
-- output), called from OfferSalaryFitmentRepository via Dapper. The breakup math itself
-- runs in HRMS_API\Services\SalaryFitmentService.cs (C#), not here.

CREATE OR ALTER PROCEDURE [dbo].[sp_OfferSalaryFitment_CRUD]
    @Operation VARCHAR(10),
    @OfferSalaryFitmentId INT = NULL,
    @CandidateApplicationId INT = NULL,
    @SalaryStructureTemplateId INT = NULL,
    @CurrentCTC DECIMAL(18,2) = NULL,
    @ExpectedCTC DECIMAL(18,2) = NULL,
    @RecommendedCTC DECIMAL(18,2) = NULL,
    @FinalCTC DECIMAL(18,2) = NULL,
    @BudgetValidationStatus VARCHAR(20) = NULL,
    @Remarks VARCHAR(1000) = NULL,
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
            INSERT INTO [dbo].[OfferSalaryFitment]
            (CandidateApplicationId, SalaryStructureTemplateId, CurrentCTC, ExpectedCTC, RecommendedCTC, FinalCTC, BudgetValidationStatus, Remarks, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@CandidateApplicationId, @SalaryStructureTemplateId, @CurrentCTC, @ExpectedCTC, @RecommendedCTC, @FinalCTC, ISNULL(@BudgetValidationStatus, 'NotValidated'), @Remarks, @IsEnabled, GETDATE(), @CreatedBy);

            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Salary fitment saved successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[OfferSalaryFitment]
            SET SalaryStructureTemplateId = @SalaryStructureTemplateId, CurrentCTC = @CurrentCTC, ExpectedCTC = @ExpectedCTC,
                RecommendedCTC = @RecommendedCTC, FinalCTC = @FinalCTC, BudgetValidationStatus = ISNULL(@BudgetValidationStatus, BudgetValidationStatus),
                Remarks = @Remarks, IsEnabled = ISNULL(@IsEnabled, IsEnabled), UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE OfferSalaryFitmentId = @OfferSalaryFitmentId;

            SELECT 1 AS Success, 'Salary fitment updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[OfferSalaryFitment] SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy WHERE OfferSalaryFitmentId = @OfferSalaryFitmentId;
            SELECT 1 AS Success, 'Salary fitment deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetOfferSalaryFitmentByCandidateApplicationId]
    @CandidateApplicationId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 * FROM [dbo].[OfferSalaryFitment] WHERE CandidateApplicationId = @CandidateApplicationId AND IsDeleted = 0 ORDER BY CreatedDate DESC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetOfferSalaryFitmentById]
    @OfferSalaryFitmentId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[OfferSalaryFitment] WHERE OfferSalaryFitmentId = @OfferSalaryFitmentId AND IsDeleted = 0;
END
GO

-- Breakup components are recomputed as a whole every time the CTC Builder runs, so the
-- repository first calls this to clear the previous set, then inserts each fresh row via
-- sp_OfferSalaryBreakupComponent_CRUD below (looped from C#, matching the Phase 1
-- convention of looped single-row inserts for child records rather than a bulk JSON call).
CREATE OR ALTER PROCEDURE [dbo].[DeleteOfferSalaryBreakupComponents]
    @OfferSalaryFitmentId INT,
    @DeletedBy INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [dbo].[OfferSalaryBreakupComponent] SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy
    WHERE OfferSalaryFitmentId = @OfferSalaryFitmentId AND IsDeleted = 0;
    SELECT 1 AS Success, 'Existing breakup cleared successfully!' AS ResponseMessage;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_OfferSalaryBreakupComponent_CRUD]
    @Operation VARCHAR(10),
    @OfferSalaryBreakupComponentId INT = NULL,
    @OfferSalaryFitmentId INT = NULL,
    @ComponentName VARCHAR(50) = NULL,
    @ComponentCategory VARCHAR(20) = NULL,
    @Frequency VARCHAR(20) = 'Monthly',
    @Amount DECIMAL(18,2) = 0,
    @CreatedBy INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @Operation IN ('INSERT', 'CREATE')
        BEGIN
            INSERT INTO [dbo].[OfferSalaryBreakupComponent] (OfferSalaryFitmentId, ComponentName, ComponentCategory, Frequency, Amount, IsEnabled, CreatedDate, CreatedBy)
            VALUES (@OfferSalaryFitmentId, @ComponentName, @ComponentCategory, ISNULL(@Frequency, 'Monthly'), @Amount, 1, GETDATE(), @CreatedBy);
            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Component saved successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetOfferSalaryBreakupByFitmentId]
    @OfferSalaryFitmentId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[OfferSalaryBreakupComponent] WHERE OfferSalaryFitmentId = @OfferSalaryFitmentId AND IsDeleted = 0 ORDER BY OfferSalaryBreakupComponentId;
END
GO
