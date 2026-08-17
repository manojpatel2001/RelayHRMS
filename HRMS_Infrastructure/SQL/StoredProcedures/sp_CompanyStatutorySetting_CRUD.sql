-- CRUD for CompanyStatutorySetting — per-company PF/ESI/Professional Tax/
-- Group Medical/Term Insurance applicability + thresholds (see
-- 2026-08-08_DynamicSalaryStructure_Schema.sql for context). Replaces the
-- hardcoded `IF @CompanyId = 11` branch in USP_CalculateSalaryStructure /
-- USP_CalculateMonthlySalary_V2 with data. Called from
-- CompanyStatutorySettingRepository via Dapper.
--
-- QUOTED_IDENTIFIER must be ON at CREATE time — CompanyStatutorySetting has a
-- filtered unique index, and a proc compiled with it OFF fails at runtime on
-- every INSERT/UPDATE with error 1934.
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_CompanyStatutorySetting_CRUD]
    @Operation VARCHAR(10),
    @CompanyStatutorySettingId INT = NULL,
    @CompanyId INT = NULL,
    @IsPFEnabled BIT = 1,
    @PFPercentage DECIMAL(5,2) = 12.00,
    @PFCapAmount DECIMAL(10,2) = 1800.00,
    @IsESIEnabled BIT = 1,
    @ESIPercentage DECIMAL(5,4) = 0.75,
    @EmployerESIPercentage DECIMAL(5,4) = 3.25,
    @ESIGrossCeiling DECIMAL(10,2) = 21000.00,
    @IsProfessionalTaxEnabled BIT = 1,
    @ProfessionalTaxThreshold DECIMAL(10,2) = 12000.00,
    @ProfessionalTaxAmount DECIMAL(10,2) = 200.00,
    @IsGroupMedicalEnabled BIT = 1,
    @GroupMedicalGrossThreshold DECIMAL(10,2) = 21000.00,
    @GroupMedicalAmount DECIMAL(10,2) = 266.00,
    @IsTermInsuranceEnabled BIT = 1,
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
            IF EXISTS (SELECT 1 FROM [dbo].[CompanyStatutorySetting] WHERE CompanyId = @CompanyId AND IsDeleted = 0)
            BEGIN
                SELECT 0 AS Success, 'A statutory setting already exists for this company — use Update instead.' AS ResponseMessage;
                RETURN;
            END

            INSERT INTO [dbo].[CompanyStatutorySetting] (
                CompanyId, IsPFEnabled, PFPercentage, PFCapAmount, IsESIEnabled, ESIPercentage, EmployerESIPercentage, ESIGrossCeiling,
                IsProfessionalTaxEnabled, ProfessionalTaxThreshold, ProfessionalTaxAmount,
                IsGroupMedicalEnabled, GroupMedicalGrossThreshold, GroupMedicalAmount,
                IsTermInsuranceEnabled, IsEnabled, CreatedDate, CreatedBy
            )
            VALUES (
                @CompanyId, @IsPFEnabled, @PFPercentage, @PFCapAmount, @IsESIEnabled, @ESIPercentage, @EmployerESIPercentage, @ESIGrossCeiling,
                @IsProfessionalTaxEnabled, @ProfessionalTaxThreshold, @ProfessionalTaxAmount,
                @IsGroupMedicalEnabled, @GroupMedicalGrossThreshold, @GroupMedicalAmount,
                @IsTermInsuranceEnabled, @IsEnabled, GETDATE(), @CreatedBy
            );
            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Statutory setting created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[CompanyStatutorySetting]
            SET IsPFEnabled = @IsPFEnabled, PFPercentage = @PFPercentage, PFCapAmount = @PFCapAmount,
                IsESIEnabled = @IsESIEnabled, ESIPercentage = @ESIPercentage, EmployerESIPercentage = @EmployerESIPercentage, ESIGrossCeiling = @ESIGrossCeiling,
                IsProfessionalTaxEnabled = @IsProfessionalTaxEnabled, ProfessionalTaxThreshold = @ProfessionalTaxThreshold, ProfessionalTaxAmount = @ProfessionalTaxAmount,
                IsGroupMedicalEnabled = @IsGroupMedicalEnabled, GroupMedicalGrossThreshold = @GroupMedicalGrossThreshold, GroupMedicalAmount = @GroupMedicalAmount,
                IsTermInsuranceEnabled = @IsTermInsuranceEnabled, IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE CompanyStatutorySettingId = @CompanyStatutorySettingId;
            SELECT 1 AS Success, 'Statutory setting updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[CompanyStatutorySetting] SET IsDeleted = 1, IsEnabled = 0, UpdatedDate = GETDATE(), UpdatedBy = @DeletedBy WHERE CompanyStatutorySettingId = @CompanyStatutorySettingId;
            SELECT 1 AS Success, 'Statutory setting deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetAllCompanyStatutorySettings]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT s.*, c.CompanyName
    FROM [dbo].[CompanyStatutorySetting] s
    LEFT JOIN [dbo].[CompanyDetails] c ON c.CompanyId = s.CompanyId
    WHERE s.IsDeleted = 0
    ORDER BY c.CompanyName;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetCompanyStatutorySettingByCompanyId]
    @CompanyId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 * FROM [dbo].[CompanyStatutorySetting] WHERE CompanyId = @CompanyId AND IsDeleted = 0 AND IsEnabled = 1;
END
GO
