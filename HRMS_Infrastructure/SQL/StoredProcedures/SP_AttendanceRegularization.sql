-- ============================================================================
-- SP_AttendanceRegularization
--
-- Backs the bulk Add Attendance tools (AddAttendance.cshtml, employee panel;
-- AddAttendanceRegularization.cshtml, admin panel) and the approval workflow
-- (UpdateAttendanceRegularization / UpdateHRDAttendanceRegularization, used by
-- AttendanceRegularizationApproval.cshtml, AttendanceRegularization.cshtml,
-- AttendanceRequestAdminReport.cshtml). On INSERT, InTime/OutTime are always
-- derived from ShiftTime+Day (any passed InTime/OutTime is ignored) — that is
-- intentional for these bulk/admin callers and must not change here. See
-- SP_AttendanceSelfServiceRequest.sql for the employee self-service page,
-- which needs the opposite behavior (explicit InTime/OutTime honored) and so
-- got its own dedicated procedure instead of changing this one.
--
-- BUGFIX (2026-08-04): On Action='UPDATE' (i.e. approve/reject), Duration was
-- being seeded from the ShiftTime+Day calculation (the employee's standard
-- shift length) instead of being recomputed from the actual InTime/OutTime
-- being saved. That meant approving a request whose real hours differed from
-- the standard shift (e.g. an 11-hour day) silently saved the standard shift's
-- duration (9h) instead, even though InTime/OutTime themselves were correct.
-- Fixed so @FinalDuration is always derived from @FinalInTime/@FinalOutTime.
-- ============================================================================
CREATE PROCEDURE [dbo].[SP_AttendanceRegularization]
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
    @Status NVARCHAR(20) = NULL,
    @CreatedBy NVARCHAR(500) = NULL,
    @ResponseMessage NVARCHAR(MAX) OUTPUT,
    @Success BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CurrentDate DATE = CAST(GETDATE() AS DATE);
    DECLARE @Month INT = MONTH(@ForDate);
    DECLARE @Year INT = YEAR(@ForDate);

    SET @Success = 1;
    SET @ResponseMessage = 'Success';

    -- =============================================
    -- ATTENDANCE LOCK & SALARY VALIDATION
    -- =============================================
    IF @EmpId IS NOT NULL
    BEGIN
        DECLARE @IsAttendanceLocked BIT;
        DECLARE @IsSalaryGenerated BIT;

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
    ELSE
    BEGIN
        IF EXISTS (
            SELECT 1
            FROM AspNetUsers U
            CROSS APPLY dbo.CheckAttendanceLockAndSalary(U.Id, @Month, @Year) C
            WHERE U.IsDeleted = 0 AND U.IsEnabled = 1 AND U.IsBlocked = 0
            AND (C.IsAttendanceLocked = 1 OR C.IsSalaryGenerated = 1)
        )
        BEGIN
            DECLARE @LockedCount INT, @SalaryCount INT;

            SELECT
                @LockedCount = SUM(CASE WHEN C.IsAttendanceLocked = 1 THEN 1 ELSE 0 END),
                @SalaryCount = SUM(CASE WHEN C.IsSalaryGenerated = 1 THEN 1 ELSE 0 END)
            FROM AspNetUsers U
            CROSS APPLY dbo.CheckAttendanceLockAndSalary(U.Id, @Month, @Year) C
            WHERE U.IsDeleted = 0 AND U.IsEnabled = 1 AND U.IsBlocked = 0;

            SET @ResponseMessage =
                'Warning: ' +
                CASE
                    WHEN @LockedCount > 0 AND @SalaryCount > 0
                    THEN CAST(@LockedCount AS VARCHAR(10)) + ' employee(s) have locked attendance and ' +
                         CAST(@SalaryCount AS VARCHAR(10)) + ' employee(s) have generated salary for the selected period.'
                    WHEN @LockedCount > 0
                    THEN CAST(@LockedCount AS VARCHAR(10)) + ' employee(s) have locked attendance for the selected period.'
                    ELSE CAST(@SalaryCount AS VARCHAR(10)) + ' employee(s) have generated salary for the selected period.'
                END;
            SET @Success = 0;
            RETURN;
        END
    END

    -- =============================================
    -- CALCULATE InTime / OutTime FROM ShiftTime+Day
    -- =============================================
    DECLARE @CalculatedInTime  DATETIME = NULL;
    DECLARE @CalculatedOutTime DATETIME = NULL;
    DECLARE @Duration DECIMAL(5,2) = NULL;

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

                IF @CalculatedInTime IS NOT NULL AND @CalculatedOutTime IS NOT NULL
                BEGIN
                    IF @CalculatedOutTime <= @CalculatedInTime
                        SET @CalculatedOutTime = DATEADD(DAY, 1, @CalculatedOutTime);
                    SET @Duration = CAST(DATEDIFF(MINUTE, @CalculatedInTime, @CalculatedOutTime) AS DECIMAL(5,2)) / 60.0;
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

        INSERT INTO AttendanceRegularization (
            EmpId, FullName, BranchName, ForDate, ShiftTime, InTime, OutTime,
            Duration, Status, Reason, Day, Remark, IsEnabled, IsDeleted,
            CreatedDate, CreatedBy, IsPending, IsApproved, IsRejected, IsBlocked, IsLocked
        )
        VALUES (
            @EmpId, @FullName, @BranchName, @ForDate, @ShiftTime,
            @CalculatedInTime, @CalculatedOutTime,
            @Duration, 'Pending', @Reason, @Day, @Remark,
            1, 0, GETUTCDATE(), @CreatedBy,
            1, 0, 0, 0, 0
        );

        SET @ResponseMessage = 'Attendance regularization request saved successfully.';
        SET @Success = 1;
    END

    -- =============================================
    -- UPDATE
    -- =============================================
    ELSE IF @Action = 'UPDATE'
    BEGIN
        DECLARE @FinalInTime   DATETIME     = ISNULL(@InTime,  @CalculatedInTime);
        DECLARE @FinalOutTime  DATETIME     = ISNULL(@OutTime, @CalculatedOutTime);
        DECLARE @FinalDuration DECIMAL(5,2) = NULL;

        -- Agar abhi bhi null hai to existing table values se lo
        IF @FinalInTime IS NULL OR @FinalOutTime IS NULL
        BEGIN
            SELECT
                @FinalInTime  = ISNULL(@FinalInTime,  InTime),
                @FinalOutTime = ISNULL(@FinalOutTime, OutTime)
            FROM AttendanceRegularization
            WHERE AttendanceRegularizationId = @Id;
        END

        -- BUGFIX: Duration must always reflect the FINAL InTime/OutTime being saved,
        -- not the employee's standard shift length. Previously this was seeded from
        -- the ShiftTime+Day calculation above (@Duration), so approving a request
        -- whose actual InTime/OutTime differ from the standard shift (e.g. a longer
        -- day) silently overwrote the correct Duration with the shift's standard
        -- hours. Recompute directly from @FinalInTime/@FinalOutTime instead.
        IF @FinalInTime IS NOT NULL AND @FinalOutTime IS NOT NULL
        BEGIN
            IF @FinalOutTime <= @FinalInTime
                SET @FinalOutTime = DATEADD(DAY, 1, @FinalOutTime);
            SET @FinalDuration = CAST(DATEDIFF(MINUTE, @FinalInTime, @FinalOutTime) AS DECIMAL(5,2)) / 60.0;
        END

        DECLARE @IsPending  BIT = 0;
        DECLARE @IsApproved BIT = 0;
        DECLARE @IsRejected BIT = 0;

        IF @Status = 'Pending'       SET @IsPending  = 1;
        ELSE IF @Status = 'Approved' SET @IsApproved = 1;
        ELSE IF @Status = 'Rejected' SET @IsRejected = 1;

        -- Approved hai but InTime/OutTime/Duration nahi �?" pehle hi rok do
        IF @IsApproved = 1 AND (@FinalInTime IS NULL OR @FinalOutTime IS NULL OR @FinalDuration IS NULL)
        BEGIN
            SET @ResponseMessage = 'ERROR: Cannot approve - InTime, OutTime or Duration could not be determined for Employee ' +
                                   CAST(@EmpId AS VARCHAR) + ' on ' + CONVERT(VARCHAR(10), @ForDate, 105) +
                                   '. Please check ShiftTime and Day values.';
            SET @Success = 0;
            RETURN;
        END

        -- =============================================
        -- TRANSACTION �?" dono tables ek saath
        -- =============================================
        BEGIN TRY
            BEGIN TRANSACTION;

            -- STEP 1: AttendanceRegularization update
            UPDATE AttendanceRegularization
            SET
                InTime      = ISNULL(@FinalInTime,   InTime),
                OutTime     = ISNULL(@FinalOutTime,  OutTime),
                Duration    = ISNULL(@FinalDuration, Duration),
                Status      = ISNULL(@Status, Status),
                Reason      = ISNULL(@Reason, Reason),
                Day         = ISNULL(@Day,    Day),
                IsPending   = @IsPending,
                IsApproved  = @IsApproved,
                IsRejected  = @IsRejected,
                UpdatedDate = GETUTCDATE(),
                UpdatedBy   = @CreatedBy
            WHERE AttendanceRegularizationId = @Id;

            IF @@ROWCOUNT = 0
            BEGIN
                ROLLBACK TRANSACTION;
                SET @ResponseMessage = 'ERROR: AttendanceRegularization record not found for Id ' + CAST(@Id AS VARCHAR);
                SET @Success = 0;
                RETURN;
            END

            -- =============================================
            -- STEP 2: Approved hai to AttendanceDetails sync
            -- =============================================
            IF @IsApproved = 1
            BEGIN
                DECLARE @FinalWorkingHours DECIMAL(5,2);
                DECLARE @AttendanceStatus  NVARCHAR(5)  = 'A';
                DECLARE @FinalSalaryDay    DECIMAL(5,2) = 0.00;

                IF EXISTS (
                    SELECT 1 FROM AttendanceDetails
                    WHERE EmployeeId = @EmpId
                    AND CAST(ShiftDate AS DATE) = CAST(@ForDate AS DATE)
                )
                BEGIN
                    -- Existing record details fetch karo
                    DECLARE @ExistingWorkingHours DECIMAL(5,2) = 0;
                    DECLARE @ExistingInTime       DATETIME     = NULL;
                    DECLARE @ExistingOutTime      DATETIME     = NULL;

                    SELECT
                        @ExistingWorkingHours = ISNULL(WorkingHours, 0),
                        @ExistingInTime       = InTime,
                        @ExistingOutTime      = OutTime
                    FROM AttendanceDetails
                    WHERE EmployeeId = @EmpId
                    AND CAST(ShiftDate AS DATE) = CAST(@ForDate AS DATE);

                    -- -----------------------------------------------
                    -- Day type ke hisab se logic
                    -- -----------------------------------------------
                    IF LOWER(@Day) = 'full day'
                    BEGIN
                        SET @FinalWorkingHours = @FinalDuration;
                        -- @FinalInTime / @FinalOutTime already shift se set hain
                    END
                    ELSE IF LOWER(@Day) IN ('first half', 'second half')
                    BEGIN
                        DECLARE @AlreadyApprovedSameHalf INT = 0;

                        SELECT @AlreadyApprovedSameHalf = COUNT(1)
                        FROM AttendanceRegularization
                        WHERE EmpId    = @EmpId
                        AND ForDate    = @ForDate
                        AND IsApproved = 1
                        AND IsEnabled  = 1
                        AND IsDeleted  = 0
                        AND LOWER(Day) = LOWER(@Day)
                        AND AttendanceRegularizationId <> @Id;

                        IF @AlreadyApprovedSameHalf > 0
                        BEGIN
                            -- Same Half dobara �?" Full Day treat karo
                            DECLARE @FullDayInTime  DATETIME = NULL;
                            DECLARE @FullDayOutTime DATETIME = NULL;
                            DECLARE @FullStartTime  TIME,
                                    @FullEndTime    TIME;
                            DECLARE @FullCleanShift NVARCHAR(50) = REPLACE(REPLACE(@ShiftTime, ' to ', '-'), ' ', '');
                            DECLARE @FullPos        INT = CHARINDEX('-', @FullCleanShift);

                            IF @FullPos > 0
                            BEGIN
                                SET @FullStartTime = TRY_CAST(LEFT(@FullCleanShift, @FullPos - 1) AS TIME);
                                SET @FullEndTime   = TRY_CAST(SUBSTRING(@FullCleanShift, @FullPos + 1, LEN(@FullCleanShift)) AS TIME);

                                IF @FullStartTime IS NOT NULL AND @FullEndTime IS NOT NULL
                                BEGIN
                                    DECLARE @FullBase DATETIME = CAST(@ForDate AS DATETIME);
                                    SET @FullDayInTime  = DATEADD(MINUTE, DATEDIFF(MINUTE, '00:00', @FullStartTime), @FullBase);
                                    SET @FullDayOutTime = DATEADD(MINUTE, DATEDIFF(MINUTE, '00:00', @FullEndTime),   @FullBase);

                                    IF @FullDayOutTime <= @FullDayInTime
                                        SET @FullDayOutTime = DATEADD(DAY, 1, @FullDayOutTime);
                                END
                            END

                            SET @FinalInTime       = ISNULL(@FullDayInTime,  @FinalInTime);
                            SET @FinalOutTime      = ISNULL(@FullDayOutTime, @FinalOutTime);
                            SET @FinalWorkingHours = CAST(DATEDIFF(MINUTE, @FinalInTime, @FinalOutTime) AS DECIMAL(5,2)) / 60.0;
                        END
                        ELSE
                        BEGIN
                            -- Different half �?" ADD karo
                            SET @FinalWorkingHours = @ExistingWorkingHours + @FinalDuration;

                            IF LOWER(@Day) = 'first half'
                            BEGIN
                                SET @FinalInTime  = CASE WHEN @FinalInTime < ISNULL(@ExistingInTime, @FinalInTime)
                                                         THEN @FinalInTime ELSE @ExistingInTime END;
                                SET @FinalOutTime = CASE WHEN ISNULL(@ExistingOutTime, @FinalOutTime) > @FinalOutTime
                                                         THEN @ExistingOutTime ELSE @FinalOutTime END;
                            END
                            ELSE IF LOWER(@Day) = 'second half'
                            BEGIN
                                SET @FinalInTime  = CASE WHEN ISNULL(@ExistingInTime, @FinalInTime) < @FinalInTime
                                                         THEN @ExistingInTime ELSE @FinalInTime END;
                                SET @FinalOutTime = CASE WHEN @FinalOutTime > ISNULL(@ExistingOutTime, @FinalOutTime)
                                                         THEN @FinalOutTime ELSE @ExistingOutTime END;
                            END
                        END
                    END

                    -- NULL guard �?" kisi bhi case mein NULL nahi rehna chahiye
                    IF @FinalWorkingHours IS NULL
                        SET @FinalWorkingHours = @FinalDuration;

                    -- AttendanceStatus & SalaryDay calculate karo
                    IF @FinalWorkingHours >= 9
                    BEGIN
                        SET @AttendanceStatus = 'P';
                        SET @FinalSalaryDay   = 1.00;
                    END
                    ELSE IF @FinalWorkingHours >= 4.5
                    BEGIN
                        SET @AttendanceStatus = 'HF';
                        SET @FinalSalaryDay   = 0.50;
                    END
                    ELSE
                    BEGIN
                        SET @AttendanceStatus = 'A';
                        SET @FinalSalaryDay   = 0.00;
                    END

                    -- Existing record update
                    UPDATE AttendanceDetails
                    SET
                        InTime           = @FinalInTime,
                        OutTime          = @FinalOutTime,
                        WorkingHours     = @FinalWorkingHours,
                        AttendanceStatus = @AttendanceStatus,
                        SalaryDay        = @FinalSalaryDay
                    WHERE
                        EmployeeId = @EmpId
                        AND CAST(ShiftDate AS DATE) = CAST(@ForDate AS DATE);

                    IF @@ROWCOUNT = 0
                    BEGIN
                        ROLLBACK TRANSACTION;
                        SET @ResponseMessage = 'ERROR: AttendanceDetails update failed for Employee ' +
                                               CAST(@EmpId AS VARCHAR) + ' on ' + CONVERT(VARCHAR(10), @ForDate, 105) +
                                               '. Approval rolled back.';
                        SET @Success = 0;
                        RETURN;
                    END

                    SET @ResponseMessage = 'Attendance regularization approved and attendance record updated successfully.';
                END
                ELSE
                BEGIN
                    -- Naya record �?" fresh insert
                    SET @FinalWorkingHours = @FinalDuration;

                    IF @FinalWorkingHours >= 9
                    BEGIN
                        SET @AttendanceStatus = 'P';
                        SET @FinalSalaryDay   = 1.00;
                    END
                    ELSE IF @FinalWorkingHours >= 4.5
                    BEGIN
                        SET @AttendanceStatus = 'HF';
                        SET @FinalSalaryDay   = 0.50;
                    END
                    ELSE
                    BEGIN
                        SET @AttendanceStatus = 'A';
                        SET @FinalSalaryDay   = 0.00;
                    END

                    INSERT INTO AttendanceDetails (
                        EmployeeId, ShiftDate, InTime, OutTime,
                        WorkingHours, AttendanceStatus, SalaryDay
                    )
                    VALUES (
                        @EmpId, @ForDate, @FinalInTime, @FinalOutTime,
                        @FinalWorkingHours, @AttendanceStatus, @FinalSalaryDay
                    );

                    IF @@ROWCOUNT = 0
                    BEGIN
                        ROLLBACK TRANSACTION;
                        SET @ResponseMessage = 'ERROR: AttendanceDetails insert failed for Employee ' +
                                               CAST(@EmpId AS VARCHAR) + ' on ' + CONVERT(VARCHAR(10), @ForDate, 105) +
                                               '. Approval rolled back.';
                        SET @Success = 0;
                        RETURN;
                    END

                    SET @ResponseMessage = 'Attendance regularization approved and attendance record created successfully.';
                END
            END
            ELSE
            BEGIN
                SET @ResponseMessage = 'Attendance regularization request updated successfully.';
            END

            COMMIT TRANSACTION;
            SET @Success = 1;

        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0
                ROLLBACK TRANSACTION;

            SET @ResponseMessage = 'ERROR: Transaction failed - ' + ERROR_MESSAGE() +
                                   ' | Employee: ' + CAST(@EmpId AS VARCHAR) +
                                   ' | Date: ' + CONVERT(VARCHAR(10), @ForDate, 105);
            SET @Success = 0;
            RETURN;
        END CATCH
    END

    -- =============================================
    -- DELETE
    -- =============================================
    ELSE IF @Action = 'DELETE'
    BEGIN
        UPDATE AttendanceRegularization
        SET
            IsDeleted   = 1,
            IsEnabled   = 0,
            DeletedDate = GETUTCDATE(),
            DeletedBy   = @CreatedBy
        WHERE AttendanceRegularizationId = @Id;

        SET @ResponseMessage = 'Attendance regularization request deleted successfully.';
        SET @Success = 1;
    END
END
GO
