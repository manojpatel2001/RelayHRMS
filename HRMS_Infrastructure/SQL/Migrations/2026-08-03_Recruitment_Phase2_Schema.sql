-- Recruitment Module — Phase 2 ("Interview to Offer") schema.
-- Interview Management, Selection Decision, self-contained Salary Fitment/CTC Builder,
-- Offer + self-contained Offer Approval subsystem.
-- Convention matches Phase 1 (2026-08-02_Recruitment_Phase1_Schema.sql): IsDeleted/IsEnabled
-- BIT, CreatedDate/CreatedBy, UpdatedDate/UpdatedBy, DeletedDate/DeletedBy (int audit ids),
-- logical FKs only, soft delete on DELETE. FKs point at CandidateApplicationId (the
-- pursuit-of-one-position identity), not CandidateId, per the Phase 1 schema's own design note.

-- 1. Interview
CREATE TABLE [dbo].[Interview] (
    InterviewId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateApplicationId INT NOT NULL,
    RoundName VARCHAR(100) NOT NULL,
    RoundSequence INT NOT NULL DEFAULT 1,
    ScheduledDate DATETIME NOT NULL,
    DurationMinutes INT NULL,
    Mode VARCHAR(20) NULL,
    MeetingLink VARCHAR(500) NULL,
    Location VARCHAR(300) NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Scheduled',
    Remarks VARCHAR(1000) NULL,
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

-- 2. Interview Panelist
CREATE TABLE [dbo].[InterviewPanelist] (
    InterviewPanelistId INT IDENTITY(1,1) PRIMARY KEY,
    InterviewId INT NOT NULL,
    EmployeeId INT NOT NULL,
    IsPrimary BIT NOT NULL DEFAULT 0,
    InviteStatus VARCHAR(20) NOT NULL DEFAULT 'Pending',
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

-- 3. Interview Feedback — one row per panelist per interview.
CREATE TABLE [dbo].[InterviewFeedback] (
    InterviewFeedbackId INT IDENTITY(1,1) PRIMARY KEY,
    InterviewId INT NOT NULL,
    PanelistEmployeeId INT NOT NULL,
    TechnicalRating INT NULL,
    CommunicationRating INT NULL,
    ProblemSolvingRating INT NULL,
    OverallRating INT NULL,
    Strengths VARCHAR(1000) NULL,
    Weaknesses VARCHAR(1000) NULL,
    Recommendation VARCHAR(20) NULL,
    Remarks VARCHAR(1000) NULL,
    IsSubmitted BIT NOT NULL DEFAULT 0,
    SubmittedDate DATETIME NULL,
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
CREATE UNIQUE INDEX UX_InterviewFeedback_Panelist ON [dbo].[InterviewFeedback](InterviewId, PanelistEmployeeId) WHERE IsDeleted = 0;
GO

-- 4. Selection Decision — consolidates panel feedback into one final call per application.
CREATE TABLE [dbo].[SelectionDecision] (
    SelectionDecisionId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateApplicationId INT NOT NULL,
    DecisionStatus VARCHAR(20) NOT NULL,
    DecidedBy INT NULL,
    DecisionDate DATETIME NOT NULL DEFAULT GETDATE(),
    Remarks VARCHAR(1000) NULL,
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

-- 5. Salary Structure Template — self-contained configurable CTC rule engine (does not
-- exist anywhere else in this codebase; does not touch Earning/Deduction/payroll).
CREATE TABLE [dbo].[SalaryStructureTemplate] (
    SalaryStructureTemplateId INT IDENTITY(1,1) PRIMARY KEY,
    TemplateName VARCHAR(150) NOT NULL,
    GradeId INT NULL,
    DesignationId INT NULL,
    CompanyId INT NULL,
    IsDefault BIT NOT NULL DEFAULT 0,
    Status VARCHAR(20) NOT NULL DEFAULT 'Active',
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

-- 6. Salary Structure Component — the % / flat-amount rules per template.
CREATE TABLE [dbo].[SalaryStructureComponent] (
    SalaryStructureComponentId INT IDENTITY(1,1) PRIMARY KEY,
    SalaryStructureTemplateId INT NOT NULL,
    ComponentName VARCHAR(50) NOT NULL,
    ComponentCategory VARCHAR(20) NOT NULL,
    CalculationType VARCHAR(20) NOT NULL,
    Value DECIMAL(9,4) NOT NULL,
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

-- 7. Offer Salary Fitment — one per offer.
CREATE TABLE [dbo].[OfferSalaryFitment] (
    OfferSalaryFitmentId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateApplicationId INT NOT NULL,
    SalaryStructureTemplateId INT NULL,
    CurrentCTC DECIMAL(18,2) NULL,
    ExpectedCTC DECIMAL(18,2) NULL,
    RecommendedCTC DECIMAL(18,2) NULL,
    FinalCTC DECIMAL(18,2) NULL,
    BudgetValidationStatus VARCHAR(20) NOT NULL DEFAULT 'NotValidated',
    Remarks VARCHAR(1000) NULL,
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

-- 8. Offer Salary Breakup Component — the CTC Builder's computed output for one offer.
CREATE TABLE [dbo].[OfferSalaryBreakupComponent] (
    OfferSalaryBreakupComponentId INT IDENTITY(1,1) PRIMARY KEY,
    OfferSalaryFitmentId INT NOT NULL,
    ComponentName VARCHAR(50) NOT NULL,
    ComponentCategory VARCHAR(20) NOT NULL,
    Frequency VARCHAR(20) NOT NULL DEFAULT 'Monthly',
    Amount DECIMAL(18,2) NOT NULL DEFAULT 0,
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

-- 9. Offer
CREATE TABLE [dbo].[Offer] (
    OfferId INT IDENTITY(1,1) PRIMARY KEY,
    CandidateApplicationId INT NOT NULL,
    JobPositionId INT NOT NULL,
    OfferSalaryFitmentId INT NULL,
    OfferedCTC DECIMAL(18,2) NULL,
    DesignationId INT NULL,
    DepartmentId INT NULL,
    BranchId INT NULL,
    GradeId INT NULL,
    JoiningDate DATE NULL,
    OfferExpiryDate DATE NULL,
    OfferStatus VARCHAR(20) NOT NULL DEFAULT 'Draft',
    OfferLetterUrl VARCHAR(500) NULL,
    ReleasedDate DATETIME NULL,
    AcceptedDate DATETIME NULL,
    AcceptedByCandidateName VARCHAR(200) NULL,
    DeclineReason VARCHAR(500) NULL,
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

-- 10. Offer Approval Level Config — admin-configurable approver chain per company.
CREATE TABLE [dbo].[OfferApprovalLevelConfig] (
    OfferApprovalLevelConfigId INT IDENTITY(1,1) PRIMARY KEY,
    CompanyId INT NULL,
    LevelNo INT NOT NULL,
    ApproverRole VARCHAR(30) NOT NULL,
    FixedApproverEmployeeId INT NULL,
    EscalationDays INT NOT NULL DEFAULT 2,
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

-- 11. Offer Approval Request — one per offer submitted for approval.
CREATE TABLE [dbo].[OfferApprovalRequest] (
    OfferApprovalRequestId INT IDENTITY(1,1) PRIMARY KEY,
    OfferId INT NOT NULL,
    CurrentLevelNo INT NOT NULL DEFAULT 1,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending',
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

-- 12. Offer Approval Request Level — one row per level-instance for a given request.
CREATE TABLE [dbo].[OfferApprovalRequestLevel] (
    OfferApprovalRequestLevelId INT IDENTITY(1,1) PRIMARY KEY,
    OfferApprovalRequestId INT NOT NULL,
    LevelNo INT NOT NULL,
    ApproverEmployeeId INT NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending',
    ActionRemarks VARCHAR(1000) NULL,
    ActionBy INT NULL,
    ActionOn DATETIME NULL,
    EscalationDueOn DATETIME NULL,
    EscalatedOn DATETIME NULL,
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

-- 13. Offer Approval Request History — immutable audit log.
CREATE TABLE [dbo].[OfferApprovalRequestHistory] (
    OfferApprovalRequestHistoryId INT IDENTITY(1,1) PRIMARY KEY,
    OfferApprovalRequestId INT NOT NULL,
    OfferApprovalRequestLevelId INT NOT NULL,
    ActionType VARCHAR(20) NOT NULL,
    ActionBy INT NULL,
    Remarks VARCHAR(1000) NULL,
    OldStatus VARCHAR(20) NULL,
    NewStatus VARCHAR(20) NULL,
    ActionDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NULL
);
GO
