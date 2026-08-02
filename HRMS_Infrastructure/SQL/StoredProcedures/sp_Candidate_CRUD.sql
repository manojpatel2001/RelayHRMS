-- CRUD for Candidate (stable person identity), called from CandidateRepository via Dapper.

CREATE OR ALTER PROCEDURE [dbo].[sp_Candidate_CRUD]
    @Operation VARCHAR(10),
    @CandidateId INT = NULL,
    @FirstName VARCHAR(100) = NULL,
    @MiddleName VARCHAR(100) = NULL,
    @LastName VARCHAR(100) = NULL,
    @Email VARCHAR(150) = NULL,
    @Phone VARCHAR(20) = NULL,
    @AlternatePhone VARCHAR(20) = NULL,
    @DateOfBirth DATE = NULL,
    @Gender VARCHAR(10) = NULL,
    @CurrentAddress VARCHAR(500) = NULL,
    @CurrentCity VARCHAR(100) = NULL,
    @PreferredLocation VARCHAR(200) = NULL,
    @CurrentEmployer VARCHAR(200) = NULL,
    @CurrentDesignation VARCHAR(150) = NULL,
    @TotalExperienceYears DECIMAL(4,1) = NULL,
    @RelevantExperienceYears DECIMAL(4,1) = NULL,
    @CurrentCTC DECIMAL(18,2) = NULL,
    @ExpectedCTC DECIMAL(18,2) = NULL,
    @NoticePeriodDays INT = NULL,
    @LinkedInUrl VARCHAR(300) = NULL,
    @GitHubUrl VARCHAR(300) = NULL,
    @PortfolioUrl VARCHAR(300) = NULL,
    @ResumeUrl VARCHAR(500) = NULL,
    @ResumeFileHash VARCHAR(128) = NULL,
    @Source VARCHAR(50) = NULL,
    @ReferredByEmployeeId INT = NULL,
    @Summary VARCHAR(2000) = NULL,
    @IsBlacklisted BIT = 0,
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
            INSERT INTO [dbo].[Candidate]
            (FirstName, MiddleName, LastName, FullName, Email, Phone, AlternatePhone, DateOfBirth, Gender,
             CurrentAddress, CurrentCity, PreferredLocation, CurrentEmployer, CurrentDesignation,
             TotalExperienceYears, RelevantExperienceYears, CurrentCTC, ExpectedCTC, NoticePeriodDays,
             LinkedInUrl, GitHubUrl, PortfolioUrl, ResumeUrl, ResumeFileHash, Source, ReferredByEmployeeId,
             Summary, IsBlacklisted, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@FirstName, @MiddleName, @LastName, LTRIM(RTRIM(CONCAT(@FirstName, ' ', ISNULL(@MiddleName, ''), ' ', ISNULL(@LastName, '')))),
             @Email, @Phone, @AlternatePhone, @DateOfBirth, @Gender,
             @CurrentAddress, @CurrentCity, @PreferredLocation, @CurrentEmployer, @CurrentDesignation,
             @TotalExperienceYears, @RelevantExperienceYears, @CurrentCTC, @ExpectedCTC, @NoticePeriodDays,
             @LinkedInUrl, @GitHubUrl, @PortfolioUrl, @ResumeUrl, @ResumeFileHash, @Source, @ReferredByEmployeeId,
             @Summary, @IsBlacklisted, @IsEnabled, GETDATE(), @CreatedBy);

            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Candidate created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[Candidate]
            SET
                FirstName = ISNULL(@FirstName, FirstName),
                MiddleName = @MiddleName,
                LastName = @LastName,
                FullName = LTRIM(RTRIM(CONCAT(ISNULL(@FirstName, FirstName), ' ', ISNULL(@MiddleName, ''), ' ', ISNULL(@LastName, '')))),
                Email = ISNULL(@Email, Email),
                Phone = ISNULL(@Phone, Phone),
                AlternatePhone = @AlternatePhone,
                DateOfBirth = @DateOfBirth,
                Gender = @Gender,
                CurrentAddress = @CurrentAddress,
                CurrentCity = @CurrentCity,
                PreferredLocation = @PreferredLocation,
                CurrentEmployer = @CurrentEmployer,
                CurrentDesignation = @CurrentDesignation,
                TotalExperienceYears = @TotalExperienceYears,
                RelevantExperienceYears = @RelevantExperienceYears,
                CurrentCTC = @CurrentCTC,
                ExpectedCTC = @ExpectedCTC,
                NoticePeriodDays = @NoticePeriodDays,
                LinkedInUrl = @LinkedInUrl,
                GitHubUrl = @GitHubUrl,
                PortfolioUrl = @PortfolioUrl,
                ResumeUrl = ISNULL(@ResumeUrl, ResumeUrl),
                ResumeFileHash = ISNULL(@ResumeFileHash, ResumeFileHash),
                Source = @Source,
                ReferredByEmployeeId = @ReferredByEmployeeId,
                Summary = @Summary,
                IsBlacklisted = ISNULL(@IsBlacklisted, IsBlacklisted),
                IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(),
                UpdatedBy = @UpdatedBy
            WHERE CandidateId = @CandidateId;

            SELECT 1 AS Success, 'Candidate updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[Candidate]
            SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy
            WHERE CandidateId = @CandidateId;

            SELECT 1 AS Success, 'Candidate deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetAllCandidates]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[Candidate] WHERE IsDeleted = 0 ORDER BY CreatedDate DESC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetCandidateById]
    @CandidateId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[Candidate] WHERE CandidateId = @CandidateId AND IsDeleted = 0;
END
GO

-- Soft duplicate-detection lookup — a warning to the recruiter, not a blocking constraint,
-- since legitimate re-applications must remain possible.
CREATE OR ALTER PROCEDURE [dbo].[CheckCandidateDuplicate]
    @Email VARCHAR(150) = NULL,
    @Phone VARCHAR(20) = NULL,
    @ResumeFileHash VARCHAR(128) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CandidateId, FullName, Email, Phone, ResumeFileHash,
        CASE
            WHEN @ResumeFileHash IS NOT NULL AND ResumeFileHash = @ResumeFileHash THEN 'ResumeHash'
            WHEN @Email IS NOT NULL AND Email = @Email THEN 'Email'
            WHEN @Phone IS NOT NULL AND Phone = @Phone THEN 'Phone'
        END AS DuplicateMatchReason
    FROM [dbo].[Candidate]
    WHERE IsDeleted = 0
      AND ((@Email IS NOT NULL AND Email = @Email)
        OR (@Phone IS NOT NULL AND Phone = @Phone)
        OR (@ResumeFileHash IS NOT NULL AND ResumeFileHash = @ResumeFileHash));
END
GO
