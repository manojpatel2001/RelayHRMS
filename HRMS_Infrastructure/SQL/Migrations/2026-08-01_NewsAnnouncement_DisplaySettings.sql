-- Adds per-announcement popup/login-notification display settings to NewsAnnouncement:
--   PopupDurationSeconds / LoginNotificationDurationSeconds: auto-dismiss interval (seconds),
--     configurable per announcement instead of a hardcoded 20s in the ESS dashboard JS.
--   IsPopShowOnce / IsLoginNotificationShowOnce: whether the popup/toast shows once ever
--     (sessionStorage keyed by NewsID) or once per fresh login (sessionStorage keyed by
--     NewsID + the login's LoginHistoryID, from the JWT).
-- Paired with the sp_NewsAnnouncement_CRUD update in ../StoredProcedures.

ALTER TABLE [dbo].[NewsAnnouncement] ADD
    PopupDurationSeconds INT NULL CONSTRAINT DF_NewsAnnouncement_PopupDurationSeconds DEFAULT 20,
    IsPopShowOnce BIT NOT NULL CONSTRAINT DF_NewsAnnouncement_IsPopShowOnce DEFAULT 1,
    LoginNotificationDurationSeconds INT NULL CONSTRAINT DF_NewsAnnouncement_LoginNotifDurationSeconds DEFAULT 20,
    IsLoginNotificationShowOnce BIT NOT NULL CONSTRAINT DF_NewsAnnouncement_IsLoginNotifShowOnce DEFAULT 1;
