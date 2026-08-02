-- CandidatePipelineStage (extensible lookup) CRUD + CandidatePipelineHistory (immutable
-- log, insert-only) + the single transactional stage-move proc used by the Kanban board.

CREATE OR ALTER PROCEDURE [dbo].[sp_CandidatePipelineStage_CRUD]
    @Operation VARCHAR(10),
    @CandidatePipelineStageId INT = NULL,
    @StageName VARCHAR(50) = NULL,
    @StageCategory VARCHAR(20) = NULL,
    @SortOrder INT = NULL,
    @IsTerminal BIT = 0,
    @CompanyId INT = NULL,
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
            INSERT INTO [dbo].[CandidatePipelineStage] (StageName, StageCategory, SortOrder, IsTerminal, CompanyId, IsEnabled, CreatedDate, CreatedBy)
            VALUES (@StageName, @StageCategory, @SortOrder, @IsTerminal, @CompanyId, @IsEnabled, GETDATE(), @CreatedBy);
            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Pipeline stage created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[CandidatePipelineStage]
            SET StageName = ISNULL(@StageName, StageName), StageCategory = ISNULL(@StageCategory, StageCategory),
                SortOrder = ISNULL(@SortOrder, SortOrder), IsTerminal = ISNULL(@IsTerminal, IsTerminal),
                IsEnabled = ISNULL(@IsEnabled, IsEnabled), UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE CandidatePipelineStageId = @CandidatePipelineStageId;
            SELECT 1 AS Success, 'Pipeline stage updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[CandidatePipelineStage] SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy WHERE CandidatePipelineStageId = @CandidatePipelineStageId;
            SELECT 1 AS Success, 'Pipeline stage deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetAllCandidatePipelineStages]
    @CompanyId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[CandidatePipelineStage]
    WHERE IsDeleted = 0 AND IsEnabled = 1 AND (CompanyId IS NULL OR @CompanyId IS NULL OR CompanyId = @CompanyId)
    ORDER BY SortOrder;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetCandidatePipelineHistoryByApplicationId]
    @CandidateApplicationId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT h.*, fs.StageName AS FromStageName, ts.StageName AS ToStageName
    FROM [dbo].[CandidatePipelineHistory] h
    LEFT JOIN [dbo].[CandidatePipelineStage] fs ON fs.CandidatePipelineStageId = h.FromStageId
    INNER JOIN [dbo].[CandidatePipelineStage] ts ON ts.CandidatePipelineStageId = h.ToStageId
    WHERE h.CandidateApplicationId = @CandidateApplicationId AND h.IsDeleted = 0
    ORDER BY h.TransitionDate;
END
GO

-- Atomically moves an application to a new stage and appends the audit-trail row —
-- mirrors ManPowerApproval calling a dedicated proc rather than generic CRUD.
CREATE OR ALTER PROCEDURE [dbo].[sp_MoveCandidatePipelineStage]
    @CandidateApplicationId INT,
    @ToStageId INT,
    @Remarks VARCHAR(1000) = NULL,
    @ActionBy INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @FromStageId INT = (SELECT CurrentPipelineStageId FROM [dbo].[CandidateApplication] WHERE CandidateApplicationId = @CandidateApplicationId);

        UPDATE [dbo].[CandidateApplication]
        SET CurrentPipelineStageId = @ToStageId, UpdatedDate = GETDATE(), UpdatedBy = @ActionBy
        WHERE CandidateApplicationId = @CandidateApplicationId;

        INSERT INTO [dbo].[CandidatePipelineHistory] (CandidateApplicationId, FromStageId, ToStageId, TransitionDate, Remarks, ActionBy, CreatedDate, CreatedBy)
        VALUES (@CandidateApplicationId, @FromStageId, @ToStageId, GETDATE(), @Remarks, @ActionBy, GETDATE(), @ActionBy);

        COMMIT TRANSACTION;
        SELECT 1 AS Success, 'Candidate moved to new stage successfully!' AS ResponseMessage;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO
