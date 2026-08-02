-- Self-contained Offer Approval subsystem: OfferApprovalLevelConfig (admin-configurable
-- chain) + OfferApprovalRequest/OfferApprovalRequestLevel/OfferApprovalRequestHistory
-- (request tracking). Deliberately NOT built on the generic ApprovalMaster/ApprovalLevel/
-- ApprovalRequest/ApprovalRequestLevel tables (see Phase 2 plan) — this whole subsystem
-- lives on schema we fully control.

-- ===== Level Config (admin-configurable approver chain) =====
CREATE OR ALTER PROCEDURE [dbo].[sp_OfferApprovalLevelConfig_CRUD]
    @Operation VARCHAR(10),
    @OfferApprovalLevelConfigId INT = NULL,
    @CompanyId INT = NULL,
    @LevelNo INT = NULL,
    @ApproverRole VARCHAR(30) = NULL,
    @FixedApproverEmployeeId INT = NULL,
    @EscalationDays INT = 2,
    @IsActive BIT = 1,
    @CreatedBy INT = NULL,
    @UpdatedBy INT = NULL,
    @DeletedBy INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @Operation IN ('INSERT', 'CREATE')
        BEGIN
            INSERT INTO [dbo].[OfferApprovalLevelConfig] (CompanyId, LevelNo, ApproverRole, FixedApproverEmployeeId, EscalationDays, IsActive, CreatedDate, CreatedBy)
            VALUES (@CompanyId, @LevelNo, @ApproverRole, @FixedApproverEmployeeId, ISNULL(@EscalationDays, 2), @IsActive, GETDATE(), @CreatedBy);
            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Approval level saved successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[OfferApprovalLevelConfig]
            SET LevelNo = ISNULL(@LevelNo, LevelNo), ApproverRole = ISNULL(@ApproverRole, ApproverRole),
                FixedApproverEmployeeId = @FixedApproverEmployeeId, EscalationDays = ISNULL(@EscalationDays, EscalationDays),
                IsActive = ISNULL(@IsActive, IsActive), UpdatedDate = GETDATE(), UpdatedBy = @UpdatedBy
            WHERE OfferApprovalLevelConfigId = @OfferApprovalLevelConfigId;
            SELECT 1 AS Success, 'Approval level updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[OfferApprovalLevelConfig] SET IsDeleted = 1, IsEnabled = 0, IsActive = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy WHERE OfferApprovalLevelConfigId = @OfferApprovalLevelConfigId;
            SELECT 1 AS Success, 'Approval level removed successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetOfferApprovalLevelConfig]
    @CompanyId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[OfferApprovalLevelConfig]
    WHERE IsDeleted = 0 AND IsActive = 1 AND (@CompanyId IS NULL OR CompanyId = @CompanyId OR CompanyId IS NULL)
    ORDER BY LevelNo;
END
GO

-- ===== Request creation (levels are pre-resolved to real EmployeeIds in C# before this
-- runs — sp_OfferApprovalRequestLevel_CRUD below just persists what it's handed) =====
CREATE OR ALTER PROCEDURE [dbo].[sp_CreateOfferApprovalRequest]
    @OfferId INT,
    @CreatedBy INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO [dbo].[OfferApprovalRequest] (OfferId, CurrentLevelNo, Status, CreatedDate, CreatedBy)
        VALUES (@OfferId, 1, 'Pending', GETDATE(), @CreatedBy);

        SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Offer approval request created successfully!' AS ResponseMessage;
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_OfferApprovalRequestLevel_CRUD]
    @Operation VARCHAR(10),
    @OfferApprovalRequestLevelId INT = NULL,
    @OfferApprovalRequestId INT = NULL,
    @LevelNo INT = NULL,
    @ApproverEmployeeId INT = NULL,
    @EscalationDueOn DATETIME = NULL,
    @CreatedBy INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @Operation IN ('INSERT', 'CREATE')
        BEGIN
            INSERT INTO [dbo].[OfferApprovalRequestLevel] (OfferApprovalRequestId, LevelNo, ApproverEmployeeId, Status, EscalationDueOn, CreatedDate, CreatedBy)
            VALUES (@OfferApprovalRequestId, @LevelNo, @ApproverEmployeeId, 'Pending', @EscalationDueOn, GETDATE(), @CreatedBy);
            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Approval level seeded successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

