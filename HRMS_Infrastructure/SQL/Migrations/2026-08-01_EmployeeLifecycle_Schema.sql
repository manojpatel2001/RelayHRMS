-- Employee History / Employee Lifecycle module — 9 new tables for categories that had
-- no backing data anywhere in the existing schema (confirmed via INFORMATION_SCHEMA
-- audit against this dev DB): Promotions, Department Changes, Shift History,
-- Recruitment, Performance Reviews, Trainings, Certifications, Awards, Disciplinary
-- Actions. Audit columns match the EmployeeTransfer/ReportingManagerDetails convention
-- (int CreatedBy/UpdatedBy/DeletedBy, not the nvarchar-string convention some older
-- tables use). Paired CRUD stored procedures live in ../StoredProcedures/, one file
-- per table, following the sp_NewsAnnouncement_CRUD.sql pattern.

-- 1. Promotions (Designation/Grade change history)
CREATE TABLE [dbo].[EmployeeDesignationHistory] (
    EmployeeDesignationHistoryId INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    OldDesignationId INT NULL,
    NewDesignationId INT NULL,
    OldGradeId INT NULL,
    NewGradeId INT NULL,
    EffectiveDate DATE NOT NULL,
    Reason VARCHAR(500) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);

-- 2. Department Changes
CREATE TABLE [dbo].[EmployeeDepartmentHistory] (
    EmployeeDepartmentHistoryId INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    OldDepartmentId INT NULL,
    NewDepartmentId INT NULL,
    EffectiveDate DATE NOT NULL,
    Reason VARCHAR(500) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);

-- 3. Shift History
CREATE TABLE [dbo].[EmployeeShiftHistory] (
    EmployeeShiftHistoryId INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    OldShiftMasterId INT NULL,
    NewShiftMasterId INT NULL,
    EffectiveDate DATE NOT NULL,
    Reason VARCHAR(500) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);

-- 4. Recruitment (pre-joining record for an already-hired employee — not a
-- multi-candidate ATS pipeline)
CREATE TABLE [dbo].[EmployeeRecruitmentDetails] (
    EmployeeRecruitmentDetailsId INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    ApplicationDate DATE NULL,
    InterviewDate DATE NULL,
    InterviewerName VARCHAR(200) NULL,
    Source VARCHAR(50) NULL,
    OfferDate DATE NULL,
    OfferAcceptedDate DATE NULL,
    Remarks VARCHAR(500) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);

-- 5. Performance Reviews
CREATE TABLE [dbo].[EmployeePerformanceReview] (
    EmployeePerformanceReviewId INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    ReviewPeriodStart DATE NULL,
    ReviewPeriodEnd DATE NULL,
    ReviewDate DATE NOT NULL,
    ReviewerId INT NULL,
    Rating INT NULL,
    Strengths VARCHAR(1000) NULL,
    AreasOfImprovement VARCHAR(1000) NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Draft',
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);

-- 6. Trainings
CREATE TABLE [dbo].[EmployeeTraining] (
    EmployeeTrainingId INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    TrainingName VARCHAR(200) NOT NULL,
    TrainingType VARCHAR(100) NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NULL,
    Provider VARCHAR(200) NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Scheduled',
    Score VARCHAR(20) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);

-- 7. Certifications (separate from EmployeeProfile_Skill, which has no date columns
-- and is already in production use for the Skills tab)
CREATE TABLE [dbo].[EmployeeCertification] (
    EmployeeCertificationId INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    CertificationName VARCHAR(200) NOT NULL,
    IssuingBody VARCHAR(200) NULL,
    IssueDate DATE NOT NULL,
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

-- 8. Awards / Recognition
CREATE TABLE [dbo].[EmployeeAward] (
    EmployeeAwardId INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    AwardName VARCHAR(200) NOT NULL,
    AwardCategory VARCHAR(100) NULL,
    AwardDate DATE NOT NULL,
    AwardedBy VARCHAR(200) NULL,
    Description VARCHAR(500) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);

-- 9. Disciplinary Actions — WarningMasterId reuses the EXISTING WarningMaster catalog
-- (warning type/level lookup) as its type dropdown; this table is the missing
-- per-employee, per-incident transaction log that WarningMaster never had.
CREATE TABLE [dbo].[EmployeeWarningHistory] (
    EmployeeWarningHistoryId INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    WarningMasterId INT NOT NULL,
    IssueDate DATE NOT NULL,
    IssuedBy VARCHAR(200) NULL,
    IncidentDescription VARCHAR(1000) NULL,
    ActionTaken VARCHAR(500) NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Issued',
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL,
    DeletedDate DATETIME NULL,
    DeletedBy INT NULL
);
