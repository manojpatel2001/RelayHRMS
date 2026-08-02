-- Recruitment Module — Phase 1 ("Foundation") schema.
-- Job Position Master, Job Posting, Candidate + profile children, Candidate Application,
-- Resume Screening Result, Candidate Pipeline Stage/History.
-- Convention matches 2026-08-01_EmployeeLifecycle_Schema.sql: IsDeleted/IsEnabled BIT,
-- CreatedDate/CreatedBy, UpdatedDate/UpdatedBy, DeletedDate/DeletedBy (int audit ids),
-- logical FKs only (no enforced FK constraints), soft delete on DELETE.

-- 1. Job Position Master — one approved ManpowerRequisition can spawn N positions.
CREATE TABLE [dbo].[JobPositionMaster] (
    JobPositionId INT IDENTITY(1,1) PRIMARY KEY,
    ManpowerRequisitionId INT NOT NULL,
    PositionTitle VARCHAR(200) NOT NULL,
    DesignationId INT NULL,
    DepartmentId INT NULL,
    BranchId INT NULL,
    CompanyId INT NULL,
    GradeId INT NULL,
    EmploymentType VARCHAR(50) NULL,
    VacancyCount INT NOT NULL DEFAULT 1,
    FilledCount INT NOT NULL DEFAULT 0,
    MinExperienceYears DECIMAL(4,1) NULL,
    MaxExperienceYears DECIMAL(4,1) NULL,
    MinEducationLevel VARCHAR(50) NULL,
    RequiredSkills VARCHAR(MAX) NULL,
    PreferredSkills VARCHAR(MAX) NULL,
    JobDescription VARCHAR(MAX) NULL,
    MinBudget DECIMAL(18,2) NULL,
    MaxBudget DECIMAL(18,2) NULL,
    Priority VARCHAR(10) NULL,
    OwnerRecruiterId INT NULL,
    ReportingManagerId INT NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Open',
    TargetClosureDate DATE NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);
GO

-- 2. Job Posting — internal/external publication of a Job Position.
CREATE TABLE [dbo].[JobPosting] (
    JobPostingId INT IDENTITY(1,1) PRIMARY KEY,
    JobPositionId INT NOT NULL,
    PostingTitle VARCHAR(200) NOT NULL,
    Slug VARCHAR(250) NOT NULL,
    IsInternal BIT NOT NULL DEFAULT 1,
    IsExternal BIT NOT NULL DEFAULT 0,
    JobDescriptionHtml VARCHAR(MAX) NULL,
    ResponsibilitiesHtml VARCHAR(MAX) NULL,
    BenefitsHtml VARCHAR(MAX) NULL,
    Location VARCHAR(200) NULL,
    WorkMode VARCHAR(20) NULL,
    PublishDate DATETIME NULL,
    ExpiryDate DATETIME NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Draft',
    ViewCount INT NOT NULL DEFAULT 0,
    ApplicationCount INT NOT NULL DEFAULT 0,
    CompanyId INT NULL,
    LinkedInEnabled BIT NOT NULL DEFAULT 0,
    LinkedInStatus VARCHAR(20) NULL,
    LinkedInPostedDate DATETIME NULL,
    LinkedInPostingUrl VARCHAR(500) NULL,
    NaukriEnabled BIT NOT NULL DEFAULT 0,
    NaukriStatus VARCHAR(20) NULL,
    NaukriPostedDate DATETIME NULL,
    NaukriPostingUrl VARCHAR(500) NULL,
    IndeedEnabled BIT NOT NULL DEFAULT 0,
    IndeedStatus VARCHAR(20) NULL,
    IndeedPostedDate DATETIME NULL,
    IndeedPostingUrl VARCHAR(500) NULL,
    ReferralEnabled BIT NOT NULL DEFAULT 0,
    ReferralStatus VARCHAR(20) NULL,
    ReferralPostedDate DATETIME NULL,
    ReferralPostingUrl VARCHAR(500) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);
