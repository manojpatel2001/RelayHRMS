-- ============================================================================
-- SP_AttendanceSelfServiceRequest
--
-- Backs the Employee self-service "Attendance Request" page
-- (AttendanceRequest.cshtml) — both the "Create Attendance" and "Update
-- Existing Attendance" radios. Called via AttendanceRegularizationRepository.
-- CreateSelfServiceRequest / UpdateSelfServiceRequest.
--
-- Deliberately a SEPARATE procedure from SP_AttendanceRegularization, which is
-- still used unchanged by the bulk Add Attendance tools (AddAttendance.cshtml,
-- AddAttendanceRegularization.cshtml) and the approval workflow
-- (UpdateAttendanceRegularization/UpdateHRDAttendanceRegularization). Those
-- callers rely on SP_AttendanceRegularization always deriving InTime/OutTime
-- from ShiftTime+Day on INSERT (ignoring any passed InTime/OutTime) — that
-- behavior must not change for them.
--
-- This procedure instead honors an explicit @InTime/@OutTime supplied by the
-- employee (falling back to the ShiftTime+Day calculation only when they
-- weren't provided), on BOTH INSERT and UPDATE, so a manually-entered
-- check-in/check-out actually determines the saved Duration. Fixes a bug
-- where an employee editing their check-out to reflect a longer day (e.g.
-- 11 hours) still had Duration saved as their standard shift length (9h),
-- because SP_AttendanceRegularization's INSERT branch silently discarded it.
--
-- Action = 'INSERT': creates a new Pending request. Fails if a request
--   already exists for that EmpId+ForDate (same guard as SP_AttendanceRegularization).
-- Action = 'UPDATE': edits an existing Pending/Rejected request's
--   InTime/OutTime/Duration/Day/ShiftTime/Reason/Remark, resets it to Pending,
--   and re-verifies ownership (@EmpId must match the record's EmpId). Refuses
--   to edit a request that has already been Approved.
--
-- Both actions reuse the CheckAttendanceLockAndSalary guard so a self-service
-- edit can't touch an attendance-locked or salary-generated period.
--
-- Does NOT sync AttendanceDetails/payroll — that only happens when a request
-- is actually approved, which stays exclusively owned by
-- SP_AttendanceRegularization's UPDATE(Approved) branch via the approval APIs.
-- ============================================================================
CREATE PROCEDURE [dbo].[SP_AttendanceSelfServiceRequest]
    @Action NVARCHAR(10),
    @Id INT = NULL,
    @EmpId INT = NULL,
    @FullName NVARCHAR(100) = NULL,
    @BranchName NVARCHAR(100) = NULL,
    @ForDate DATE = NULL,
    @ShiftTime NVARCHAR(50) = NULL,
    @InTime DATETIME = NULL,
    @OutTime DATETIME = NULL,
    @Day NVARCHAR(20) = NULL,
    @Reason NVARCHAR(500) = NULL,
    @Remark NVARCHAR(500) = NULL,
    @CreatedBy NVARCHAR(500) = NULL,
    @ResponseMessage NVARCHAR(MAX) OUTPUT,
    @Success BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Month INT = MONTH(@ForDate);
    DECLARE @Year INT = YEAR(@ForDate);

    SET @Success = 1;
    SET @ResponseMessage = 'Success';

    IF @EmpId IS NOT NULL
    BEGIN
        DECLARE @IsAttendanceLocked BIT, @IsSalaryGenerated BIT;

        SELECT
            @IsAttendanceLocked = IsAttendanceLocked,
            @IsSalaryGenerated = IsSalaryGenerated
        FROM dbo.CheckAttendanceLockAndSalary(@EmpId, @Month, @Year);

        IF @IsAttendanceLocked = 1 AND @IsSalaryGenerated = 1
        BEGIN
            SET @ResponseMessage = 'Attendance is locked and salary has been generated for this employee for the selected month/year.';
            SET @Success = 0;
            RETURN;
        END
        ELSE IF @IsAttendanceLocked = 1
        BEGIN
            SET @ResponseMessage = 'Attendance is locked for this employee for the selected month/year.';
            SET @Success = 0;
            RETURN;
        END
        ELSE IF @IsSalaryGenerated = 1
        BEGIN
            SET @ResponseMessage = 'Salary has been generated for this employee for the selected month/year.';
            SET @Success = 0;
            RETURN;
        END
    END

    -- Fallback InTime/OutTime derived from ShiftTime+Day, used only when the
    -- caller did not supply an explicit check-in/check-out.
    DECLARE @CalculatedInTime DATETIME = NULL;
    DECLARE @CalculatedOutTime DATETIME = NULL;

    IF @Day IS NOT NULL AND @ShiftTime IS NOT NULL
    BEGIN
        DECLARE @StartTime TIME, @EndTime TIME;
        DECLARE @CleanShift NVARCHAR(50) = REPLACE(REPLACE(@ShiftTime, ' to ', '-'), ' ', '');
        DECLARE @Pos INT = CHARINDEX('-', @CleanShift);

        IF @Pos > 0
        BEGIN
            SET @StartTime = TRY_CAST(LEFT(@CleanShift, @Pos - 1) AS TIME);
            SET @EndTime   = TRY_CAST(SUBSTRING(@CleanShift, @Pos + 1, LEN(@CleanShift)) AS TIME);

            IF @StartTime IS NOT NULL AND @EndTime IS NOT NULL
            BEGIN
                DECLARE @BaseDateTime  DATETIME = CAST(@ForDate AS DATETIME);
                DECLARE @StartDateTime DATETIME = DATEADD(MINUTE, DATEDIFF(MINUTE, '00:00', @StartTime), @BaseDateTime);
                DECLARE @EndDateTime   DATETIME = DATEADD(MINUTE, DATEDIFF(MINUTE, '00:00', @EndTime),   @BaseDateTime);

                IF @EndDateTime <= @StartDateTime
                    SET @EndDateTime = DATEADD(DAY, 1, @EndDateTime);

                DECLARE @TotalMinutes INT = DATEDIFF(MINUTE, @StartDateTime, @EndDateTime);

                IF LOWER(@Day) = 'full day'
                BEGIN
                    SET @CalculatedInTime  = @StartDateTime;
                    SET @CalculatedOutTime = @EndDateTime;
                END
                ELSE IF LOWER(@Day) = 'first half'
                BEGIN
                    SET @CalculatedInTime  = @StartDateTime;
                    SET @CalculatedOutTime = DATEADD(MINUTE, @TotalMinutes / 2, @StartDateTime);
                END
                ELSE IF LOWER(@Day) = 'second half'
                BEGIN
                    SET @CalculatedInTime  = DATEADD(MINUTE, @TotalMinutes / 2, @StartDateTime);
                    SET @CalculatedOutTime = @EndDateTime;
                END
            END
        END
    END

    -- =============================================
    -- INSERT
    -- =============================================
    IF @Action = 'INSERT'
    BEGIN
        IF EXISTS (
            SELECT 1 FROM AttendanceRegularization
            WHERE EmpId = @EmpId AND ForDate = @ForDate
            AND IsEnabled = 1 AND IsDeleted = 0
        )
        BEGIN
            SET @ResponseMessage = 'Attendance regularization request already exists for this employee on ' + CONVERT(VARCHAR(10), @ForDate, 105) + '.';
            SET @Success = 0;
            RETURN;
        END

        DECLARE @FinalInTime DATETIME = ISNULL(@InTime, @CalculatedInTime);
        DECLARE @FinalOutTime DATETIME = ISNULL(@OutTime, @CalculatedOutTime);

        IF @FinalInTime IS NULL OR @FinalOutTime IS NULL
        BEGIN
            SET @ResponseMessage = 'Check-in and check-out time could not be determined. Please enter them or ensure a shift is configured.';
            SET @Success = 0;
            RETURN;
        END

        IF @FinalOutTime <= @FinalInTime
            SET @FinalOutTime = DATEADD(DAY, 1, @FinalOutTime);

        DECLARE @FinalDuration DECIMAL(5,2) = CAST(DATEDIFF(MINUTE, @FinalInTime, @FinalOutTime) AS DECIMAL(5,2)) / 60.0;

        INSERT INTO AttendanceRegularization (
            EmpId, FullName, BranchName, ForDate, ShiftTime, InTime, OutTime,
            Duration, Status, Reason, Day, Remark, IsEnabled, IsDeleted,
            CreatedDate, CreatedBy, IsPending, IsApproved, IsRejected, IsBlocked, IsLocked
        )
        VALUES (
            @EmpId, @FullName, @BranchName, @ForDate, @ShiftTime,
            @FinalInTime, @FinalOutTime,
            @FinalDuration, 'Pending', @Reason, @Day, @Remark,
            1, 0, GETUTCDATE(), @CreatedBy,
            1, 0, 0, 0, 0
        );

        SET @ResponseMessage = 'Attendance regularization request saved successfully.';
        SET @Success = 1;
    END

    -- =============================================
    -- UPDATE (edit an existing self-submitted request's check-in/check-out)
    -- =============================================
    ELSE IF @Action = 'UPDATE'
    BEGIN
        DECLARE @ExistingEmpId INT, @ExistingStatus NVARCHAR(20), @ExistingInTime DATETIME, @ExistingOutTime DATETIME;

        SELECT
            @ExistingEmpId  = EmpId,
            @ExistingStatus = Status,
            @ExistingInTime = InTime,
            @ExistingOutTime = OutTime
        FROM AttendanceRegularization
        WHERE AttendanceRegularizationId = @Id AND IsEnabled = 1 AND IsDeleted = 0;

        IF @ExistingEmpId IS NULL
        BEGIN
            SET @ResponseMessage = 'Attendance regularization request not found.';
            SET @Success = 0;
            RETURN;
        END

        IF @ExistingEmpId <> @EmpId
        BEGIN
            SET @ResponseMessage = 'You are not authorized to update this request.';
            SET @Success = 0;
            RETURN;
        END

        IF @ExistingStatus = 'Approved'
        BEGIN
            SET @ResponseMessage = 'This request has already been approved and cannot be edited.';
            SET @Success = 0;
            RETURN;
        END

        DECLARE @FinalInTime2  DATETIME = ISNULL(@InTime,  ISNULL(@ExistingInTime,  @CalculatedInTime));
        DECLARE @FinalOutTime2 DATETIME = ISNULL(@OutTime, ISNULL(@ExistingOutTime, @CalculatedOutTime));

        IF @FinalInTime2 IS NULL OR @FinalOutTime2 IS NULL
        BEGIN
            SET @ResponseMessage = 'Check-in and check-out time could not be determined.';
            SET @Success = 0;
            RETURN;
        END

        IF @FinalOutTime2 <= @FinalInTime2
            SET @FinalOutTime2 = DATEADD(DAY, 1, @FinalOutTime2);

        DECLARE @FinalDuration2 DECIMAL(5,2) = CAST(DATEDIFF(MINUTE, @FinalInTime2, @FinalOutTime2) AS DECIMAL(5,2)) / 60.0;

        UPDATE AttendanceRegularization
        SET
            InTime      = @FinalInTime2,
            OutTime     = @FinalOutTime2,
            Duration    = @FinalDuration2,
            Day         = ISNULL(@Day, Day),
            ShiftTime   = ISNULL(@ShiftTime, ShiftTime),
            Reason      = ISNULL(@Reason, Reason),
            Remark      = ISNULL(@Remark, Remark),
            Status      = 'Pending',
            IsPending   = 1,
            IsApproved  = 0,
            IsRejected  = 0,
            UpdatedDate = GETUTCDATE(),
            UpdatedBy   = @CreatedBy
        WHERE AttendanceRegularizationId = @Id;

        SET @ResponseMessage = 'Attendance regularization request updated successfully.';
        SET @Success = 1;
    END
    ELSE
    BEGIN
        SET @ResponseMessage = 'Invalid action specified.';
        SET @Success = 0;
    END
END
GO
