-- ============================================================================
-- GetEmployeeTodayInOutStatus
--
-- Backs the ESS "My In Out" Current Status card (Clock In / Clock Out buttons,
-- Last Clock In, Last Clock Out, Today's Worked Hours).
--
-- Additive/read-only: does not alter AttendanceLog, AttendanceDetails, or the
-- TR_CalculateAttendanceSummary trigger that drives payroll AttendanceStatus/
-- SalaryDay. Reads AttendanceLog directly instead, because that existing
-- trigger only tracks the day's very FIRST and LAST punch (MIN/MAX) - it
-- silently ignores any punches in between, so it can't tell "clocked in" from
-- "clocked out" once an employee has punched more than twice in a day.
--
-- This proc instead treats today's punches as an alternating sequence
-- (1st = in, 2nd = out, 3rd = in, ...), which is what a single "record a
-- punch now" button naturally produces:
--   - IsClockedIn   = true when today's punch count is odd (an "in" with no
--                     matching "out" yet).
--   - LastClockIn   = the most recent "in" punch (odd position), whether or
--                     not it has a matching "out" yet.
--   - LastClockOut  = the most recent "out" punch (even position).
--   - WorkedSeconds = sum of completed in/out pairs, plus the still-open
--                     segment (now - last clock-in) when currently clocked in
--                     so the figure keeps ticking on every poll.
-- ============================================================================
CREATE PROCEDURE [dbo].[GetEmployeeTodayInOutStatus]
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH TodayPunches AS (
        SELECT
            PunchDateTime,
            ROW_NUMBER() OVER (ORDER BY PunchDateTime ASC) AS Rn
        FROM AttendanceLog
        WHERE EmployeeId = @EmployeeId
          AND CAST(PunchDateTime AS DATE) = CAST(GETDATE() AS DATE)
    ),
    Pairs AS (
        SELECT
            i.PunchDateTime AS ClockIn,
            o.PunchDateTime AS ClockOut
        FROM TodayPunches i
        LEFT JOIN TodayPunches o ON o.Rn = i.Rn + 1 AND o.Rn % 2 = 0
        WHERE i.Rn % 2 = 1
    )
    SELECT
        ISNULL((SELECT COUNT(*) FROM TodayPunches), 0) AS PunchCount,
        (SELECT MAX(ClockIn) FROM Pairs) AS LastClockIn,
        (SELECT MAX(ClockOut) FROM Pairs) AS LastClockOut,
        CASE WHEN ISNULL((SELECT COUNT(*) FROM TodayPunches), 0) % 2 = 1
             THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsClockedIn,
        ISNULL((SELECT SUM(DATEDIFF(SECOND, ClockIn, ClockOut)) FROM Pairs WHERE ClockOut IS NOT NULL), 0)
        +
        CASE WHEN ISNULL((SELECT COUNT(*) FROM TodayPunches), 0) % 2 = 1
             THEN DATEDIFF(SECOND, (SELECT ClockIn FROM Pairs WHERE ClockOut IS NULL), GETDATE())
             ELSE 0 END AS WorkedSeconds;
END
GO
