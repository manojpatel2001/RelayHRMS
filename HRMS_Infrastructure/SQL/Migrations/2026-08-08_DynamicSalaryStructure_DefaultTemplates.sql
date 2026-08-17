-- Seeds the actual default Salary Structure Templates the Dynamic Salary
-- Structure Template rollout depends on. Without these, no employee resolves
-- to any template (no EmployeeSalaryStructureTemplate mapping exists yet for
-- most employees, and until this script runs, no SalaryStructureTemplate has
-- IsDefault=1 either) — the new engine would report "no template configured"
-- for almost everyone. This script reproduces the OLD USP_CalculateSalaryStructure
-- / USP_CalculateMonthlySalary_V2 formulas as data, company-wise, so switching
-- the calculation engine over changes ZERO calculated output on its own.
--
-- Two template shapes:
--   1. "Standard Salary Structure (Legacy Match)" — one per company except 11:
--      Basic = 40% of Gross, HRA = 40% of Basic, Conveyance = flat 1600,
--      Child Education = flat 200, Medical = flat 1250, Deputation = remainder.
--      Set as the company-wide IsDefault=1 template (GradeId/DesignationId NULL).
--   2. "Zero Allowance (Statutory Minimum)" — one template, CompanyId=11:
--      Basic = 100% of Gross, nothing else. Set as Company 11's IsDefault=1
--      template (replacing the old `IF @CompanyId = 11` branch), AND also
--      explicitly mapped to EmployeeId 165 and 218 (both CompanyId=9) via
--      EmployeeSalaryStructureTemplate — replacing the old literal
--      `@ZeroAllowanceEmployees TABLE VALUES (165), (218)` override, since an
--      explicit employee mapping always wins over any company/grade default.

SET QUOTED_IDENTIFIER ON;
SET NOCOUNT ON;

DECLARE @CompanyIds TABLE (CompanyId INT);
INSERT INTO @CompanyIds VALUES (8), (9), (10), (12), (13), (14); -- every company except 11

DECLARE @CompanyId INT, @TemplateId INT;

DECLARE company_cursor CURSOR LOCAL FAST_FORWARD FOR SELECT CompanyId FROM @CompanyIds;
OPEN company_cursor;
FETCH NEXT FROM company_cursor INTO @CompanyId;

WHILE @@FETCH_STATUS = 0
BEGIN
    IF NOT EXISTS (SELECT 1 FROM SalaryStructureTemplate WHERE CompanyId = @CompanyId AND IsDefault = 1 AND IsDeleted = 0)
    BEGIN
        INSERT INTO SalaryStructureTemplate (TemplateName, GradeId, DesignationId, CompanyId, IsDefault, Status, IsEnabled, CreatedDate)
        VALUES ('Standard Salary Structure (Legacy Match)', NULL, NULL, @CompanyId, 1, 'Active', 1, GETDATE());

        SET @TemplateId = SCOPE_IDENTITY();

        INSERT INTO SalaryStructureComponent (SalaryStructureTemplateId, ComponentName, ComponentCategory, CalculationType, Value, SortOrder, IsEnabled, CreatedDate)
        VALUES
            (@TemplateId, 'Basic', 'Earning', 'PercentOfCTC', 40.0000, 10, 1, GETDATE()),
            (@TemplateId, 'Child Education Allowance', 'Earning', 'FlatAmount', 200.0000, 20, 1, GETDATE()),
            (@TemplateId, 'Conveyance Allowance', 'Earning', 'FlatAmount', 1600.0000, 30, 1, GETDATE()),
            (@TemplateId, 'HRA', 'Earning', 'PercentOfBasic', 40.0000, 40, 1, GETDATE()),
            (@TemplateId, 'Medical Allowance', 'Earning', 'FlatAmount', 1250.0000, 50, 1, GETDATE()),
            (@TemplateId, 'Deputation Allowance', 'Earning', 'RemainderOfGross', 0.0000, 60, 1, GETDATE());
    END

    FETCH NEXT FROM company_cursor INTO @CompanyId;
END
CLOSE company_cursor;
DEALLOCATE company_cursor;

-- Company 11's own default (replaces `IF @CompanyId = 11` in both old SPs)
DECLARE @ZeroAllowanceTemplateId INT;

IF NOT EXISTS (SELECT 1 FROM SalaryStructureTemplate WHERE CompanyId = 11 AND IsDefault = 1 AND IsDeleted = 0)
BEGIN
    INSERT INTO SalaryStructureTemplate (TemplateName, GradeId, DesignationId, CompanyId, IsDefault, Status, IsEnabled, CreatedDate)
    VALUES ('Zero Allowance (Statutory Minimum)', NULL, NULL, 11, 1, 'Active', 1, GETDATE());

    SET @ZeroAllowanceTemplateId = SCOPE_IDENTITY();

    INSERT INTO SalaryStructureComponent (SalaryStructureTemplateId, ComponentName, ComponentCategory, CalculationType, Value, SortOrder, IsEnabled, CreatedDate)
    VALUES (@ZeroAllowanceTemplateId, 'Basic', 'Earning', 'PercentOfCTC', 100.0000, 10, 1, GETDATE());
END
ELSE
BEGIN
    SELECT @ZeroAllowanceTemplateId = SalaryStructureTemplateId FROM SalaryStructureTemplate WHERE CompanyId = 11 AND IsDefault = 1 AND IsDeleted = 0;
END

-- Explicit legacy overrides: EmployeeId 165 and 218 (both CompanyId=9) always
-- resolved to zero allowance regardless of company under the old SP — an
-- explicit employee-level mapping reproduces that without touching Company 9's
-- own default template for everyone else.
IF NOT EXISTS (SELECT 1 FROM EmployeeSalaryStructureTemplate WHERE EmployeeId = 165 AND IsActive = 1 AND IsDeleted = 0)
    INSERT INTO EmployeeSalaryStructureTemplate (EmployeeId, SalaryStructureTemplateId, CompanyId, EffectiveFromDate, IsActive, IsEnabled, CreatedDate)
    VALUES (165, @ZeroAllowanceTemplateId, 9, CAST(GETDATE() AS DATE), 1, 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM EmployeeSalaryStructureTemplate WHERE EmployeeId = 218 AND IsActive = 1 AND IsDeleted = 0)
    INSERT INTO EmployeeSalaryStructureTemplate (EmployeeId, SalaryStructureTemplateId, CompanyId, EffectiveFromDate, IsActive, IsEnabled, CreatedDate)
    VALUES (218, @ZeroAllowanceTemplateId, 9, CAST(GETDATE() AS DATE), 1, 1, GETDATE());

SELECT SalaryStructureTemplateId, TemplateName, CompanyId, IsDefault FROM SalaryStructureTemplate WHERE IsDeleted = 0 ORDER BY CompanyId;
