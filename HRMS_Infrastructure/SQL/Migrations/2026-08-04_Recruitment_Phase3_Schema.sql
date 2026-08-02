-- Recruitment Module — Phase 3 ("Onboarding") schema.
-- Document Collection, Background Verification (+ optional Medical), HR Approval
-- Checklist + Joining Confirmation, Employee Conversion audit/link.
-- Convention matches Phases 1-2: IsDeleted/IsEnabled BIT, CreatedDate/CreatedBy,
-- UpdatedDate/UpdatedBy, DeletedDate/DeletedBy (int audit ids), logical FKs only,
-- soft delete on DELETE. FKs point at CandidateApplicationId.
-- Self-contained (does not touch the legacy Employee AttachmentDetails/DocumentType
-- tables — their live schema isn't in source control and DocumentType has no admin
-- CRUD anywhere), mirroring the Phase 2 Offer Approval decision.

-- 1. Candidate Document Type — admin-configurable master (closes the gap the legacy
-- DocumentType has: no CRUD screen anywhere in this codebase).
CREATE TABLE [dbo].[CandidateDocumentType] (
    CandidateDocumentTypeId INT IDENTITY(1,1) PRIMARY KEY,
    DocumentName VARCHAR(150) NOT NULL,
    IsMandatory BIT NOT NULL DEFAULT 0,
    SortOrder INT NOT NULL DEFAULT 0,
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

INSERT INTO [dbo].[CandidateDocumentType] (DocumentName, IsMandatory, SortOrder) VALUES
    ('PAN Card', 1, 10),
    ('Aadhaar Card', 1, 20),
    ('Passport', 0, 30),
    ('Driving License', 0, 40),
    ('Photo', 1, 50),
    ('Bank Passbook / Cancelled Cheque', 1, 60),
    ('Education Certificates', 1, 70),
    ('Experience Certificates', 0, 80),
    ('Previous Salary Slip', 0, 90),
    ('Relieving Letter', 0, 100),
    ('PF Details', 0, 110),
    ('ESIC Details', 0, 120);
GO

-- 2. Candidate Document — the uploaded file per type per application.
CREATE TABLE [dbo].[CandidateDocument] (
    CandidateDocumentId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateApplicationId INT NOT NULL,
    CandidateDocumentTypeId INT NOT NULL,
    DocumentUrl VARCHAR(500) NULL,
    Remarks VARCHAR(500) NULL,
    VerifiedStatus VARCHAR(20) NOT NULL DEFAULT 'Pending',
    VerifiedBy INT NULL,
    VerifiedDate DATETIME NULL,
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

-- 3. Candidate Background Verification Check — one row per check type per application.
-- 'Medical' is included as just another CheckType (UI marks it optional) rather than a
-- separate table. Vendor tracking is a plain name/contact pair, not a separate master.
CREATE TABLE [dbo].[CandidateBGVCheck] (
    CandidateBGVCheckId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateApplicationId INT NOT NULL,
    CheckType VARCHAR(30) NOT NULL,
    VendorName VARCHAR(200) NULL,
    VendorContact VARCHAR(100) NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending',
    InitiatedDate DATETIME NULL,
    CompletedDate DATETIME NULL,
    ProofDocumentUrl VARCHAR(500) NULL,
    Remarks VARCHAR(1000) NULL,
    VerifiedBy INT NULL,
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

-- 4. Candidate Onboarding Checklist — one row per application; HR-toggled sign-off flags
-- plus Joining Confirmation fields. Mirrors the Exit-Clearance NOC precedent's
-- parent-status-flags half (fixed, small item count, so no child table needed here).
CREATE TABLE [dbo].[CandidateOnboardingChecklist] (
    CandidateOnboardingChecklistId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateApplicationId INT NOT NULL,
    OfferAcceptedConfirmed BIT NOT NULL DEFAULT 0,
    DocumentsComplete BIT NOT NULL DEFAULT 0,
    BGVComplete BIT NOT NULL DEFAULT 0,
    MedicalComplete BIT NOT NULL DEFAULT 0,
    SalaryApproved BIT NOT NULL DEFAULT 0,
    JoiningApproved BIT NOT NULL DEFAULT 0,
    EmployeeCreationApproved BIT NOT NULL DEFAULT 0,
    FinalRemarks VARCHAR(1000) NULL,
    FinalApprovedBy INT NULL,
    FinalApprovedDate DATETIME NULL,
    JoiningStatus VARCHAR(20) NOT NULL DEFAULT 'Pending',
    ActualJoiningDate DATE NULL,
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
CREATE UNIQUE INDEX UX_CandidateOnboardingChecklist_Application ON [dbo].[CandidateOnboardingChecklist](CandidateApplicationId) WHERE IsDeleted = 0;
GO

-- 5. Candidate Employee Conversion — durable link/audit from a recruitment record to the
-- resulting Employee row (Employee creation doesn't hand back its new Id, and this is
-- useful later for "hired via Recruitment" reporting).
CREATE TABLE [dbo].[CandidateEmployeeConversion] (
    CandidateEmployeeConversionId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateApplicationId INT NOT NULL,
    EmployeeId INT NULL,
    EmployeeCode VARCHAR(50) NULL,
    AlfaEmployeeCode VARCHAR(50) NULL,
    ConversionStatus VARCHAR(20) NOT NULL DEFAULT 'Pending',
    FailureReason VARCHAR(500) NULL,
    ConvertedDate DATETIME NULL,
    ConvertedBy INT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL,
    UpdatedDate DATETIME NULL,
    UpdatedBy INT NULL
);
GO
