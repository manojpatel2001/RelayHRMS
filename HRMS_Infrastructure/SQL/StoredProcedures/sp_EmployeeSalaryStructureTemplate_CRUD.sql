-- CRUD + resolver for EmployeeSalaryStructureTemplate — the employee-wise
-- mapping onto the existing SalaryStructureTemplate engine (see
-- sp_SalaryStructure_CRUD.sql). Called from
-- EmployeeSalaryStructureTemplateRepository via Dapper, same convention as
-- SalaryStructureRepository.
--
-- QUOTED_IDENTIFIER must be ON at CREATE time — EmployeeSalaryStructureTemplate
-- has a filtered/regular index, and a proc compiled with it OFF fails at
-- runtime on every INSERT/UPDATE with error 1934.
SET QUOTED_IDENTIFIER ON;
GO

-- Assigning a new template to an employee closes out any existing active
-- mapping row (EffectiveToDate = yesterday, IsActive = 0) rather than
-- deleting it, mirroring how EmployeeSalaryHistory tracks Gross/Basic changes
-- over time.
CREATE OR ALTER PROCEDURE [dbo].[sp_EmployeeSalaryStructureTemplate_CRUD]
    @Operation VARCHAR(10),
    @EmployeeSalaryStructureTemplateId INT = NULL,
    @EmployeeId INT = NULL,
    @SalaryStructureTemplateId INT = NULL,
    @CompanyId INT = NULL,
    @CreatedBy INT = NULL,
    @UpdatedBy INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @Operation IN ('INSERT', 'CREATE', 'ASSIGN')
        BEGIN
            UPDATE [dbo].[EmployeeSalaryStructureTemplate]
            SET IsActive = 0, EffectiveToDate = CAST(GETDATE() AS DATE), UpdatedDate = GETDATE(), UpdatedBy = @CreatedBy
            WHERE EmployeeId = @EmployeeId AND IsActive = 1 AND IsDeleted = 0;

            INSERT INTO [dbo].[EmployeeSalaryStructureTemplate]
                (EmployeeId, SalaryStructureTemplateId, CompanyId, EffectiveFromDate, IsActive, IsEnabled, CreatedDate, CreatedBy)
            VALUES
                (@EmployeeId, @SalaryStructureTemplateId, @CompanyId, CAST(GETDATE() AS DATE), 1, 1, GETDATE(), @CreatedBy);

            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Salary structure template assigned successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UNASSIGN'
        BEGIN
            UPDATE [dbo].[EmployeeSalaryStructureTemplate]
            SET IsActive = 0, EffectiveToDate = CAST(GETDATE() AS DATE), UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE EmployeeId = @EmployeeId AND IsActive = 1 AND IsDeleted = 0;

            SELECT 1 AS Success, 'Employee reverted to grade-default template.' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetEmployeeSalaryStructureTemplateByEmployeeId]
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT est.*, t.TemplateName
    FROM [dbo].[EmployeeSalaryStructureTemplate] est
    INNER JOIN [dbo].[SalaryStructureTemplate] t ON t.SalaryStructureTemplateId = est.SalaryStructureTemplateId
    WHERE est.EmployeeId = @EmployeeId AND est.IsActive = 1 AND est.IsDeleted = 0;
END
GO

-- Resolves the template that should actually be used for an employee right
-- now: an explicit employee-level mapping always wins; otherwise falls back
-- to the most specific Grade/Designation default template for the company;
-- returns zero rows if nothing is configured (caller must not silently guess
-- at that point — see SalaryStructureCalculationService).
CREATE OR ALTER PROCEDURE [dbo].[GetEffectiveSalaryStructureTemplate]
    @EmployeeId INT,
    @CompanyId INT,
    @GradeId INT = NULL,
    @DesignationId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ResolvedTemplateId INT;
    DECLARE @Source VARCHAR(20);

    SELECT TOP 1 @ResolvedTemplateId = est.SalaryStructureTemplateId, @Source = 'Employee'
    FROM [dbo].[EmployeeSalaryStructureTemplate] est
    WHERE est.EmployeeId = @EmployeeId AND est.IsActive = 1 AND est.IsDeleted = 0;

    IF @ResolvedTemplateId IS NULL
    BEGIN
        SELECT TOP 1 @ResolvedTemplateId = t.SalaryStructureTemplateId, @Source = 'GradeDefault'
        FROM [dbo].[SalaryStructureTemplate] t
        WHERE t.CompanyId = @CompanyId AND t.IsDefault = 1 AND t.IsEnabled = 1 AND t.IsDeleted = 0
          AND (
                (t.GradeId = @GradeId AND t.DesignationId = @DesignationId)
             OR (t.GradeId = @GradeId AND t.DesignationId IS NULL)
             OR (t.GradeId IS NULL AND t.DesignationId IS NULL)
          )
        ORDER BY
            CASE
                WHEN t.GradeId = @GradeId AND t.DesignationId = @DesignationId THEN 1
                WHEN t.GradeId = @GradeId AND t.DesignationId IS NULL THEN 2
                WHEN t.GradeId IS NULL AND t.DesignationId IS NULL THEN 3
                ELSE 4
            END;
    END

    SELECT @ResolvedTemplateId AS SalaryStructureTemplateId, @Source AS ResolvedFrom;
END
GO
