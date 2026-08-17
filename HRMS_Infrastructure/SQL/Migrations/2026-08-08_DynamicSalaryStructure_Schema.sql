-- Dynamic Salary Structure Template rollout — 2 new tables.
--
-- Context: allowance/payroll math previously lived only as hardcoded T-SQL in
-- USP_CalculateSalaryStructure / USP_CalculateMonthlySalary_V2 (now scripted
-- for the first time under ../StoredProcedures/ as a "before" reference).
-- These two tables let that math be driven by data instead:
--
-- 1. EmployeeSalaryStructureTemplate — maps a specific employee to a
--    SalaryStructureTemplate (the existing Earning/Bonus/Deduction component
--    engine, previously used only for Offer/Recruitment fitment). Effective-
--    dated the same way EmployeeSalaryHistory already tracks Gross/Basic
--    changes (EffectiveFromDate/EffectiveToDate/IsActive) — assigning a new
--    template closes out the prior active row rather than deleting it, so
--    history is preserved. When an employee has no active row here, the
--    resolver stored procedure (GetEffectiveSalaryStructureTemplate, see
--    sp_EmployeeSalaryStructureTemplate_CRUD.sql) falls back to the most
--    specific SalaryStructureTemplate flagged IsDefault=1 for their
--    Grade/Designation/Company.
--
-- 2. CompanyStatutorySetting — one row per company holding today's PF/ESI/
--    Professional Tax/Group Medical thresholds as DATA instead of literals,
--    replacing the hardcoded `IF @CompanyId = 11` branch (which zeroed all
--    of these) with an explicit, inspectable, editable-without-a-deploy
--    settings row per company. Seeded (in a separate script, run after this
--    one) with values that reproduce EVERY existing company's current
--    behavior exactly, so applying this migration changes zero calculated
--    output on its own.

CREATE TABLE [dbo].[EmployeeSalaryStructureTemplate] (
    EmployeeSalaryStructureTemplateId INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    SalaryStructureTemplateId INT NOT NULL,
    CompanyId INT NOT NULL,
    EffectiveFromDate DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    EffectiveToDate DATE NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL
);
CREATE INDEX IX_EmployeeSalaryStructureTemplate_EmployeeId_Active
    ON [dbo].[EmployeeSalaryStructureTemplate] (EmployeeId, IsActive);

CREATE TABLE [dbo].[CompanyStatutorySetting] (
    CompanyStatutorySettingId INT IDENTITY(1,1) PRIMARY KEY,
    CompanyId INT NOT NULL,
    IsPFEnabled BIT NOT NULL DEFAULT 1,
    PFPercentage DECIMAL(5,2) NOT NULL DEFAULT 12.00,
    PFCapAmount DECIMAL(10,2) NOT NULL DEFAULT 1800.00,
    IsESIEnabled BIT NOT NULL DEFAULT 1,
    ESIPercentage DECIMAL(5,4) NOT NULL DEFAULT 0.75,
    EmployerESIPercentage DECIMAL(5,4) NOT NULL DEFAULT 3.25,
    ESIGrossCeiling DECIMAL(10,2) NOT NULL DEFAULT 21000.00,
    IsProfessionalTaxEnabled BIT NOT NULL DEFAULT 1,
    ProfessionalTaxThreshold DECIMAL(10,2) NOT NULL DEFAULT 12000.00,
    ProfessionalTaxAmount DECIMAL(10,2) NOT NULL DEFAULT 200.00,
    IsGroupMedicalEnabled BIT NOT NULL DEFAULT 1,
    GroupMedicalGrossThreshold DECIMAL(10,2) NOT NULL DEFAULT 21000.00,
    GroupMedicalAmount DECIMAL(10,2) NOT NULL DEFAULT 266.00,
    IsTermInsuranceEnabled BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL
);
CREATE UNIQUE INDEX UQ_CompanyStatutorySetting_CompanyId
    ON [dbo].[CompanyStatutorySetting] (CompanyId) WHERE IsDeleted = 0;
