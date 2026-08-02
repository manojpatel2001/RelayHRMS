-- CRUD for CandidateDocument, called from CandidateDocumentRepository via Dapper.

CREATE OR ALTER PROCEDURE [dbo].[sp_CandidateDocument_CRUD]
    @Operation VARCHAR(10),
    @CandidateDocumentId INT = NULL,
    @CandidateApplicationId INT = NULL,
    @CandidateDocumentTypeId INT = NULL,
    @DocumentUrl VARCHAR(500) = NULL,
    @Remarks VARCHAR(500) = NULL,
    @VerifiedStatus VARCHAR(20) = NULL,
    @VerifiedBy INT = NULL,
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
            INSERT INTO [dbo].[CandidateDocument]
            (CandidateApplicationId, CandidateDocumentTypeId, DocumentUrl, Remarks, VerifiedStatus, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@CandidateApplicationId, @CandidateDocumentTypeId, @DocumentUrl, @Remarks, ISNULL(@VerifiedStatus, 'Pending'), @IsEnabled, GETDATE(), @CreatedBy);

            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Document uploaded successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[CandidateDocument]
            SET DocumentUrl = ISNULL(@DocumentUrl, DocumentUrl),
                Remarks = @Remarks,
                VerifiedStatus = ISNULL(@VerifiedStatus, VerifiedStatus),
                VerifiedBy = CASE WHEN @VerifiedStatus IS NOT NULL THEN @VerifiedBy ELSE VerifiedBy END,
                VerifiedDate = CASE WHEN @VerifiedStatus IS NOT NULL AND @VerifiedStatus <> 'Pending' THEN GETDATE() ELSE VerifiedDate END,
                IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE CandidateDocumentId = @CandidateDocumentId;

            SELECT 1 AS Success, 'Document updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[CandidateDocument] SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy WHERE CandidateDocumentId = @CandidateDocumentId;
            SELECT 1 AS Success, 'Document deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetCandidateDocumentsByApplicationId]
    @CandidateApplicationId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT d.*, t.DocumentName, t.IsMandatory
    FROM [dbo].[CandidateDocument] d
    INNER JOIN [dbo].[CandidateDocumentType] t ON t.CandidateDocumentTypeId = d.CandidateDocumentTypeId
    WHERE d.CandidateApplicationId = @CandidateApplicationId AND d.IsDeleted = 0
    ORDER BY t.SortOrder;
END
GO
