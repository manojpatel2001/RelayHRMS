-- Combined INSERT/UPDATE/DELETE/GET procedure for NewsAnnouncement, called from
-- HRMS_Infrastructure\Repository\JobMaster\NewsAnnouncementRepository.cs via Dapper,
-- discriminated by @Operation. Run ../Migrations/2026-08-01_NewsAnnouncement_DisplaySettings.sql
-- first if the 4 display-settings columns don't exist yet.

CREATE OR ALTER PROCEDURE [dbo].[sp_NewsAnnouncement_CRUD]
    @Operation VARCHAR(10),
    @NewsID INT = NULL,
    @CmpID INT = NULL,
    @NewsTitle VARCHAR(50) = NULL,
    @NewsDescription VARCHAR(250) = NULL,
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL,
    @IsVisible BIT = 0,
    @IsThought BIT = 0,
    @IsPop BIT = 0,
    @IsLoginNotification BIT = 0,
    @BranchWiseNewsAnnoun VARCHAR(2000) = NULL,
    @IsEnabled BIT = 1,
    @IsDeleted BIT = 0,
    @PopupDurationSeconds INT = 20,
    @IsPopShowOnce BIT = 1,
    @LoginNotificationDurationSeconds INT = 20,
    @IsLoginNotificationShowOnce BIT = 1,
    @CreatedBy INT = NULL,
    @UpdatedBy INT = NULL,
    @DeletedBy INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Operation IN ('INSERT', 'CREATE')
        BEGIN
            INSERT INTO [dbo].[NewsAnnouncement]
            (
                CmpID, NewsTitle, NewsDescription, StartDate, EndDate,
                IsVisible, IsThought, IsPop, IsLoginNotification, BranchWiseNewsAnnoun,
                IsEnabled, IsDeleted,
                PopupDurationSeconds, IsPopShowOnce, LoginNotificationDurationSeconds, IsLoginNotificationShowOnce,
                CreatedDate, CreatedBy
            )
            VALUES
            (
                @CmpID, @NewsTitle, @NewsDescription, @StartDate, @EndDate,
                @IsVisible, @IsThought, @IsPop, @IsLoginNotification, @BranchWiseNewsAnnoun,
                @IsEnabled, @IsDeleted,
                ISNULL(@PopupDurationSeconds, 20), ISNULL(@IsPopShowOnce, 1), ISNULL(@LoginNotificationDurationSeconds, 20), ISNULL(@IsLoginNotificationShowOnce, 1),
                GETDATE(), @CreatedBy
            );

            SELECT 1 AS Success, 'News announcement created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[NewsAnnouncement]
            SET
                CmpID = ISNULL(@CmpID, CmpID),
                NewsTitle = ISNULL(@NewsTitle, NewsTitle),
                NewsDescription = ISNULL(@NewsDescription, NewsDescription),
                StartDate = ISNULL(@StartDate, StartDate),
                EndDate = ISNULL(@EndDate, EndDate),
                IsVisible = ISNULL(@IsVisible, IsVisible),
                IsThought = ISNULL(@IsThought, IsThought),
                IsPop = ISNULL(@IsPop, IsPop),
                IsLoginNotification = ISNULL(@IsLoginNotification, IsLoginNotification),
                BranchWiseNewsAnnoun = ISNULL(@BranchWiseNewsAnnoun, BranchWiseNewsAnnoun),
                IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                PopupDurationSeconds = ISNULL(@PopupDurationSeconds, PopupDurationSeconds),
                IsPopShowOnce = ISNULL(@IsPopShowOnce, IsPopShowOnce),
                LoginNotificationDurationSeconds = ISNULL(@LoginNotificationDurationSeconds, LoginNotificationDurationSeconds),
                IsLoginNotificationShowOnce = ISNULL(@IsLoginNotificationShowOnce, IsLoginNotificationShowOnce),
                UpdatedDate = GETDATE(),
                UpdatedBy = @UpdatedBy
            WHERE NewsID = @NewsID;

            SELECT 1 AS Success, 'News announcement updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[NewsAnnouncement]
            SET
                IsDeleted = 1,
                IsEnabled = 0,
                DeletedDate = GETDATE(),
                DeletedBy = @DeletedBy
            WHERE NewsID = @NewsID;

            SELECT 1 AS Success, 'News announcement deleted successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'GET'
        BEGIN
            SELECT *
            FROM [dbo].[NewsAnnouncement]
            WHERE
                IsEnabled = 1
                AND IsDeleted = 0
                AND (@NewsID IS NULL OR NewsID = @NewsID);

            SELECT 1 AS Success, 'Record fetched successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, 'Something went wrong!' AS ResponseMessage;
    END CATCH
END
