-- Combined CRUD for the 5 Candidate profile child tables (Education/Experience/Skill/
-- Certification/Project) — grouped into one file since all 5 are identical-shape
-- CRUD-by-CandidateId child records, following sp_EmployeeRecruitmentDetails_CRUD.sql's
-- @Operation-discriminated pattern.

-- ===== Education =====
CREATE OR ALTER PROCEDURE [dbo].[sp_CandidateEducation_CRUD]
    @Operation VARCHAR(10),
    @CandidateEducationId INT = NULL,
    @CandidateId INT = NULL,
    @Degree VARCHAR(150) = NULL,
    @Specialization VARCHAR(150) = NULL,
    @Institution VARCHAR(200) = NULL,
    @University VARCHAR(200) = NULL,
    @YearOfPassing INT = NULL,
    @PercentageOrCGPA VARCHAR(20) = NULL,
    @EducationLevel VARCHAR(50) = NULL,
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
            INSERT INTO [dbo].[CandidateEducation]
            (CandidateId, Degree, Specialization, Institution, University, YearOfPassing, PercentageOrCGPA, EducationLevel, IsEnabled, CreatedDate, CreatedBy)
            VALUES (@CandidateId, @Degree, @Specialization, @Institution, @University, @YearOfPassing, @PercentageOrCGPA, @EducationLevel, @IsEnabled, GETDATE(), @CreatedBy);
            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Education record saved successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[CandidateEducation]
            SET Degree = ISNULL(@Degree, Degree), Specialization = @Specialization, Institution = @Institution,
                University = @University, YearOfPassing = @YearOfPassing, PercentageOrCGPA = @PercentageOrCGPA,
                EducationLevel = @EducationLevel, IsEnabled = ISNULL(@IsEnabled, IsEnabled), UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE CandidateEducationId = @CandidateEducationId;
            SELECT 1 AS Success, 'Education record updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[CandidateEducation] SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy WHERE CandidateEducationId = @CandidateEducationId;
            SELECT 1 AS Success, 'Education record deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetCandidateEducationByCandidateId]
    @CandidateId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[CandidateEducation] WHERE CandidateId = @CandidateId AND IsDeleted = 0 ORDER BY YearOfPassing DESC;
END
GO

-- ===== Experience =====
CREATE OR ALTER PROCEDURE [dbo].[sp_CandidateExperience_CRUD]
    @Operation VARCHAR(10),
    @CandidateExperienceId INT = NULL,
    @CandidateId INT = NULL,
    @CompanyName VARCHAR(200) = NULL,
    @Designation VARCHAR(150) = NULL,
    @StartDate DATE = NULL,
    @EndDate DATE = NULL,
    @IsCurrentEmployer BIT = 0,
    @DurationMonths INT = NULL,
    @Responsibilities VARCHAR(1000) = NULL,
    @Location VARCHAR(150) = NULL,
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
            INSERT INTO [dbo].[CandidateExperience]
            (CandidateId, CompanyName, Designation, StartDate, EndDate, IsCurrentEmployer, DurationMonths, Responsibilities, Location, IsEnabled, CreatedDate, CreatedBy)
            VALUES (@CandidateId, @CompanyName, @Designation, @StartDate, @EndDate, @IsCurrentEmployer, @DurationMonths, @Responsibilities, @Location, @IsEnabled, GETDATE(), @CreatedBy);
            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Experience record saved successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[CandidateExperience]
            SET CompanyName = ISNULL(@CompanyName, CompanyName), Designation = @Designation, StartDate = @StartDate,
                EndDate = @EndDate, IsCurrentEmployer = ISNULL(@IsCurrentEmployer, IsCurrentEmployer), DurationMonths = @DurationMonths,
                Responsibilities = @Responsibilities, Location = @Location, IsEnabled = ISNULL(@IsEnabled, IsEnabled), UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE CandidateExperienceId = @CandidateExperienceId;
            SELECT 1 AS Success, 'Experience record updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[CandidateExperience] SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy WHERE CandidateExperienceId = @CandidateExperienceId;
            SELECT 1 AS Success, 'Experience record deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetCandidateExperienceByCandidateId]
    @CandidateId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[CandidateExperience] WHERE CandidateId = @CandidateId AND IsDeleted = 0 ORDER BY StartDate DESC;
END
GO

