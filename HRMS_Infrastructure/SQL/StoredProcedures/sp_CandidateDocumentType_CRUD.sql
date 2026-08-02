-- CRUD for CandidateDocumentType, called from CandidateDocumentRepository via Dapper.

CREATE OR ALTER PROCEDURE [dbo].[sp_CandidateDocumentType_CRUD]
    @Operation VARCHAR(10),
    @CandidateDocumentTypeId INT = NULL,
    @DocumentName VARCHAR(150) = NULL,
    @IsMandatory BIT = 0,
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
            INSERT INTO [dbo].[CandidateDocumentType] (DocumentName, IsMandatory, SortOrder, IsEnabled, CreatedDate, CreatedBy)
            VALUES (@DocumentName, @IsMandatory, @SortOrder, @IsEnabled, GETDATE(), @CreatedBy);
            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Document type created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[CandidateDocumentType]
            SET DocumentName = ISNULL(@DocumentName, DocumentName), IsMandatory = ISNULL(@IsMandatory, IsMandatory),
                SortOrder = ISNULL(@SortOrder, SortOrder), IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE CandidateDocumentTypeId = @CandidateDocumentTypeId;
            SELECT 1 AS Success, 'Document type updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[CandidateDocumentType] SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy WHERE CandidateDocumentTypeId = @CandidateDocumentTypeId;
            SELECT 1 AS Success, 'Document type removed successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetAllCandidateDocumentTypes]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[CandidateDocumentType] WHERE IsDeleted = 0 AND IsEnabled = 1 ORDER BY SortOrder;
END
GO
