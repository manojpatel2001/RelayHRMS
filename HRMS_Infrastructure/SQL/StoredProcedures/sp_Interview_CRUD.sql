-- CRUD for Interview + InterviewPanelist, called from InterviewRepository via Dapper.
-- Mirrors sp_EmployeeRecruitmentDetails_CRUD.sql's @Operation-discriminated pattern.

CREATE OR ALTER PROCEDURE [dbo].[sp_Interview_CRUD]
    @Operation VARCHAR(10),
    @InterviewId INT = NULL,
    @CandidateApplicationId INT = NULL,
    @RoundName VARCHAR(100) = NULL,
    @RoundSequence INT = 1,
    @ScheduledDate DATETIME = NULL,
    @DurationMinutes INT = NULL,
    @Mode VARCHAR(20) = NULL,
    @MeetingLink VARCHAR(500) = NULL,
    @Location VARCHAR(300) = NULL,
    @Status VARCHAR(20) = NULL,
    @Remarks VARCHAR(1000) = NULL,
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
            INSERT INTO [dbo].[Interview]
            (CandidateApplicationId, RoundName, RoundSequence, ScheduledDate, DurationMinutes, Mode, MeetingLink, Location, Status, Remarks, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@CandidateApplicationId, @RoundName, @RoundSequence, @ScheduledDate, @DurationMinutes, @Mode, @MeetingLink, @Location, ISNULL(@Status, 'Scheduled'), @Remarks, @IsEnabled, GETDATE(), @CreatedBy);

            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Interview scheduled successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[Interview]
            SET
                RoundName = ISNULL(@RoundName, RoundName),
                RoundSequence = ISNULL(@RoundSequence, RoundSequence),
                ScheduledDate = ISNULL(@ScheduledDate, ScheduledDate),
                DurationMinutes = @DurationMinutes,
                Mode = @Mode,
                MeetingLink = @MeetingLink,
                Location = @Location,
                Status = ISNULL(@Status, Status),
                Remarks = @Remarks,
                IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(),
                UpdatedBy = @UpdatedBy
            WHERE InterviewId = @InterviewId;

            SELECT 1 AS Success, 'Interview updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[Interview]
            SET IsDeleted = 1, IsEnabled = 0, Status = 'Cancelled', DeletedDate = GETDATE(), DeletedBy = @DeletedBy
            WHERE InterviewId = @InterviewId;

            SELECT 1 AS Success, 'Interview cancelled successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetAllInterviews]
    @CandidateApplicationId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT i.*, ca.CandidateId, c.FullName, pos.PositionTitle
    FROM [dbo].[Interview] i
    INNER JOIN [dbo].[CandidateApplication] ca ON ca.CandidateApplicationId = i.CandidateApplicationId
    INNER JOIN [dbo].[Candidate] c ON c.CandidateId = ca.CandidateId
    LEFT JOIN [dbo].[JobPositionMaster] pos ON pos.JobPositionId = ca.JobPositionId
    WHERE i.IsDeleted = 0 AND (@CandidateApplicationId IS NULL OR i.CandidateApplicationId = @CandidateApplicationId)
    ORDER BY i.ScheduledDate DESC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetInterviewById]
    @InterviewId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT i.*, ca.CandidateId, c.FullName, c.Email, pos.PositionTitle
    FROM [dbo].[Interview] i
    INNER JOIN [dbo].[CandidateApplication] ca ON ca.CandidateApplicationId = i.CandidateApplicationId
    INNER JOIN [dbo].[Candidate] c ON c.CandidateId = ca.CandidateId
    LEFT JOIN [dbo].[JobPositionMaster] pos ON pos.JobPositionId = ca.JobPositionId
    WHERE i.InterviewId = @InterviewId AND i.IsDeleted = 0;
END
GO

