-- Combined INSERT/UPDATE/DELETE/GET procedure for EmployeeWarningHistory (Disciplinary
-- Actions), called from HRMS_Infrastructure\Repository\Employee\EmployeeWarningHistoryRepository.cs
-- via Dapper, discriminated by @Operation. WarningMasterId is a FK to the EXISTING
-- WarningMaster catalog table (warning type/level lookup) — this table is only the
-- missing per-employee, per-incident transaction log. Mirrors sp_NewsAnnouncement_CRUD.sql.

CREATE OR ALTER PROCEDURE [dbo].[sp_EmployeeWarningHistory_CRUD]
    @Operation VARCHAR(10),
    @EmployeeWarningHistoryId INT = NULL,
    @EmployeeId INT = NULL,
    @WarningMasterId INT = NULL,
    @IssueDate DATE = NULL,
    @IssuedBy VARCHAR(200) = NULL,
    @IncidentDescription VARCHAR(1000) = NULL,
    @ActionTaken VARCHAR(500) = NULL,
    @Status VARCHAR(20) = NULL,
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
            INSERT INTO [dbo].[EmployeeWarningHistory]
            (EmployeeId, WarningMasterId, IssueDate, IssuedBy, IncidentDescription, ActionTaken, Status, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@EmployeeId, @WarningMasterId, @IssueDate, @IssuedBy, @IncidentDescription, @ActionTaken, ISNULL(@Status, 'Issued'), @IsEnabled, GETDATE(), @CreatedBy);

            SELECT 1 AS Success, 'Warning record created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[EmployeeWarningHistory]
            SET
                WarningMasterId = ISNULL(@WarningMasterId, WarningMasterId),
                IssueDate = ISNULL(@IssueDate, IssueDate),
                IssuedBy = ISNULL(@IssuedBy, IssuedBy),
                IncidentDescription = ISNULL(@IncidentDescription, IncidentDescription),
                ActionTaken = ISNULL(@ActionTaken, ActionTaken),
                Status = ISNULL(@Status, Status),
                IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(),
                UpdatedBy = @UpdatedBy
            WHERE EmployeeWarningHistoryId = @EmployeeWarningHistoryId;

            SELECT 1 AS Success, 'Warning record updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[EmployeeWarningHistory]
            SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy
            WHERE EmployeeWarningHistoryId = @EmployeeWarningHistoryId;

            SELECT 1 AS Success, 'Warning record deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, 'Something went wrong!' AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetEmployeeWarningHistoryByEmployeeId]
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT wh.*, wm.WarningName, wm.Level
    FROM [dbo].[EmployeeWarningHistory] wh
    LEFT JOIN [dbo].[WarningMaster] wm ON wm.WarningMasterId = wh.WarningMasterId
    WHERE wh.EmployeeId = @EmployeeId AND wh.IsDeleted = 0
    ORDER BY wh.IssueDate DESC;
END
