-- CRUD for CandidateApplication (one candidate's pursuit of one Job Position),
-- called from CandidateApplicationRepository via Dapper.

CREATE OR ALTER PROCEDURE [dbo].[sp_CandidateApplication_CRUD]
    @Operation VARCHAR(10),
    @CandidateApplicationId INT = NULL,
    @CandidateId INT = NULL,
    @JobPositionId INT = NULL,
    @JobPostingId INT = NULL,
    @ApplicationSource VARCHAR(50) = NULL,
    @ResumeUrl VARCHAR(500) = NULL,
    @CoverLetter VARCHAR(2000) = NULL,
    @CurrentPipelineStageId INT = NULL,
    @IsActive BIT = 1,
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
            INSERT INTO [dbo].[CandidateApplication]
            (CandidateId, JobPositionId, JobPostingId, ApplicationDate, ApplicationSource, ResumeUrl, CoverLetter, CurrentPipelineStageId, IsActive, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@CandidateId, @JobPositionId, @JobPostingId, GETDATE(), @ApplicationSource, @ResumeUrl, @CoverLetter, @CurrentPipelineStageId, @IsActive, @IsEnabled, GETDATE(), @CreatedBy);

            DECLARE @NewId INT = SCOPE_IDENTITY();

            IF @JobPostingId IS NOT NULL
                UPDATE [dbo].[JobPosting] SET ApplicationCount = ApplicationCount + 1 WHERE JobPostingId = @JobPostingId;

            SELECT @NewId AS NewId, 1 AS Success, 'Candidate application created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[CandidateApplication]
            SET
                ApplicationSource = ISNULL(@ApplicationSource, ApplicationSource),
                ResumeUrl = ISNULL(@ResumeUrl, ResumeUrl),
                CoverLetter = @CoverLetter,
                IsActive = ISNULL(@IsActive, IsActive),
                IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(),
                UpdatedBy = @UpdatedBy
            WHERE CandidateApplicationId = @CandidateApplicationId;

            SELECT 1 AS Success, 'Candidate application updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[CandidateApplication]
            SET IsDeleted = 1, IsEnabled = 0, IsActive = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy
            WHERE CandidateApplicationId = @CandidateApplicationId;

            SELECT 1 AS Success, 'Candidate application deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetAllCandidateApplications]
    @JobPositionId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ca.*, c.FullName, c.Email, c.Phone, c.CurrentEmployer, c.TotalExperienceYears, c.ExpectedCTC,
           pos.PositionTitle, st.StageName, st.StageCategory,
           sr.OverallScore, sr.Recommendation
    FROM [dbo].[CandidateApplication] ca
    INNER JOIN [dbo].[Candidate] c ON c.CandidateId = ca.CandidateId
    LEFT JOIN [dbo].[JobPositionMaster] pos ON pos.JobPositionId = ca.JobPositionId
    LEFT JOIN [dbo].[CandidatePipelineStage] st ON st.CandidatePipelineStageId = ca.CurrentPipelineStageId
    LEFT JOIN [dbo].[CandidateResumeScreeningResult] sr ON sr.CandidateApplicationId = ca.CandidateApplicationId AND sr.IsDeleted = 0
    WHERE ca.IsDeleted = 0 AND (@JobPositionId IS NULL OR ca.JobPositionId = @JobPositionId)
    ORDER BY ca.ApplicationDate DESC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetCandidateApplicationById]
    @CandidateApplicationId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ca.*, c.FullName, c.Email, c.Phone, pos.PositionTitle, st.StageName
    FROM [dbo].[CandidateApplication] ca
    INNER JOIN [dbo].[Candidate] c ON c.CandidateId = ca.CandidateId
    LEFT JOIN [dbo].[JobPositionMaster] pos ON pos.JobPositionId = ca.JobPositionId
    LEFT JOIN [dbo].[CandidatePipelineStage] st ON st.CandidatePipelineStageId = ca.CurrentPipelineStageId
    WHERE ca.CandidateApplicationId = @CandidateApplicationId AND ca.IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetCandidateApplicationsByCandidateId]
    @CandidateId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ca.*, pos.PositionTitle, st.StageName
    FROM [dbo].[CandidateApplication] ca
    LEFT JOIN [dbo].[JobPositionMaster] pos ON pos.JobPositionId = ca.JobPositionId
    LEFT JOIN [dbo].[CandidatePipelineStage] st ON st.CandidatePipelineStageId = ca.CurrentPipelineStageId
    WHERE ca.CandidateId = @CandidateId AND ca.IsDeleted = 0
    ORDER BY ca.ApplicationDate DESC;
END
GO

-- Kanban board data: applications grouped by pipeline stage for one Job Position.
CREATE OR ALTER PROCEDURE [dbo].[GetKanbanBoardData]
    @JobPositionId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ca.CandidateApplicationId, ca.CandidateId, ca.JobPositionId, ca.CurrentPipelineStageId,
           c.FullName, c.Email, c.Phone, c.CurrentEmployer, c.TotalExperienceYears,
           sr.OverallScore, sr.Recommendation,
           st.StageName, st.SortOrder
    FROM [dbo].[CandidateApplication] ca
    INNER JOIN [dbo].[Candidate] c ON c.CandidateId = ca.CandidateId
    INNER JOIN [dbo].[CandidatePipelineStage] st ON st.CandidatePipelineStageId = ca.CurrentPipelineStageId
    LEFT JOIN [dbo].[CandidateResumeScreeningResult] sr ON sr.CandidateApplicationId = ca.CandidateApplicationId AND sr.IsDeleted = 0
    WHERE ca.IsDeleted = 0 AND ca.IsActive = 1 AND ca.JobPositionId = @JobPositionId
    ORDER BY st.SortOrder, ca.ApplicationDate;
END
GO
