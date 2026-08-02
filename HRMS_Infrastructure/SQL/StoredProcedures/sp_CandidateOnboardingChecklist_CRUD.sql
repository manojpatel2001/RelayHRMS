-- Create-or-update for CandidateOnboardingChecklist — one current row per application,
-- following the same "one row, look up existing first" pattern as
-- sp_CandidateResumeScreeningResult_CRUD.sql / sp_OfferSalaryFitment_CRUD.sql.

CREATE OR ALTER PROCEDURE [dbo].[sp_CandidateOnboardingChecklist_CRUD]
    @Operation VARCHAR(10),
    @CandidateOnboardingChecklistId INT = NULL,
    @CandidateApplicationId INT = NULL,
    @OfferAcceptedConfirmed BIT = 0,
    @DocumentsComplete BIT = 0,
    @BGVComplete BIT = 0,
    @MedicalComplete BIT = 0,
    @SalaryApproved BIT = 0,
    @JoiningApproved BIT = 0,
    @EmployeeCreationApproved BIT = 0,
    @FinalRemarks VARCHAR(1000) = NULL,
    @FinalApprovedBy INT = NULL,
    @JoiningStatus VARCHAR(20) = NULL,
    @ActualJoiningDate DATE = NULL,
    @IsEnabled BIT = 1,
    @CreatedBy INT = NULL,
    @UpdatedBy INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @ExistingId INT = (SELECT TOP 1 CandidateOnboardingChecklistId FROM [dbo].[CandidateOnboardingChecklist] WHERE CandidateApplicationId = @CandidateApplicationId AND IsDeleted = 0);

        IF @Operation IN ('INSERT', 'CREATE') AND @ExistingId IS NULL
        BEGIN
            INSERT INTO [dbo].[CandidateOnboardingChecklist]
            (CandidateApplicationId, OfferAcceptedConfirmed, DocumentsComplete, BGVComplete, MedicalComplete, SalaryApproved, JoiningApproved, EmployeeCreationApproved,
             FinalRemarks, FinalApprovedBy, FinalApprovedDate, JoiningStatus, ActualJoiningDate, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@CandidateApplicationId, @OfferAcceptedConfirmed, @DocumentsComplete, @BGVComplete, @MedicalComplete, @SalaryApproved, @JoiningApproved, @EmployeeCreationApproved,
             @FinalRemarks, @FinalApprovedBy, CASE WHEN @FinalApprovedBy IS NOT NULL THEN GETDATE() ELSE NULL END, ISNULL(@JoiningStatus, 'Pending'), @ActualJoiningDate, @IsEnabled, GETDATE(), @CreatedBy);

            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Checklist created successfully!' AS ResponseMessage;
        END
        ELSE
        BEGIN
            DECLARE @TargetId INT = ISNULL(@ExistingId, @CandidateOnboardingChecklistId);

            UPDATE [dbo].[CandidateOnboardingChecklist]
            SET OfferAcceptedConfirmed = @OfferAcceptedConfirmed,
                DocumentsComplete = @DocumentsComplete,
                BGVComplete = @BGVComplete,
                MedicalComplete = @MedicalComplete,
                SalaryApproved = @SalaryApproved,
                JoiningApproved = @JoiningApproved,
                EmployeeCreationApproved = @EmployeeCreationApproved,
                FinalRemarks = @FinalRemarks,
                FinalApprovedBy = ISNULL(@FinalApprovedBy, FinalApprovedBy),
                FinalApprovedDate = CASE WHEN @FinalApprovedBy IS NOT NULL THEN GETDATE() ELSE FinalApprovedDate END,
                JoiningStatus = ISNULL(@JoiningStatus, JoiningStatus),
                ActualJoiningDate = ISNULL(@ActualJoiningDate, ActualJoiningDate),
                UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE CandidateOnboardingChecklistId = @TargetId;

            SELECT @TargetId AS NewId, 1 AS Success, 'Checklist updated successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetCandidateOnboardingChecklistByApplicationId]
    @CandidateApplicationId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 * FROM [dbo].[CandidateOnboardingChecklist] WHERE CandidateApplicationId = @CandidateApplicationId AND IsDeleted = 0 ORDER BY CreatedDate DESC;
END
GO