-- Calendar feed: all interviews for a company within a date range.
CREATE OR ALTER PROCEDURE [dbo].[GetInterviewCalendarEvents]
    @CompanyId INT = NULL,
    @FromDate DATETIME,
    @ToDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    SELECT i.InterviewId, i.RoundName, i.ScheduledDate, i.DurationMinutes, i.Mode, i.Status,
           c.FullName, pos.PositionTitle
    FROM [dbo].[Interview] i
    INNER JOIN [dbo].[CandidateApplication] ca ON ca.CandidateApplicationId = i.CandidateApplicationId
    INNER JOIN [dbo].[Candidate] c ON c.CandidateId = ca.CandidateId
    LEFT JOIN [dbo].[JobPositionMaster] pos ON pos.JobPositionId = ca.JobPositionId
    WHERE i.IsDeleted = 0
      AND i.ScheduledDate BETWEEN @FromDate AND @ToDate
      AND (@CompanyId IS NULL OR pos.CompanyId = @CompanyId);
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetInterviewsByPanelistEmployeeId]
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT i.*, ca.CandidateId, c.FullName, pos.PositionTitle, ip.InterviewPanelistId, ip.IsPrimary, ip.InviteStatus,
           CASE WHEN fb.InterviewFeedbackId IS NULL THEN 0 ELSE fb.IsSubmitted END AS HasFeedback
    FROM [dbo].[InterviewPanelist] ip
    INNER JOIN [dbo].[Interview] i ON i.InterviewId = ip.InterviewId AND i.IsDeleted = 0
    INNER JOIN [dbo].[CandidateApplication] ca ON ca.CandidateApplicationId = i.CandidateApplicationId
    INNER JOIN [dbo].[Candidate] c ON c.CandidateId = ca.CandidateId
    LEFT JOIN [dbo].[JobPositionMaster] pos ON pos.JobPositionId = ca.JobPositionId
    LEFT JOIN [dbo].[InterviewFeedback] fb ON fb.InterviewId = i.InterviewId AND fb.PanelistEmployeeId = @EmployeeId AND fb.IsDeleted = 0
    WHERE ip.IsDeleted = 0 AND ip.EmployeeId = @EmployeeId
    ORDER BY i.ScheduledDate DESC;
END
GO

-- ===== Panelist assignment (simple CRUD, no separate history needed) =====
CREATE OR ALTER PROCEDURE [dbo].[sp_InterviewPanelist_CRUD]
    @Operation VARCHAR(10),
    @InterviewPanelistId INT = NULL,
    @InterviewId INT = NULL,
    @EmployeeId INT = NULL,
    @IsPrimary BIT = 0,
    @InviteStatus VARCHAR(20) = NULL,
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
            INSERT INTO [dbo].[InterviewPanelist] (InterviewId, EmployeeId, IsPrimary, InviteStatus, IsEnabled, CreatedDate, CreatedBy)
            VALUES (@InterviewId, @EmployeeId, @IsPrimary, ISNULL(@InviteStatus, 'Pending'), @IsEnabled, GETDATE(), @CreatedBy);
            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Panelist added successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[InterviewPanelist]
            SET IsPrimary = ISNULL(@IsPrimary, IsPrimary), InviteStatus = ISNULL(@InviteStatus, InviteStatus),
                IsEnabled = ISNULL(@IsEnabled, IsEnabled), UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE InterviewPanelistId = @InterviewPanelistId;
            SELECT 1 AS Success, 'Panelist updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[InterviewPanelist] SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy WHERE InterviewPanelistId = @InterviewPanelistId;
            SELECT 1 AS Success, 'Panelist removed successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

-- Panelist display names are resolved in C# via the existing EmployeeManageRepository
-- (GetEmployeeById), not a raw JOIN here — the live Employee table name/columns aren't
-- confirmed in source control, so this avoids writing SQL against an unverified schema.
CREATE OR ALTER PROCEDURE [dbo].[GetInterviewPanelistsByInterviewId]
    @InterviewId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[InterviewPanelist] WHERE InterviewId = @InterviewId AND IsDeleted = 0;
END
GO
