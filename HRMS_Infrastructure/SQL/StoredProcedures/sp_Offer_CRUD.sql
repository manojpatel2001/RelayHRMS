-- CRUD for Offer, called from OfferRepository via Dapper.

CREATE OR ALTER PROCEDURE [dbo].[sp_Offer_CRUD]
    @Operation VARCHAR(20),
    @OfferId INT = NULL,
    @CandidateApplicationId INT = NULL,
    @JobPositionId INT = NULL,
    @OfferSalaryFitmentId INT = NULL,
    @OfferedCTC DECIMAL(18,2) = NULL,
    @DesignationId INT = NULL,
    @DepartmentId INT = NULL,
    @BranchId INT = NULL,
    @GradeId INT = NULL,
    @JoiningDate DATE = NULL,
    @OfferExpiryDate DATE = NULL,
    @OfferStatus VARCHAR(20) = NULL,
    @OfferLetterUrl VARCHAR(500) = NULL,
    @AcceptedByCandidateName VARCHAR(200) = NULL,
    @DeclineReason VARCHAR(500) = NULL,
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
            INSERT INTO [dbo].[Offer]
            (CandidateApplicationId, JobPositionId, OfferSalaryFitmentId, OfferedCTC, DesignationId, DepartmentId, BranchId, GradeId,
             JoiningDate, OfferExpiryDate, OfferStatus, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@CandidateApplicationId, @JobPositionId, @OfferSalaryFitmentId, @OfferedCTC, @DesignationId, @DepartmentId, @BranchId, @GradeId,
             @JoiningDate, @OfferExpiryDate, ISNULL(@OfferStatus, 'Draft'), @IsEnabled, GETDATE(), @CreatedBy);

            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Offer created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[Offer]
            SET OfferSalaryFitmentId = @OfferSalaryFitmentId, OfferedCTC = @OfferedCTC, DesignationId = @DesignationId,
                DepartmentId = @DepartmentId, BranchId = @BranchId, GradeId = @GradeId, JoiningDate = @JoiningDate,
                OfferExpiryDate = @OfferExpiryDate, OfferStatus = ISNULL(@OfferStatus, OfferStatus),
                IsEnabled = ISNULL(@IsEnabled, IsEnabled), UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE OfferId = @OfferId;

            SELECT 1 AS Success, 'Offer updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[Offer] SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy WHERE OfferId = @OfferId;
            SELECT 1 AS Success, 'Offer deleted successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'SETSTATUS'
        BEGIN
            UPDATE [dbo].[Offer]
            SET OfferStatus = @OfferStatus,
                ReleasedDate = CASE WHEN @OfferStatus = 'Released' THEN GETDATE() ELSE ReleasedDate END,
                UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE OfferId = @OfferId;
            SELECT 1 AS Success, 'Offer status updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'SETLETTERURL'
        BEGIN
            UPDATE [dbo].[Offer] SET OfferLetterUrl = @OfferLetterUrl, UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy WHERE OfferId = @OfferId;
            SELECT 1 AS Success, 'Offer letter URL saved successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'RECORDRESPONSE'
        BEGIN
            UPDATE [dbo].[Offer]
            SET OfferStatus = @OfferStatus,
                AcceptedDate = CASE WHEN @OfferStatus = 'Accepted' THEN GETDATE() ELSE AcceptedDate END,
                AcceptedByCandidateName = @AcceptedByCandidateName,
                DeclineReason = @DeclineReason,
                UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE OfferId = @OfferId;
            SELECT 1 AS Success, 'Candidate response recorded successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetAllOffers]
    @CompanyId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT o.*, c.FullName, c.Email, pos.PositionTitle
    FROM [dbo].[Offer] o
    INNER JOIN [dbo].[CandidateApplication] ca ON ca.CandidateApplicationId = o.CandidateApplicationId
    INNER JOIN [dbo].[Candidate] c ON c.CandidateId = ca.CandidateId
    LEFT JOIN [dbo].[JobPositionMaster] pos ON pos.JobPositionId = o.JobPositionId
    WHERE o.IsDeleted = 0 AND (@CompanyId IS NULL OR pos.CompanyId = @CompanyId)
    ORDER BY o.CreatedDate DESC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetOfferById]
    @OfferId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT o.*, c.FullName, c.Email, c.Phone, pos.PositionTitle, pos.MinBudget, pos.MaxBudget,
           d.DesignationName, dep.DepartmentName, b.BranchName, g.GradeName
    FROM [dbo].[Offer] o
    INNER JOIN [dbo].[CandidateApplication] ca ON ca.CandidateApplicationId = o.CandidateApplicationId
    INNER JOIN [dbo].[Candidate] c ON c.CandidateId = ca.CandidateId
    LEFT JOIN [dbo].[JobPositionMaster] pos ON pos.JobPositionId = o.JobPositionId
    LEFT JOIN [dbo].[Designation] d ON d.DesignationId = o.DesignationId
    LEFT JOIN [dbo].[Department] dep ON dep.DepartmentId = o.DepartmentId
    LEFT JOIN [dbo].[Branch] b ON b.BranchId = o.BranchId
    LEFT JOIN [dbo].[Grade] g ON g.GradeId = o.GradeId
    WHERE o.OfferId = @OfferId AND o.IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetOfferByCandidateApplicationId]
    @CandidateApplicationId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 * FROM [dbo].[Offer] WHERE CandidateApplicationId = @CandidateApplicationId AND IsDeleted = 0 ORDER BY CreatedDate DESC;
END
GO
