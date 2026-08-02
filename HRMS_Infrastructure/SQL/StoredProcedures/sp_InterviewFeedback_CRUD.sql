-- CRUD for InterviewFeedback (one row per panelist per interview), called from
-- InterviewFeedbackRepository via Dapper.

CREATE OR ALTER PROCEDURE [dbo].[sp_InterviewFeedback_CRUD]
    @Operation VARCHAR(10),
    @InterviewFeedbackId INT = NULL,
    @InterviewId INT = NULL,
    @PanelistEmployeeId INT = NULL,
    @TechnicalRating INT = NULL,
    @CommunicationRating INT = NULL,
    @ProblemSolvingRating INT = NULL,
    @OverallRating INT = NULL,
    @Strengths VARCHAR(1000) = NULL,
    @Weaknesses VARCHAR(1000) = NULL,
    @Recommendation VARCHAR(20) = NULL,
    @Remarks VARCHAR(1000) = NULL,
    @IsSubmitted BIT = 0,
    @IsEnabled BIT = 1,
    @CreatedBy INT = NULL,
    @UpdatedBy INT = NULL,
    @DeletedBy INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @ExistingId INT = (SELECT TOP 1 InterviewFeedbackId FROM [dbo].[InterviewFeedback] WHERE InterviewId = @InterviewId AND PanelistEmployeeId = @PanelistEmployeeId AND IsDeleted = 0);

        IF @Operation IN ('INSERT', 'CREATE') AND @ExistingId IS NULL
        BEGIN
            INSERT INTO [dbo].[InterviewFeedback]
            (InterviewId, PanelistEmployeeId, TechnicalRating, CommunicationRating, ProblemSolvingRating, OverallRating,
             Strengths, Weaknesses, Recommendation, Remarks, IsSubmitted, SubmittedDate, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@InterviewId, @PanelistEmployeeId, @TechnicalRating, @CommunicationRating, @ProblemSolvingRating, @OverallRating,
             @Strengths, @Weaknesses, @Recommendation, @Remarks, @IsSubmitted, CASE WHEN @IsSubmitted = 1 THEN GETDATE() ELSE NULL END, @IsEnabled, GETDATE(), @CreatedBy);

            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Interview feedback saved successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation IN ('INSERT', 'CREATE', 'UPDATE') AND @ExistingId IS NOT NULL
        BEGIN
            UPDATE [dbo].[InterviewFeedback]
            SET TechnicalRating = @TechnicalRating, CommunicationRating = @CommunicationRating, ProblemSolvingRating = @ProblemSolvingRating,
                OverallRating = @OverallRating, Strengths = @Strengths, Weaknesses = @Weaknesses, Recommendation = @Recommendation,
                Remarks = @Remarks, IsSubmitted = @IsSubmitted,
                SubmittedDate = CASE WHEN @IsSubmitted = 1 THEN GETDATE() ELSE SubmittedDate END,
                UpdatedDate = GETDATE(), UpdatedBy = ISNULL(@UpdatedBy, @CreatedBy)
            WHERE InterviewFeedbackId = @ExistingId;

            SELECT @ExistingId AS NewId, 1 AS Success, 'Interview feedback updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[InterviewFeedback] SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy WHERE InterviewFeedbackId = @InterviewFeedbackId;
            SELECT 1 AS Success, 'Interview feedback deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetInterviewFeedbackByInterviewId]
    @InterviewId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[InterviewFeedback] WHERE InterviewId = @InterviewId AND IsDeleted = 0;
END
GO

-- Consolidated feedback across ALL interviews for one candidate application (used by the
-- Selection Decision screen to show a panel-wide summary before the final call).
CREATE OR ALTER PROCEDURE [dbo].[GetConsolidatedFeedbackByCandidateApplicationId]
    @CandidateApplicationId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT f.*, i.RoundName, i.ScheduledDate
    FROM [dbo].[InterviewFeedback] f
    INNER JOIN [dbo].[Interview] i ON i.InterviewId = f.InterviewId
    WHERE i.CandidateApplicationId = @CandidateApplicationId AND f.IsDeleted = 0 AND i.IsDeleted = 0
    ORDER BY i.ScheduledDate;
END
GO
