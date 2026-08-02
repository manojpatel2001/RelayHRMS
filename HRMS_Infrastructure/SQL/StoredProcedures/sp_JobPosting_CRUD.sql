-- CRUD for JobPosting, called from JobPostingRepository via Dapper.

CREATE OR ALTER PROCEDURE [dbo].[sp_JobPosting_CRUD]
    @Operation VARCHAR(10),
    @JobPostingId INT = NULL,
    @JobPositionId INT = NULL,
    @PostingTitle VARCHAR(200) = NULL,
    @Slug VARCHAR(250) = NULL,
    @IsInternal BIT = 1,
    @IsExternal BIT = 0,
    @JobDescriptionHtml VARCHAR(MAX) = NULL,
    @ResponsibilitiesHtml VARCHAR(MAX) = NULL,
    @BenefitsHtml VARCHAR(MAX) = NULL,
    @Location VARCHAR(200) = NULL,
    @WorkMode VARCHAR(20) = NULL,
    @PublishDate DATETIME = NULL,
    @ExpiryDate DATETIME = NULL,
    @Status VARCHAR(20) = NULL,
    @CompanyId INT = NULL,
    @LinkedInEnabled BIT = 0, @LinkedInStatus VARCHAR(20) = NULL, @LinkedInPostedDate DATETIME = NULL, @LinkedInPostingUrl VARCHAR(500) = NULL,
    @NaukriEnabled BIT = 0, @NaukriStatus VARCHAR(20) = NULL, @NaukriPostedDate DATETIME = NULL, @NaukriPostingUrl VARCHAR(500) = NULL,
    @IndeedEnabled BIT = 0, @IndeedStatus VARCHAR(20) = NULL, @IndeedPostedDate DATETIME = NULL, @IndeedPostingUrl VARCHAR(500) = NULL,
    @ReferralEnabled BIT = 0, @ReferralStatus VARCHAR(20) = NULL, @ReferralPostedDate DATETIME = NULL, @ReferralPostingUrl VARCHAR(500) = NULL,
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
            INSERT INTO [dbo].[JobPosting]
            (JobPositionId, PostingTitle, Slug, IsInternal, IsExternal, JobDescriptionHtml, ResponsibilitiesHtml, BenefitsHtml,
             Location, WorkMode, PublishDate, ExpiryDate, Status, CompanyId,
             LinkedInEnabled, NaukriEnabled, IndeedEnabled, ReferralEnabled, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@JobPositionId, @PostingTitle, @Slug, @IsInternal, @IsExternal, @JobDescriptionHtml, @ResponsibilitiesHtml, @BenefitsHtml,
             @Location, @WorkMode, @PublishDate, @ExpiryDate, ISNULL(@Status, 'Draft'), @CompanyId,
             @LinkedInEnabled, @NaukriEnabled, @IndeedEnabled, @ReferralEnabled, @IsEnabled, GETDATE(), @CreatedBy);

            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Job posting created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[JobPosting]
            SET
                PostingTitle = ISNULL(@PostingTitle, PostingTitle),
                Slug = ISNULL(@Slug, Slug),
                IsInternal = @IsInternal,
                IsExternal = @IsExternal,
                JobDescriptionHtml = @JobDescriptionHtml,
                ResponsibilitiesHtml = @ResponsibilitiesHtml,
                BenefitsHtml = @BenefitsHtml,
                Location = @Location,
                WorkMode = @WorkMode,
                PublishDate = @PublishDate,
                ExpiryDate = @ExpiryDate,
                Status = ISNULL(@Status, Status),
                LinkedInEnabled = @LinkedInEnabled, LinkedInStatus = ISNULL(@LinkedInStatus, LinkedInStatus), LinkedInPostedDate = ISNULL(@LinkedInPostedDate, LinkedInPostedDate), LinkedInPostingUrl = ISNULL(@LinkedInPostingUrl, LinkedInPostingUrl),
                NaukriEnabled = @NaukriEnabled, NaukriStatus = ISNULL(@NaukriStatus, NaukriStatus), NaukriPostedDate = ISNULL(@NaukriPostedDate, NaukriPostedDate), NaukriPostingUrl = ISNULL(@NaukriPostingUrl, NaukriPostingUrl),
                IndeedEnabled = @IndeedEnabled, IndeedStatus = ISNULL(@IndeedStatus, IndeedStatus), IndeedPostedDate = ISNULL(@IndeedPostedDate, IndeedPostedDate), IndeedPostingUrl = ISNULL(@IndeedPostingUrl, IndeedPostingUrl),
                ReferralEnabled = @ReferralEnabled, ReferralStatus = ISNULL(@ReferralStatus, ReferralStatus), ReferralPostedDate = ISNULL(@ReferralPostedDate, ReferralPostedDate), ReferralPostingUrl = ISNULL(@ReferralPostingUrl, ReferralPostingUrl),
                IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(),
                UpdatedBy = @UpdatedBy
            WHERE JobPostingId = @JobPostingId;

            SELECT 1 AS Success, 'Job posting updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[JobPosting]
            SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy
            WHERE JobPostingId = @JobPostingId;

            SELECT 1 AS Success, 'Job posting deleted successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'PUBLISH'
        BEGIN
            UPDATE [dbo].[JobPosting]
            SET Status = 'Published', PublishDate = ISNULL(PublishDate, GETDATE()), UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE JobPostingId = @JobPostingId;

            SELECT 1 AS Success, 'Job posting published successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetAllJobPosting]
    @CompanyId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT jp.*, pos.PositionTitle
    FROM [dbo].[JobPosting] jp
    LEFT JOIN [dbo].[JobPositionMaster] pos ON pos.JobPositionId = jp.JobPositionId
    WHERE jp.IsDeleted = 0 AND (@CompanyId IS NULL OR jp.CompanyId = @CompanyId)
    ORDER BY jp.CreatedDate DESC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetJobPostingById]
    @JobPostingId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT jp.*, pos.PositionTitle
    FROM [dbo].[JobPosting] jp
    LEFT JOIN [dbo].[JobPositionMaster] pos ON pos.JobPositionId = jp.JobPositionId
    WHERE jp.JobPostingId = @JobPostingId AND jp.IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetJobPostingBySlug]
    @Slug VARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [dbo].[JobPosting] SET ViewCount = ViewCount + 1 WHERE Slug = @Slug AND IsDeleted = 0;
    SELECT * FROM [dbo].[JobPosting] WHERE Slug = @Slug AND IsDeleted = 0;
END
GO