GO
CREATE UNIQUE INDEX UX_JobPosting_Slug ON [dbo].[JobPosting](Slug) WHERE IsDeleted = 0;
GO

-- 3. Candidate — stable person identity (Phase 2/3 Interview/Offer/BGV anchor).
CREATE TABLE [dbo].[Candidate] (
    CandidateId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    MiddleName VARCHAR(100) NULL,
    LastName VARCHAR(100) NULL,
    FullName VARCHAR(300) NULL,
    Email VARCHAR(150) NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    AlternatePhone VARCHAR(20) NULL,
    DateOfBirth DATE NULL,
    Gender VARCHAR(10) NULL,
    CurrentAddress VARCHAR(500) NULL,
    CurrentCity VARCHAR(100) NULL,
    PreferredLocation VARCHAR(200) NULL,
    CurrentEmployer VARCHAR(200) NULL,
    CurrentDesignation VARCHAR(150) NULL,
    TotalExperienceYears DECIMAL(4,1) NULL,
    RelevantExperienceYears DECIMAL(4,1) NULL,
    CurrentCTC DECIMAL(18,2) NULL,
    ExpectedCTC DECIMAL(18,2) NULL,
    NoticePeriodDays INT NULL,
    LinkedInUrl VARCHAR(300) NULL,
    GitHubUrl VARCHAR(300) NULL,
    PortfolioUrl VARCHAR(300) NULL,
    ResumeUrl VARCHAR(500) NULL,
    ResumeFileHash VARCHAR(128) NULL,
    Source VARCHAR(50) NULL,
    ReferredByEmployeeId INT NULL,
    Summary VARCHAR(2000) NULL,
    IsBlacklisted BIT NOT NULL DEFAULT 0,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);
GO

-- 4. Candidate profile child tables (separate tables, not JSON — screening needs to
-- query/aggregate these relationally; mirrors the EmployeeCertification precedent).
CREATE TABLE [dbo].[CandidateEducation] (
    CandidateEducationId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateId INT NOT NULL,
    Degree VARCHAR(150) NOT NULL,
    Specialization VARCHAR(150) NULL,
    Institution VARCHAR(200) NULL,
    University VARCHAR(200) NULL,
    YearOfPassing INT NULL,
    PercentageOrCGPA VARCHAR(20) NULL,
    EducationLevel VARCHAR(50) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);
GO

CREATE TABLE [dbo].[CandidateExperience] (
    CandidateExperienceId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateId INT NOT NULL,
    CompanyName VARCHAR(200) NOT NULL,
    Designation VARCHAR(150) NULL,
    StartDate DATE NULL,
    EndDate DATE NULL,
    IsCurrentEmployer BIT NOT NULL DEFAULT 0,
    DurationMonths INT NULL,
    Responsibilities VARCHAR(1000) NULL,
    Location VARCHAR(150) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);
GO

CREATE TABLE [dbo].[CandidateSkill] (
    CandidateSkillId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateId INT NOT NULL,
    SkillName VARCHAR(150) NOT NULL,
    ProficiencyLevel VARCHAR(20) NULL,
    ExperienceYears DECIMAL(4,1) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);
GO

CREATE TABLE [dbo].[CandidateCertification] (
    CandidateCertificationId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateId INT NOT NULL,
    CertificationName VARCHAR(200) NOT NULL,
    IssuingBody VARCHAR(200) NULL,
    IssueDate DATE NULL,
    ExpiryDate DATE NULL,
    CertificateUrl VARCHAR(500) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);
GO

CREATE TABLE [dbo].[CandidateProject] (
    CandidateProjectId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateId INT NOT NULL,
    ProjectName VARCHAR(200) NOT NULL,
    Description VARCHAR(1000) NULL,
    RoleInProject VARCHAR(150) NULL,
    TechnologiesUsed VARCHAR(500) NULL,
    StartDate DATE NULL,
    EndDate DATE NULL,
    ProjectUrl VARCHAR(300) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);