-- ===== Approve / Reject one level — atomic: advances the request, inserts history =====
CREATE OR ALTER PROCEDURE [dbo].[sp_ActionOnOfferApprovalRequestLevel]
    @OfferApprovalRequestLevelId INT,
    @Action VARCHAR(10),          -- 'Approve' or 'Reject'
    @ActionBy INT,
    @Remarks VARCHAR(1000) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @OfferApprovalRequestId INT, @LevelNo INT, @OldStatus VARCHAR(20), @NewStatus VARCHAR(20);
        DECLARE @MaxLevelNo INT, @IsAllLevelsCompleted BIT = 0;

        SELECT @OfferApprovalRequestId = OfferApprovalRequestId, @LevelNo = LevelNo, @OldStatus = Status
        FROM [dbo].[OfferApprovalRequestLevel] WHERE OfferApprovalRequestLevelId = @OfferApprovalRequestLevelId;

        SET @NewStatus = CASE WHEN @Action = 'Approve' THEN 'Approved' ELSE 'Rejected' END;

        UPDATE [dbo].[OfferApprovalRequestLevel]
        SET Status = @NewStatus, ActionRemarks = @Remarks, ActionBy = @ActionBy, ActionOn = GETDATE(), UpdatedDate = GETDATE(), UpdatedBy = @ActionBy
        WHERE OfferApprovalRequestLevelId = @OfferApprovalRequestLevelId;

        INSERT INTO [dbo].[OfferApprovalRequestHistory] (OfferApprovalRequestId, OfferApprovalRequestLevelId, ActionType, ActionBy, Remarks, OldStatus, NewStatus, ActionDate, CreatedDate, CreatedBy)
        VALUES (@OfferApprovalRequestId, @OfferApprovalRequestLevelId, @Action, @ActionBy, @Remarks, @OldStatus, @NewStatus, GETDATE(), GETDATE(), @ActionBy);

        IF @Action = 'Reject'
        BEGIN
            UPDATE [dbo].[OfferApprovalRequest] SET Status = 'Rejected', UpdatedDate = GETDATE(), UpdatedBy = @ActionBy WHERE OfferApprovalRequestId = @OfferApprovalRequestId;
            SET @IsAllLevelsCompleted = 1;
        END
        ELSE
        BEGIN
            SELECT @MaxLevelNo = MAX(LevelNo) FROM [dbo].[OfferApprovalRequestLevel] WHERE OfferApprovalRequestId = @OfferApprovalRequestId AND IsDeleted = 0;

            IF @LevelNo >= @MaxLevelNo
            BEGIN
                UPDATE [dbo].[OfferApprovalRequest] SET Status = 'Approved', CurrentLevelNo = @LevelNo, UpdatedDate = GETDATE(), UpdatedBy = @ActionBy WHERE OfferApprovalRequestId = @OfferApprovalRequestId;
                SET @IsAllLevelsCompleted = 1;
            END
            ELSE
            BEGIN
                UPDATE [dbo].[OfferApprovalRequest] SET CurrentLevelNo = @LevelNo + 1, UpdatedDate = GETDATE(), UpdatedBy = @ActionBy WHERE OfferApprovalRequestId = @OfferApprovalRequestId;
                SET @IsAllLevelsCompleted = 0;
            END
        END

        COMMIT TRANSACTION;
        SELECT 1 AS Success, 'Offer approval action recorded successfully!' AS ResponseMessage, @IsAllLevelsCompleted AS IsAllLevelsCompleted, @OfferApprovalRequestId AS OfferApprovalRequestId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage, CAST(0 AS BIT) AS IsAllLevelsCompleted, NULL AS OfferApprovalRequestId;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetOfferApprovalRequestByOfferId]
    @OfferId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 * FROM [dbo].[OfferApprovalRequest] WHERE OfferId = @OfferId AND IsDeleted = 0 ORDER BY CreatedDate DESC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetOfferApprovalRequestLevelsByRequestId]
    @OfferApprovalRequestId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.*, r.OfferId
    FROM [dbo].[OfferApprovalRequestLevel] l
    INNER JOIN [dbo].[OfferApprovalRequest] r ON r.OfferApprovalRequestId = l.OfferApprovalRequestId
    WHERE l.OfferApprovalRequestId = @OfferApprovalRequestId AND l.IsDeleted = 0
    ORDER BY l.LevelNo;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetPendingOfferApprovalsByApproverEmployeeId]
    @ApproverEmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.*, r.OfferId, o.OfferedCTC, o.JoiningDate, o.OfferStatus,
           c.FullName AS CandidateName, pos.PositionTitle
    FROM [dbo].[OfferApprovalRequestLevel] l
    INNER JOIN [dbo].[OfferApprovalRequest] r ON r.OfferApprovalRequestId = l.OfferApprovalRequestId AND r.CurrentLevelNo = l.LevelNo AND r.Status = 'Pending'
    INNER JOIN [dbo].[Offer] o ON o.OfferId = r.OfferId
    INNER JOIN [dbo].[CandidateApplication] ca ON ca.CandidateApplicationId = o.CandidateApplicationId
    INNER JOIN [dbo].[Candidate] c ON c.CandidateId = ca.CandidateId
    LEFT JOIN [dbo].[JobPositionMaster] pos ON pos.JobPositionId = o.JobPositionId
    WHERE l.IsDeleted = 0 AND l.Status = 'Pending' AND l.ApproverEmployeeId = @ApproverEmployeeId;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetOfferApprovalRequestHistoryByRequestId]
    @OfferApprovalRequestId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[OfferApprovalRequestHistory] WHERE OfferApprovalRequestId = @OfferApprovalRequestId ORDER BY ActionDate;
