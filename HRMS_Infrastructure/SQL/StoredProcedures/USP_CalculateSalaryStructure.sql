-- ============================================================================
-- USP_CalculateSalaryStructure
--
-- Snapshot of the LIVE production definition, scripted into source control
-- for the first time as of 2026-08-08 (this proc previously existed only in
-- the database, unversioned). This is the "before" reference for the
-- Dynamic Salary Structure Template rollout (see
-- HRMS_Infrastructure\SQL\StoredProcedures\sp_EmployeeSalaryStructureTemplate_CRUD.sql
-- and SalaryStructureCalculationService.cs) — do NOT hand-edit this copy and
-- assume it matches the DB; always re-pull OBJECT_DEFINITION before altering
-- live, and update this file to match immediately after any live ALTER.
--
-- Known issues this rollout replaces (kept here only as historical record):
--   * Hardcoded `IF @CompanyId = 11` branch (zero allowances, no PF/ESI/PT).
--   * Hardcoded flat amounts (Conveyance 1600, ChildEducation 200, Medical
--     1250) and hardcoded percentages (Basic = 40% of Gross, HRA = 40% of
--     Basic) instead of a configurable Salary Structure Template.
-- ============================================================================
CREATE OR ALTER PROCEDURE [dbo].[USP_CalculateSalaryStructure]
    @Action NVARCHAR(10),
    @GrossSalary DECIMAL(10,2),
    @EmployeeId INT = 0,
    @CompanyId INT = 0,
    @BasicSalary DECIMAL(10,2),
    @IsPFApplicable INT = 0
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DECLARE
            @CalculatedBasicSalary DECIMAL(10,2),
            @ChildEducationAllowance DECIMAL(10,2),
            @ConveyanceAllowance DECIMAL(10,2),
            @HRA DECIMAL(10,2),
            @MedicalAllowance DECIMAL(10,2),
            @DeputationAllowance DECIMAL(10,2),
            @TotalGrossSalary DECIMAL(10,2),

            @PF DECIMAL(10,2),
            @ESI DECIMAL(10,2),
            @ProfessionalTax DECIMAL(10,2),
            @GroupMedical DECIMAL(10,2),
            @TermInsurance DECIMAL(10,2),
            @TotalDeductions DECIMAL(10,2),
            @NetSalary DECIMAL(10,2),

            @EmployerPF DECIMAL(10,2),
            @EmployerESI DECIMAL(10,2),
            @CTC DECIMAL(10,2),
            @CreatedDate DATETIME = GETUTCDATE(),
            @IsDeleted BIT = 0,
            @IsEnabled BIT = 1,

            @ProcessingDate DATE = CAST(GETUTCDATE() AS DATE);

        -----------------------------------------------------------------
        -- CompanyId = 11 Logic
        -----------------------------------------------------------------
        IF @CompanyId = 11
        BEGIN
            SET @CalculatedBasicSalary      = @GrossSalary;
            SET @ChildEducationAllowance    = 0;
            SET @ConveyanceAllowance        = 0;
            SET @HRA                        = 0;
            SET @MedicalAllowance           = 0;
            SET @DeputationAllowance        = 0;
            SET @TotalGrossSalary           = @GrossSalary;

            SET @PF                         = 0;
            SET @ESI                        = 0;
            SET @ProfessionalTax            = 0;

            -- APPLY Group Medical
            SET @GroupMedical = CASE
                                    WHEN @GrossSalary >= 21000 THEN 266
                                    ELSE 0
                                END;

            -- APPLY Term Insurance (same as Monthly SP)
            SET @TermInsurance = dbo.fn_GetTermInsuranceDeduction(
                                    @GrossSalary,
                                    @ProcessingDate,
                                    @CompanyId
                                );

            -- ONLY applicable deductions
            SET @TotalDeductions = ROUND(@GroupMedical + @TermInsurance, 0);

            SET @NetSalary = ROUND(@GrossSalary - @TotalDeductions, 0);

            SET @EmployerPF = 0;
            SET @EmployerESI = 0;
            SET @CTC = @GrossSalary;
        END
        ELSE
        BEGIN
            IF @BasicSalary = 0
                SET @CalculatedBasicSalary = ROUND(@GrossSalary * 0.40, 0);
            ELSE
                SET @CalculatedBasicSalary = @BasicSalary;

            SET @ChildEducationAllowance = 200;
            SET @ConveyanceAllowance     = 1600;
            SET @HRA                     = ROUND(@CalculatedBasicSalary * 0.40, 0);
            SET @MedicalAllowance        = 1250;

            SET @DeputationAllowance = ROUND(
                @GrossSalary
                - @CalculatedBasicSalary
                - @ChildEducationAllowance
                - @ConveyanceAllowance
                - @HRA
                - @MedicalAllowance, 0);

            SET @TotalGrossSalary = @GrossSalary;

            SET @PF = CASE
                        WHEN @IsPFApplicable = 1 THEN
                            ROUND(CASE
                                WHEN @CalculatedBasicSalary * 0.12 > 1800 THEN 1800
                                ELSE @CalculatedBasicSalary * 0.12
                            END, 0)
                        ELSE 0
                      END;

            SET @ESI = ROUND(CASE
                            WHEN @GrossSalary <= 21000 THEN @GrossSalary * 0.0075
                            ELSE 0
                         END, 0);

            SET @ProfessionalTax = CASE
                                     WHEN @GrossSalary > 12000 THEN 200
                                     ELSE 0
                                   END;

            SET @GroupMedical = CASE
                                  WHEN @GrossSalary >= 21000 THEN 266
                                  ELSE 0
                                END;

            SET @TermInsurance = dbo.fn_GetTermInsuranceDeduction(
                                    @GrossSalary,
                                    @ProcessingDate,
                                    @CompanyId
                                );

            SET @TotalDeductions = ROUND(@PF + @ESI + @ProfessionalTax + @GroupMedical + @TermInsurance, 0);
            SET @NetSalary       = ROUND(@TotalGrossSalary - @TotalDeductions, 0);

            SET @EmployerPF = CASE
                                WHEN @IsPFApplicable = 1 THEN
                                    ROUND(CASE
                                        WHEN @CalculatedBasicSalary * 0.12 > 1800 THEN 1800
                                        ELSE @CalculatedBasicSalary * 0.12
                                    END, 0)
                                ELSE 0
                              END;

            SET @EmployerESI = ROUND(CASE
                                   WHEN @GrossSalary <= 21000 THEN @GrossSalary * 0.0325
                                   ELSE 0
                                END, 0);

            SET @CTC = ROUND(@GrossSalary + @EmployerPF + @EmployerESI, 0);
        END

        -----------------------------------------------------------------
        -- ACTION: GET
        -----------------------------------------------------------------
        IF (@Action = 'GET')
        BEGIN
            SELECT
                @CalculatedBasicSalary      AS BasicSalary,
                @ChildEducationAllowance    AS ChildEducationAllowance,
                @ConveyanceAllowance        AS ConveyanceAllowance,
                @HRA                        AS HRA,
                @MedicalAllowance           AS MedicalAllowance,
                @DeputationAllowance        AS DeputationAllowance,
                @TotalGrossSalary           AS TotalGrossSalary,
                @PF                         AS EmployeePF,
                @ESI                        AS EmployeeESI,
                @ProfessionalTax            AS ProfessionalTax,
                @GroupMedical               AS GroupMedical,
                @TermInsurance              AS TermInsurance,
                @TotalDeductions            AS TotalDeductions,
                @NetSalary                  AS NetSalary,
                @EmployerPF                 AS EmployerPF,
                @EmployerESI                AS EmployerESI,
                @CTC                        AS CTC;
        END;

        -----------------------------------------------------------------
        -- ACTION: CREATE
        -----------------------------------------------------------------
        IF (@Action = 'CREATE')
        BEGIN
            IF NOT EXISTS (
                SELECT 1 FROM EmployeeSalaryAllowance
                WHERE EmployeeId = @EmployeeId
                  AND CompanyId  = @CompanyId
                  AND IsDeleted  = 0
                  AND IsEnabled  = 1
            )
            BEGIN
                INSERT INTO EmployeeSalaryAllowance (
                    EmployeeId, CompanyId,
                    BasicSalary, ChildEducationAllowance, ConveyanceAllowance, HRA,
                    MedicalAllowance, DeputationAllowance, TotalGrossSalary,
                    EmployeePF, EmployeeESI, ProfessionalTax, GroupMedical, TermInsurance,
                    TotalDeductions, NetSalary,
                    EmployerPF, EmployerESI, CTC, CreatedDate, IsEnabled, IsDeleted
                )
                VALUES (
                    @EmployeeId, @CompanyId,
                    @CalculatedBasicSalary, @ChildEducationAllowance, @ConveyanceAllowance, @HRA,
                    @MedicalAllowance, @DeputationAllowance, @TotalGrossSalary,
                    @PF, @ESI, @ProfessionalTax, @GroupMedical, @TermInsurance,
                    @TotalDeductions, @NetSalary,
                    @EmployerPF, @EmployerESI, @CTC, @CreatedDate, @IsEnabled, @IsDeleted
                );
            END
            SELECT @EmployeeId AS Id;
        END;

        -----------------------------------------------------------------
        -- ACTION: UPDATE
        -----------------------------------------------------------------
        IF (@Action = 'UPDATE')
        BEGIN
            UPDATE EmployeeSalaryAllowance
            SET
                CompanyId                = @CompanyId,
                BasicSalary              = @CalculatedBasicSalary,
                ChildEducationAllowance  = @ChildEducationAllowance,
                ConveyanceAllowance      = @ConveyanceAllowance,
                HRA                      = @HRA,
                MedicalAllowance         = @MedicalAllowance,
                DeputationAllowance      = @DeputationAllowance,
                TotalGrossSalary         = @TotalGrossSalary,
                EmployeePF               = @PF,
                EmployeeESI              = @ESI,
                ProfessionalTax          = @ProfessionalTax,
                GroupMedical             = @GroupMedical,
                TermInsurance            = @TermInsurance,
                TotalDeductions          = @TotalDeductions,
                NetSalary                = @NetSalary,
                EmployerPF               = @EmployerPF,
                EmployerESI              = @EmployerESI,
                CTC                      = @CTC
            WHERE EmployeeId = @EmployeeId AND IsDeleted = 0 AND IsEnabled = 1;

            SELECT @EmployeeId AS Id;


			--increment history update
            UPDATE esh
			SET
				esh.HRA                     = esa.HRA,
				esh.ConveyanceAllowance     = esa.ConveyanceAllowance,
				esh.ChildEducationAllowance = esa.ChildEducationAllowance,
				esh.MedicalAllowance        = esa.MedicalAllowance,
				esh.DeputationAllowance     = esa.DeputationAllowance,
				esh.TotalGrossSalary        = esa.TotalGrossSalary,
				esh.EmployeePF              = esa.EmployeePF,
				esh.EmployeeESI             = esa.EmployeeESI,
				esh.ProfessionalTax         = esa.ProfessionalTax,
				esh.GroupMedical            = esa.GroupMedical,
				esh.TermInsurance           = esa.TermInsurance,
				esh.TotalDeductions         = esa.TotalDeductions,
				esh.NetSalary               = esa.NetSalary,
				esh.EmployerPF              = esa.EmployerPF,
				esh.EmployerESI             = esa.EmployerESI,
				esh.CTC                     = esa.CTC
			FROM EmployeeSalaryHistory esh
			INNER JOIN EmployeeSalaryAllowance esa
				ON esa.EmployeeId = esh.EmployeeId
			   AND esa.IsEnabled = 1
			   AND esa.IsDeleted = 0
			WHERE esh.EmployeeId = @EmployeeId
			  AND esh.IsActive = 1
			  -- Sirf current active (open-ended) record update karo
			  AND esh.EffectiveToDate IS NULL;

        END;

        -----------------------------------------------------------------
        -- ACTION: DELETE
        -----------------------------------------------------------------
        IF (@Action = 'DELETE')
        BEGIN
            UPDATE EmployeeSalaryAllowance
            SET IsEnabled = 0, IsDeleted = 1
            WHERE EmployeeId = @EmployeeId AND IsDeleted = 0 AND IsEnabled = 1;

            SELECT @EmployeeId AS Id;
        END;

    END TRY
    BEGIN CATCH
        SELECT 0 AS Id;
    END CATCH
END
