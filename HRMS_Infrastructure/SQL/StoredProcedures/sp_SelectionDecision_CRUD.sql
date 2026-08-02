-- CRUD for SelectionDecision, called from SelectionDecisionRepository via Dapper.

CREATE OR ALTER PROCEDURE [dbo].[sp_SelectionDecision_CRUD]
    @Operation VARCHAR(10),
    @SelectionDecisionId INT = NULL,
    @CandidateApplicationId INT = NULL,
    @DecisionStatus VARCHAR(20) = NULL,
    @DecidedBy INT = NULL,
    @Remarks VARCHAR(1000) = NULL,
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
            INSERT INTO [dbo].[SelectionDecision] (CandidateApplicationId, DecisionStatus, DecidedBy, DecisionDate, Remarks, IsEnabled, CreatedDate, CreatedBy)
            VALUES (@CandidateApplicationId, @DecisionStatus, @DecidedBy, GETDATE(), @Remarks, @IsEnabled, GETDATE(), @CreatedBy);
            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Selection decision recorded successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[SelectionDecision]
            SET DecisionStatus = ISNULL(@DecisionStatus, DecisionStatus), DecidedBy = ISNULL(@DecidedBy, DecidedBy),
                Remarks = @Remarks, IsEnabled = ISNULL(@IsEnabled, IsEnabled), UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE SelectionDecisionId = @SelectionDecisionId;
            SELECT 1 AS Success, 'Selection decision updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[SelectionDecision] SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy WHERE SelectionDecisionId = @SelectionDecisionId;
            SELECT 1 AS Success, 'Selection decision deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetSelectionDecisionByCandidateApplicationId]
    @CandidateApplicationId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 * FROM [dbo].[SelectionDecision]
    WHERE CandidateApplicationId = @CandidateApplicationId AND IsDeleted = 0
    ORDER BY DecisionDate DESC;
END
GO
