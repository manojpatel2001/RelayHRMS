-- Create-or-update for CandidateResumeScreeningResult — one current row per application,
-- written by HRMS_API's ResumeScreeningService (C# rule-based scoring, no logic in SQL).

CREATE OR ALTER PROCEDURE [dbo].[sp_CandidateResumeScreeningResult_CRUD]
    @Operation VARCHAR(10),
    @CandidateResumeScreeningResultId INT = NULL,
    @CandidateApplicationId INT = NULL,
    @JobPositionId INT = NULL,
    @SkillMatchScore DECIMAL(5,2) = NULL,
    @ExperienceMatchScore DECIMAL(5,2) = NULL,
    @EducationMatchScore DECIMAL(5,2) = NULL,
    @OverallScore DECIMAL(5,2) = NULL,
    @Recommendation VARCHAR(20) = NULL,
    @MatchedSkills VARCHAR(1000) = NULL,
    @MissingSkills VARCHAR(1000) = NULL,
    @IsDuplicate BIT = 0,
    @DuplicateOfCandidateId INT = NULL,
    @DuplicateMatchReason VARCHAR(100) = NULL,
    @ScreeningEngineVersion VARCHAR(20) = NULL,
    @CreatedBy INT = NULL,
    @UpdatedBy INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @ExistingId INT = (SELECT TOP 1 CandidateResumeScreeningResultId FROM [dbo].[CandidateResumeScreeningResult] WHERE CandidateApplicationId = @CandidateApplicationId AND IsDeleted = 0);

        IF @Operation IN ('INSERT', 'CREATE') AND @ExistingId IS NULL
        BEGIN
            INSERT INTO [dbo].[CandidateResumeScreeningResult]
            (CandidateApplicationId, JobPositionId, SkillMatchScore, ExperienceMatchScore, EducationMatchScore, OverallScore,
             Recommendation, MatchedSkills, MissingSkills, IsDuplicate, DuplicateOfCandidateId, DuplicateMatchReason,
             ScreenedDate, ScreeningEngineVersion, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@CandidateApplicationId, @JobPositionId, @SkillMatchScore, @ExperienceMatchScore, @EducationMatchScore, @OverallScore,
             @Recommendation, @MatchedSkills, @MissingSkills, @IsDuplicate, @DuplicateOfCandidateId, @DuplicateMatchReason,
             GETDATE(), @ScreeningEngineVersion, 1, GETDATE(), @CreatedBy);

            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Screening result saved successfully!' AS ResponseMessage;
        END
        ELSE
        BEGIN
            UPDATE [dbo].[CandidateResumeScreeningResult]
            SET SkillMatchScore = @SkillMatchScore, ExperienceMatchScore = @ExperienceMatchScore, EducationMatchScore = @EducationMatchScore,
                OverallScore = @OverallScore, Recommendation = @Recommendation, MatchedSkills = @MatchedSkills, MissingSkills = @MissingSkills,
                IsDuplicate = @IsDuplicate, DuplicateOfCandidateId = @DuplicateOfCandidateId, DuplicateMatchReason = @DuplicateMatchReason,
                ScreenedDate = GETDATE(), ScreeningEngineVersion = @ScreeningEngineVersion, UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE CandidateApplicationId = @CandidateApplicationId;

            SELECT 1 AS Success, 'Screening result updated successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetCandidateResumeScreeningResultByApplicationId]
    @CandidateApplicationId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[CandidateResumeScreeningResult] WHERE CandidateApplicationId = @CandidateApplicationId AND IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetScreeningResultsByJobPositionId]
    @JobPositionId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT sr.*, ca.CandidateId, c.FullName
    FROM [dbo].[CandidateResumeScreeningResult] sr
    INNER JOIN [dbo].[CandidateApplication] ca ON ca.CandidateApplicationId = sr.CandidateApplicationId
    INNER JOIN [dbo].[Candidate] c ON c.CandidateId = ca.CandidateId
    WHERE sr.JobPositionId = @JobPositionId AND sr.IsDeleted = 0
    ORDER BY sr.OverallScore DESC;
END
GO