GO

-- 5. Candidate Application — one candidate's pursuit of one Job Position. Phase 2+
-- Interview/Offer/BGV records will FK to CandidateApplicationId, not CandidateId.
CREATE TABLE [dbo].[CandidateApplication] (
    CandidateApplicationId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateId INT NOT NULL,
    JobPositionId INT NOT NULL,
    JobPostingId INT NULL,
    ApplicationDate DATETIME NOT NULL DEFAULT GETDATE(),
    ApplicationSource VARCHAR(50) NULL,
    ResumeUrl VARCHAR(500) NULL,
    CoverLetter VARCHAR(2000) NULL,
    CurrentPipelineStageId INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);
GO

-- 6. Rule-based Resume Screening result — one current row per application.
CREATE TABLE [dbo].[CandidateResumeScreeningResult] (
    CandidateResumeScreeningResultId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateApplicationId INT NOT NULL,
    JobPositionId INT NOT NULL,
    SkillMatchScore DECIMAL(5,2) NULL,
    ExperienceMatchScore DECIMAL(5,2) NULL,
    EducationMatchScore DECIMAL(5,2) NULL,
    OverallScore DECIMAL(5,2) NOT NULL,
    Recommendation VARCHAR(20) NOT NULL,
    MatchedSkills VARCHAR(1000) NULL,
    MissingSkills VARCHAR(1000) NULL,
    IsDuplicate BIT NOT NULL DEFAULT 0,
    DuplicateOfCandidateId INT NULL,
    DuplicateMatchReason VARCHAR(100) NULL,
    ScreenedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ScreeningEngineVersion VARCHAR(20) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);
GO
CREATE UNIQUE INDEX UX_CandidateResumeScreeningResult_Application ON [dbo].[CandidateResumeScreeningResult](CandidateApplicationId) WHERE IsDeleted = 0;
GO

-- 7. Candidate Pipeline Stage — extensible lookup, gapped SortOrder so Phase 2 can
-- insert new interview rounds without renumbering.
CREATE TABLE [dbo].[CandidatePipelineStage] (
    CandidatePipelineStageId INT IDENTITY(1,1) PRIMARY KEY,
    StageName VARCHAR(50) NOT NULL,
    StageCategory VARCHAR(20) NOT NULL,
    SortOrder INT NOT NULL,
    IsTerminal BIT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CompanyId INT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);
GO

INSERT INTO [dbo].[CandidatePipelineStage] (StageName, StageCategory, SortOrder, IsTerminal) VALUES
    ('Applied', 'Sourcing', 10, 0),
    ('Screening', 'Screening', 20, 0),
    ('Shortlisted', 'Screening', 30, 0),
    ('Interview Scheduled', 'Interview', 40, 0),
    ('Interview Round 1', 'Interview', 50, 0),
    ('Technical Round', 'Interview', 60, 0),
    ('HR Round', 'Interview', 70, 0),
    ('Management Round', 'Interview', 80, 0),
    ('Selected', 'Offer', 90, 0),
    ('Offer Released', 'Offer', 100, 0),
    ('Offer Accepted', 'Offer', 110, 0),
    ('Joined', 'Closure', 120, 1),
    ('Rejected', 'Closure', 999, 1);
GO

-- 8. Candidate Pipeline History — immutable transition log (Kanban drag-drop audit trail).
CREATE TABLE [dbo].[CandidatePipelineHistory] (
    CandidatePipelineHistoryId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateApplicationId INT NOT NULL,
    FromStageId INT NULL,
    ToStageId INT NOT NULL,
    TransitionDate DATETIME NOT NULL DEFAULT GETDATE(),
    Remarks VARCHAR(1000) NULL,
    ActionBy INT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);
GO