-- ===== Skill =====
CREATE OR ALTER PROCEDURE [dbo].[sp_CandidateSkill_CRUD]
    @Operation VARCHAR(10),
    @CandidateSkillId INT = NULL,
    @CandidateId INT = NULL,
    @SkillName VARCHAR(150) = NULL,
    @ProficiencyLevel VARCHAR(20) = NULL,
    @ExperienceYears DECIMAL(4,1) = NULL,
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
            INSERT INTO [dbo].[CandidateSkill] (CandidateId, SkillName, ProficiencyLevel, ExperienceYears, IsEnabled, CreatedDate, CreatedBy)
            VALUES (@CandidateId, @SkillName, @ProficiencyLevel, @ExperienceYears, @IsEnabled, GETDATE(), @CreatedBy);
            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Skill saved successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[CandidateSkill]
            SET SkillName = ISNULL(@SkillName, SkillName), ProficiencyLevel = @ProficiencyLevel, ExperienceYears = @ExperienceYears,
                IsEnabled = ISNULL(@IsEnabled, IsEnabled), UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE CandidateSkillId = @CandidateSkillId;
            SELECT 1 AS Success, 'Skill updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[CandidateSkill] SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy WHERE CandidateSkillId = @CandidateSkillId;
            SELECT 1 AS Success, 'Skill deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetCandidateSkillByCandidateId]
    @CandidateId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[CandidateSkill] WHERE CandidateId = @CandidateId AND IsDeleted = 0 ORDER BY SkillName;
END
GO

-- ===== Certification =====
CREATE OR ALTER PROCEDURE [dbo].[sp_CandidateCertification_CRUD]
    @Operation VARCHAR(10),
    @CandidateCertificationId INT = NULL,
    @CandidateId INT = NULL,
    @CertificationName VARCHAR(200) = NULL,
    @IssuingBody VARCHAR(200) = NULL,
    @IssueDate DATE = NULL,
    @ExpiryDate DATE = NULL,
    @CertificateUrl VARCHAR(500) = NULL,
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
            INSERT INTO [dbo].[CandidateCertification] (CandidateId, CertificationName, IssuingBody, IssueDate, ExpiryDate, CertificateUrl, IsEnabled, CreatedDate, CreatedBy)
            VALUES (@CandidateId, @CertificationName, @IssuingBody, @IssueDate, @ExpiryDate, @CertificateUrl, @IsEnabled, GETDATE(), @CreatedBy);
            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Certification saved successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[CandidateCertification]
            SET CertificationName = ISNULL(@CertificationName, CertificationName), IssuingBody = @IssuingBody, IssueDate = @IssueDate,
                ExpiryDate = @ExpiryDate, CertificateUrl = ISNULL(@CertificateUrl, CertificateUrl), IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE CandidateCertificationId = @CandidateCertificationId;
            SELECT 1 AS Success, 'Certification updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[CandidateCertification] SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy WHERE CandidateCertificationId = @CandidateCertificationId;
            SELECT 1 AS Success, 'Certification deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetCandidateCertificationByCandidateId]
    @CandidateId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[CandidateCertification] WHERE CandidateId = @CandidateId AND IsDeleted = 0 ORDER BY IssueDate DESC;
END
GO

-- ===== Project =====
CREATE OR ALTER PROCEDURE [dbo].[sp_CandidateProject_CRUD]
    @Operation VARCHAR(10),
    @CandidateProjectId INT = NULL,
    @CandidateId INT = NULL,
    @ProjectName VARCHAR(200) = NULL,
    @Description VARCHAR(1000) = NULL,
    @RoleInProject VARCHAR(150) = NULL,
    @TechnologiesUsed VARCHAR(500) = NULL,
    @StartDate DATE = NULL,
    @EndDate DATE = NULL,
    @ProjectUrl VARCHAR(300) = NULL,
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
            INSERT INTO [dbo].[CandidateProject] (CandidateId, ProjectName, Description, RoleInProject, TechnologiesUsed, StartDate, EndDate, ProjectUrl, IsEnabled, CreatedDate, CreatedBy)
            VALUES (@CandidateId, @ProjectName, @Description, @RoleInProject, @TechnologiesUsed, @StartDate, @EndDate, @ProjectUrl, @IsEnabled, GETDATE(), @CreatedBy);
            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Project saved successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[CandidateProject]
            SET ProjectName = ISNULL(@ProjectName, ProjectName), Description = @Description, RoleInProject = @RoleInProject,
                TechnologiesUsed = @TechnologiesUsed, StartDate = @StartDate, EndDate = @EndDate, ProjectUrl = @ProjectUrl,
                IsEnabled = ISNULL(@IsEnabled, IsEnabled), UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE CandidateProjectId = @CandidateProjectId;
            SELECT 1 AS Success, 'Project updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[CandidateProject] SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy WHERE CandidateProjectId = @CandidateProjectId;
            SELECT 1 AS Success, 'Project deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetCandidateProjectByCandidateId]
    @CandidateId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[CandidateProject] WHERE CandidateId = @CandidateId AND IsDeleted = 0 ORDER BY StartDate DESC;
END
GO
