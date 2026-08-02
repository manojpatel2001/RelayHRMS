-- CRUD for SalaryStructureTemplate + SalaryStructureComponent (the self-contained CTC
-- rule engine), called from SalaryStructureRepository via Dapper.

CREATE OR ALTER PROCEDURE [dbo].[sp_SalaryStructureTemplate_CRUD]
    @Operation VARCHAR(10),
    @SalaryStructureTemplateId INT = NULL,
    @TemplateName VARCHAR(150) = NULL,
    @GradeId INT = NULL,
    @DesignationId INT = NULL,
    @CompanyId INT = NULL,
    @IsDefault BIT = 0,
    @Status VARCHAR(20) = NULL,
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
            INSERT INTO [dbo].[SalaryStructureTemplate] (TemplateName, GradeId, DesignationId, CompanyId, IsDefault, Status, IsEnabled, CreatedDate, CreatedBy)
            VALUES (@TemplateName, @GradeId, @DesignationId, @CompanyId, @IsDefault, ISNULL(@Status, 'Active'), @IsEnabled, GETDATE(), @CreatedBy);
            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Salary structure template created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[SalaryStructureTemplate]
            SET TemplateName = ISNULL(@TemplateName, TemplateName), GradeId = @GradeId, DesignationId = @DesignationId,
                IsDefault = ISNULL(@IsDefault, IsDefault), Status = ISNULL(@Status, Status), IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE SalaryStructureTemplateId = @SalaryStructureTemplateId;
            SELECT 1 AS Success, 'Salary structure template updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[SalaryStructureTemplate] SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy WHERE SalaryStructureTemplateId = @SalaryStructureTemplateId;
            SELECT 1 AS Success, 'Salary structure template deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetAllSalaryStructureTemplates]
    @CompanyId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT t.*, g.GradeName, d.DesignationName
    FROM [dbo].[SalaryStructureTemplate] t
    LEFT JOIN [dbo].[Grade] g ON g.GradeId = t.GradeId
    LEFT JOIN [dbo].[Designation] d ON d.DesignationId = t.DesignationId
    WHERE t.IsDeleted = 0 AND (@CompanyId IS NULL OR t.CompanyId = @CompanyId)
    ORDER BY t.TemplateName;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetSalaryStructureTemplateById]
    @SalaryStructureTemplateId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[SalaryStructureTemplate] WHERE SalaryStructureTemplateId = @SalaryStructureTemplateId AND IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_SalaryStructureComponent_CRUD]
    @Operation VARCHAR(10),
    @SalaryStructureComponentId INT = NULL,
    @SalaryStructureTemplateId INT = NULL,
    @ComponentName VARCHAR(50) = NULL,
    @ComponentCategory VARCHAR(20) = NULL,
    @CalculationType VARCHAR(20) = NULL,
    @Value DECIMAL(9,4) = NULL,
    @SortOrder INT = 0,
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
            INSERT INTO [dbo].[SalaryStructureComponent] (SalaryStructureTemplateId, ComponentName, ComponentCategory, CalculationType, Value, SortOrder, IsEnabled, CreatedDate, CreatedBy)
            VALUES (@SalaryStructureTemplateId, @ComponentName, @ComponentCategory, @CalculationType, @Value, @SortOrder, @IsEnabled, GETDATE(), @CreatedBy);
            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Component saved successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[SalaryStructureComponent]
            SET ComponentName = ISNULL(@ComponentName, ComponentName), ComponentCategory = ISNULL(@ComponentCategory, ComponentCategory),
                CalculationType = ISNULL(@CalculationType, CalculationType), Value = ISNULL(@Value, Value), SortOrder = ISNULL(@SortOrder, SortOrder),
                IsEnabled = ISNULL(@IsEnabled, IsEnabled), UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE SalaryStructureComponentId = @SalaryStructureComponentId;
            SELECT 1 AS Success, 'Component updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[SalaryStructureComponent] SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy WHERE SalaryStructureComponentId = @SalaryStructureComponentId;
            SELECT 1 AS Success, 'Component deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetSalaryStructureComponentsByTemplateId]
    @SalaryStructureTemplateId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[SalaryStructureComponent] WHERE SalaryStructureTemplateId = @SalaryStructureTemplateId AND IsDeleted = 0 ORDER BY SortOrder;
END
GO