END
GO

-- Escalation sweep for the Hangfire job — mirrors the generic engine's escalation shape
-- but scoped to Offer's own tables; marks overdue pending levels as escalated and returns
-- them so the C# job can send the (already-generic) ApprovalEscalatedEmailTemplate.html.
CREATE OR ALTER PROCEDURE [dbo].[GetAndMarkOverdueOfferApprovalLevels]
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Now DATETIME = GETDATE();

    DECLARE @Overdue TABLE (OfferApprovalRequestLevelId INT);

    INSERT INTO @Overdue (OfferApprovalRequestLevelId)
    SELECT l.OfferApprovalRequestLevelId
    FROM [dbo].[OfferApprovalRequestLevel] l
    INNER JOIN [dbo].[OfferApprovalRequest] r ON r.OfferApprovalRequestId = l.OfferApprovalRequestId AND r.CurrentLevelNo = l.LevelNo AND r.Status = 'Pending'
    WHERE l.IsDeleted = 0 AND l.Status = 'Pending' AND l.EscalationDueOn IS NOT NULL AND l.EscalationDueOn < @Now AND l.EscalatedOn IS NULL;

    UPDATE l SET l.EscalatedOn = @Now
    FROM [dbo].[OfferApprovalRequestLevel] l
    INNER JOIN @Overdue o ON o.OfferApprovalRequestLevelId = l.OfferApprovalRequestLevelId;

    SELECT l.*, r.OfferId, c.FullName AS CandidateName, pos.PositionTitle, pos.CompanyId
    FROM [dbo].[OfferApprovalRequestLevel] l
    INNER JOIN @Overdue ov ON ov.OfferApprovalRequestLevelId = l.OfferApprovalRequestLevelId
    INNER JOIN [dbo].[OfferApprovalRequest] r ON r.OfferApprovalRequestId = l.OfferApprovalRequestId
    INNER JOIN [dbo].[Offer] o ON o.OfferId = r.OfferId
    INNER JOIN [dbo].[CandidateApplication] ca ON ca.CandidateApplicationId = o.CandidateApplicationId
    INNER JOIN [dbo].[Candidate] c ON c.CandidateId = ca.CandidateId
    LEFT JOIN [dbo].[JobPositionMaster] pos ON pos.JobPositionId = o.JobPositionId;
END
GO
