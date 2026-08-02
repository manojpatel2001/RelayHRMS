-- Phase 1 Recruitment overhaul: additive extension of the existing ManpowerRequisition
-- table so it can keep serving as the Manpower Request / Job Requisition Approval step
-- of the new Recruitment module (no duplicate headcount-request screen is being built).
-- NOTE: ReplacementOrNew is intentionally NOT added — the existing RequirementType
-- column ('New Position'/'Replacement', see ManpowerRequisition.cshtml #ddlRequirementType)
-- already covers that distinction.
-- The live ManageManpowerRequisition proc is not in source control, so rather than
-- editing it blind, the 4 new fields are written via a small separate proc
-- (sp_ManpowerRequisitionPhase1Fields_Update) called right after the existing
-- create/update call — additive, zero risk to the existing proc.

IF COL_LENGTH('dbo.ManpowerRequisitions', 'Priority') IS NULL
    ALTER TABLE [dbo].[ManpowerRequisitions] ADD Priority VARCHAR(10) NULL; -- 'High' | 'Medium' | 'Low'
GO

IF COL_LENGTH('dbo.ManpowerRequisitions', 'ReplacedEmployeeId') IS NULL
    ALTER TABLE [dbo].[ManpowerRequisitions] ADD ReplacedEmployeeId INT NULL; -- used only when RequirementType = 'Replacement'
GO

IF COL_LENGTH('dbo.ManpowerRequisitions', 'Remarks') IS NULL
    ALTER TABLE [dbo].[ManpowerRequisitions] ADD Remarks VARCHAR(1000) NULL; -- distinct from JobResponsibility
GO

IF COL_LENGTH('dbo.ManpowerRequisitions', 'BudgetAmount') IS NULL
    ALTER TABLE [dbo].[ManpowerRequisitions] ADD BudgetAmount DECIMAL(18, 2) NULL; -- budget ceiling vs TakeHomeSalary
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_ManpowerRequisitionPhase1Fields_Update]
    @ManpowerRequisitionId INT,
    @Priority VARCHAR(10) = NULL,
    @ReplacedEmployeeId INT = NULL,
    @Remarks VARCHAR(1000) = NULL,
    @BudgetAmount DECIMAL(18, 2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE [dbo].[ManpowerRequisitions]
        SET
            Priority = @Priority,
            ReplacedEmployeeId = @ReplacedEmployeeId,
            Remarks = @Remarks,
            BudgetAmount = @BudgetAmount
        WHERE ManpowerRequisitionId = @ManpowerRequisitionId;

        SELECT 1 AS Success, 'Manpower requisition Phase 1 fields updated successfully!' AS ResponseMessage;
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, 'Something went wrong!' AS ResponseMessage;
    END CATCH
END
GO
