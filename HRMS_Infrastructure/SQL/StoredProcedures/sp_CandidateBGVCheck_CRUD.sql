-- CRUD for CandidateBGVCheck, called from CandidateBGVRepository via Dapper.

CREATE OR ALTER PROCEDURE [dbo].[sp_CandidateBGVCheck_CRUD]
    @Operation VARCHAR(10),
    @CandidateBGVCheckId INT = NULL,
    @CandidateApplicationId INT = NULL,
    @CheckType VARCHAR(30) = NULL,
    @VendorName VARCHAR(200) = NULL,
    @VendorContact VARCHAR(100) = NULL,
    @Status VARCHAR(20) = NULL,
    @InitiatedDate DATETIME = NULL,
    @CompletedDate DATETIME = NULL,
    @ProofDocumentUrl VARCHAR(500) = NULL,
    @Remarks VARCHAR(1000) = NULL,
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
            INSERT INTO [dbo].[CandidateBGVCheck]
            (CandidateApplicationId, CheckType, VendorName, VendorContact, Status, InitiatedDate, CompletedDate, ProofDocumentUrl, Remarks, VerifiedBy, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@CandidateApplicationId, @CheckType, @VendorName, @VendorContact, ISNULL(@Status, 'Pending'), @InitiatedDate, @CompletedDate, @ProofDocumentUrl, @Remarks, @VerifiedBy, @IsEnabled, GETDATE(), @CreatedBy);

            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'BGV check saved successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[CandidateBGVCheck]
            SET VendorName = @VendorName, VendorContact = @VendorContact,
                Status = ISNULL(@Status, Status),
                InitiatedDate = @InitiatedDate,
                CompletedDate = CASE WHEN @Status = 'Completed' THEN ISNULL(@CompletedDate, GETDATE()) ELSE @CompletedDate END,
                ProofDocumentUrl = ISNULL(@ProofDocumentUrl, ProofDocumentUrl),
                Remarks = @Remarks,
                VerifiedBy = @VerifiedBy,
                IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE CandidateBGVCheckId = @CandidateBGVCheckId;

            SELECT 1 AS Success, 'BGV check updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[CandidateBGVCheck] SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy WHERE CandidateBGVCheckId = @CandidateBGVCheckId;
            SELECT 1 AS Success, 'BGV check deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetCandidateBGVChecksByApplicationId]
    @CandidateApplicationId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[CandidateBGVCheck] WHERE CandidateApplicationId = @CandidateApplicationId AND IsDeleted = 0 ORDER BY CandidateBGVCheckId;
END
GO
