-- sqlcmd's default session has QUOTED_IDENTIFIER OFF, which gets baked into
-- these procedures' compiled metadata at ALTER time — and AspNetUsers has 5
-- filtered indexes that require QUOTED_IDENTIFIER ON for any INSERT/UPDATE
-- against it, so without this, every employee Create/Update fails with
-- "INSERT failed because the following SET options have incorrect
-- settings: 'QUOTED_IDENTIFIER'." This must stay the first statement run
-- in this session, before any of the ALTER PROCEDUREs below.
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- ============================================================================
-- MUST BE APPLIED BEFORE deploying the Employee Master Mobile Access / Is Selfi
-- Required / Geo Fencing toggle feature (HRMS_Infrastructure/Repository/
-- EmployeeMaster/EmployeeManageRepository.cs now passes @IsMobileAccess,
-- @IsSelfiRequired, @IsGeofencingRequired to both procedures below).
--
-- Pulled directly from the live HRMS_2105 database (164.52.206.29,51432) via
-- OBJECT_DEFINITION() on 2026-08-25 and diffed by hand — every line below is
-- either the verbatim existing body or a clearly-marked addition. No other
-- behavior is changed.
-- ============================================================================


-- ============================================================================
-- 1) usp_ManageEmployee
--    - @IsMobileAccess BIT and @IsSelfiRequired BIT already existed as
--      parameters, but @IsSelfiRequired was never actually used anywhere in
--      the body, and @IsMobileAccess was only wired into the CREATE branch's
--      INSERT, never into UPDATE_BASIC's SET.
--    - @IsGeofencingRequired did not exist at all — without this change,
--      every Create/Update Employee call from the new C# code fails
--      immediately with "@IsGeofencingRequired is not a parameter for
--      procedure usp_ManageEmployee."
--    Changes: added @IsGeofencingRequired parameter; added IsSelfiRequired +
--    IsGeofencingRequired to the CREATE INSERT/VALUES lists; added
--    IsMobileAccess, IsSelfiRequired, IsGeofencingRequired to UPDATE_BASIC's
--    SET clause.
-- ============================================================================

ALTER PROCEDURE [dbo].[usp_ManageEmployee]
    @Action NVARCHAR(20), -- 'CREATE', 'UPDATE_BASIC', 'UPDATE_PERSONAL', 'UPDATE_CONTACT', 'UPDATE_SALARY', 'DELETE', 'IsBlocked'
    @Id INT = NULL,
    -- Basic Info (for CREATE and UPDATE_BASIC)
    @Initial NVARCHAR(50) = NULL,
    @FirstName NVARCHAR(100) = NULL,
    @MiddleName NVARCHAR(100) = NULL,
    @LastName NVARCHAR(100) = NULL,
    @FullName NVARCHAR(MAX) = NULL,
    @EmployeeCode NVARCHAR(50) = NULL,
    @AlfaCode NVARCHAR(MAX) = NULL,
    @AlfaEmployeeCode NVARCHAR(MAX) = NULL,
    @DateOfJoining DATETIME = NULL,
    @BranchId INT = NULL,
    @GradeId INT = NULL,
    @ShiftMasterId NVARCHAR(100) = NULL,
    @CTC NVARCHAR(100) = NULL,
    @DesignationId INT = NULL,
    @GrossSalary DECIMAL(18,2) = NULL,
    @CategoryId NVARCHAR(100) = NULL,
    @BasicSalary DECIMAL(18,2) = NULL,
    @DepartmentId INT = NULL,
    @EmployeeTypeId NVARCHAR(100) = NULL,
    @DateOfBirth DATETIME = NULL,
    @LoginAlias NVARCHAR(100) = NULL,
    @Password NVARCHAR(100) = NULL,
    @ReportingManagerId INT = NULL,
    @SubBranch NVARCHAR(100) = NULL,
    @EnrollNo NVARCHAR(100) = NULL,
    @CompanyId INT = NULL,
    @Overtime BIT = 0,
    @Latemark BIT = 0,
    @Earlymark BIT = 0,
    @Fullpf BIT = 0,
    @Pt BIT = 0,
    @Fixsalary BIT = 0,
    @Probation BIT = 0,
    @Trainee BIT = 0,
    @EmployeeProfileUrl NVARCHAR(255) = NULL,
    @EmployeeSignatureUrl NVARCHAR(255) = NULL,
    @IsPFApplicable BIT = 1,
    @WeekOffDetailsId INT = NULL,
    @IsPermissionPunchInOut BIT = 0,
    @IsLeft BIT = 0,
    -- Personal Info (for UPDATE_PERSONAL)
    @Gender NVARCHAR(20) = NULL,
    @PersonalEmailId NVARCHAR(100) = NULL,
    @FatherName NVARCHAR(100) = NULL,
    @MotherName NVARCHAR(100) = NULL,
    @BloodGroup NVARCHAR(10) = NULL,
    @Height NVARCHAR(50) = NULL,
    @MaritalStatus NVARCHAR(50) = NULL,
    @MarriageDate DATETIME = NULL,
    @MarkIdentification NVARCHAR(255) = NULL,
    @Religion NVARCHAR(100) = NULL,
    @Caste NVARCHAR(100) = NULL,
    @CastCategory NVARCHAR(100) = NULL,
    @AadharCardNo NVARCHAR(20) = NULL,
    @PANNo NVARCHAR(20) = NULL,
    @Dispensary NVARCHAR(255) = NULL,
    @DoctorName NVARCHAR(100) = NULL,
    @DispensaryAddress NVARCHAR(255) = NULL,
    @UANNumber NVARCHAR(20) = NULL,
    @DrivingLicense NVARCHAR(50) = NULL,
    @DrivingLicenseExpiry DATETIME = NULL,
    @RationCardType NVARCHAR(50) = NULL,
    @RationCardNo NVARCHAR(50) = NULL,
    @ProbationCompletionPeriod DECIMAL = NULL,
    @ProbationPeriodType NVARCHAR(50) = NULL,
    @ManagerProbationId INT = NULL,
    @ConfirmDate DATETIME = NULL,
    @RetirementDate DATETIME = NULL,
    @OfferDate DATETIME = NULL,
    @TraineeCompletionPeriod DECIMAL = NULL,
    @TraineePeriodType NVARCHAR(50) = NULL,
    @NoOfChildren NVARCHAR(50) = NULL,
    @ESICNo NVARCHAR(50) = NULL,
    @PFNo NVARCHAR(50) = NULL,
    -- Contact Info (for UPDATE_CONTACT)
    @PresentAddress NVARCHAR(MAX) = NULL,
    @PresentTehsil NVARCHAR(100) = NULL,
    @PresentDistrict NVARCHAR(100) = NULL,
    @PresentCity NVARCHAR(100) = NULL,
    @PresentStateId INT = NULL,
    @PresentPincode NVARCHAR(20) = NULL,
    @PresentThanaId INT = NULL,
    @PermanentAddress NVARCHAR(MAX) = NULL,
    @PermanentTehsil NVARCHAR(100) = NULL,
    @PermanentDistrict NVARCHAR(100) = NULL,
    @PermanentCity NVARCHAR(100) = NULL,
    @PermanentStateId INT = NULL,
    @PermanentPincode NVARCHAR(20) = NULL,
    @PermanentThanaId INT = NULL,
    @CountryId INT = NULL,
    @WorkPhone NVARCHAR(20) = NULL,
    @PersonalPhone NVARCHAR(20) = NULL,
    @OfficialEmail NVARCHAR(100) = NULL,
    @Nationality NVARCHAR(50) = NULL,
    @ExtensionNo NVARCHAR(10) = NULL,
    @MobileNo NVARCHAR(20) = NULL,
    @SameAsPresentAddress BIT = 0,
    -- Salary Info (for UPDATE_SALARY)
    @PrimaryPaymentMode NVARCHAR(50) = NULL,
    @PrimaryBankName NVARCHAR(MAX) = NULL,
    @PrimaryIFSCCode NVARCHAR(20) = NULL,
    @PrimaryAccountNumber NVARCHAR(50) = NULL,
    @PrimaryBankBranchName NVARCHAR(100) = NULL,
    @WagesTypes NVARCHAR(50) = NULL,
    @GroupJoiningDate DATETIME = NULL,
    @BusinessSegmentId INT = NULL,
    @EmployeeSalaryReport NVARCHAR(MAX) = NULL,
    @EmployeePFReport NVARCHAR(MAX) = NULL,
    @EmployeePTReport NVARCHAR(MAX) = NULL,
    @EmployeeTaxReport NVARCHAR(MAX) = NULL,
    @EmployeeESIReport NVARCHAR(MAX) = NULL,
    @EmployeeNamePrmaryBank NVARCHAR(100) = NULL,
    -- Common
    @IsDeleted BIT = 0,
    @IsEnabled BIT = 1,
    @IsBlocked BIT = 0,
    @CreatedDate DATETIME = NULL,
    @CreatedBy NVARCHAR(100) = NULL,
    @AttendanceLimit INT = NULL,
    @Remark NVARCHAR(MAX) = NULL,
    @UpdatedDate DATETIME = NULL,
    @UpdatedBy NVARCHAR(100) = NULL,
    @DeletedDate DATETIME = NULL,
    @DeletedBy NVARCHAR(100) = NULL,
    @ResponseMessage NVARCHAR(255) OUTPUT,
    @Success BIT OUTPUT,
    @RoleId INT = NULL,
    @PhoneNumberConfirmed BIT = 0,
    @TwoFactorEnabled BIT = 0,
    @EmailConfirmed BIT = 0,
    @LockoutEnabled BIT = 0,
    @IsPasswordChange BIT = 0,
    @IsMobileAccess BIT = 0,
    @IsSelfiRequired BIT = 0,
    @IsGeofencingRequired BIT = 0, -- NEW
    @IsProbationEvaluated BIT = 0,
    @IsGTL BIT = 1,
    @IsGMP BIT = 0,
    @isInPunchMandatoryBiomatrix BIT = 0,
    @AccessFailedCount INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SET @Success = 0;
        SET @ResponseMessage = '';
        DECLARE @ProbationEndDate DATETIME = NULL;

        -- Start transaction at the beginning
        BEGIN TRANSACTION;

        -- CREATE: Insert a new employee
        IF @Action = 'CREATE'
        BEGIN
            -- Null checks
            IF @EmployeeCode IS NULL OR LTRIM(RTRIM(@EmployeeCode)) = ''
            BEGIN
                SET @Success = 0;
                SET @ResponseMessage = 'EmployeeCode cannot be null or empty.';
                ROLLBACK TRANSACTION;
                RETURN;
            END

            IF @LoginAlias IS NULL OR LTRIM(RTRIM(@LoginAlias)) = ''
            BEGIN
                SET @Success = 0;
                SET @ResponseMessage = 'LoginAlias cannot be null or empty.';
                ROLLBACK TRANSACTION;
                RETURN;
            END

            -- Check if EmployeeCode already exists
            IF EXISTS (SELECT 1 FROM AspNetUsers WHERE EmployeeCode = @EmployeeCode AND IsDeleted = 0)
            BEGIN
                SET @Success = 0;
                SET @ResponseMessage = 'EmployeeCode already exists.';
                ROLLBACK TRANSACTION;
                RETURN;
            END

            -- Check if LoginAlias already exists
            IF EXISTS (SELECT 1 FROM AspNetUsers WHERE LoginAlias = @LoginAlias AND IsDeleted = 0)
            BEGIN
                SET @Success = 0;
                SET @ResponseMessage = 'LoginAlias already exists.';
                ROLLBACK TRANSACTION;
                RETURN;
            END

            -- Check if RoleId is valid
            IF @RoleId IS NULL OR NOT EXISTS (
                SELECT 1
                FROM aspnetRoles
                WHERE Id = @RoleId
                  AND IsDeleted = 0
                  AND IsEnabled = 1
            )
            BEGIN
                SET @Success = 0;
                SET @ResponseMessage = 'Invalid or inactive RoleId.';
                ROLLBACK TRANSACTION;
                RETURN;
            END

            -- Calculate ProbationEndDate if ProbationCompletionPeriod is provided
            IF @ProbationCompletionPeriod IS NOT NULL AND @ProbationCompletionPeriod <> 0
            BEGIN
                IF @ProbationPeriodType = 'Months'
                    SET @ProbationEndDate = DATEADD(MONTH, @ProbationCompletionPeriod, @DateOfJoining);
                ELSE
                    SET @ProbationEndDate = DATEADD(DAY, @ProbationCompletionPeriod, @DateOfJoining);
            END

            -- Insert into AspNetUsers
            INSERT INTO AspNetUsers (
                FullName, Email, UserName, PasswordHash, Initial, FirstName, MiddleName, LastName, EmployeeCode,
                AlfaEmployeeCode, AlfaCode, DateOfJoining, BranchId, GradeId, ShiftMasterId, CTC, DesignationId,
                GrossSalary, CategoryId, BasicSalary, DepartmentId, EmployeeTypeId, DateOfBirth, LoginAlias,
                ReportingManagerId, SubBranch, EnrollNo, CompanyId, Overtime, Latemark, Earlymark, Fullpf, Pt,
                Fixsalary, Probation, Trainee, IsPFApplicable, WeekOffDetailsId, IsPermissionPunchInOut,
                ProbationEndDate, Gender, PersonalEmailId, FatherName, MotherName, BloodGroup, Height, MaritalStatus,
                MarriageDate, MarkIdentification, Religion, Caste, CastCategory, AadharCardNo, PANNo, Dispensary,
                DoctorName, DispensaryAddress, UANNumber, DrivingLicense, DrivingLicenseExpiry, RationCardType,
                RationCardNo, ProbationCompletionPeriod, ProbationPeriodType, ManagerProbationId, ConfirmDate,
                RetirementDate, OfferDate, TraineeCompletionPeriod, TraineePeriodType, NoOfChildren, ESICNo, PFNo,
                PresentAddress, PresentTehsil, PresentDistrict, PresentCity, PresentStateId, PresentPincode,
                PresentThanaId, PermanentAddress, PermanentTehsil, PermanentDistrict, PermanentCity,
                PermanentStateId, PermanentPincode, PermanentThanaId, CountryId, WorkPhone, PersonalPhone,
                OfficialEmail, Nationality, ExtensionNo, MobileNo, SameAsPresentAddress, PrimaryPaymentMode,
                PrimaryBankName, PrimaryIFSCCode, PrimaryAccountNumber, PrimaryBankBranchName, WagesTypes,
                GroupJoiningDate, BusinessSegmentId, EmployeeSalaryReport, EmployeePFReport, EmployeePTReport,
                EmployeeTaxReport, EmployeeESIReport, EmployeeNamePrmaryBank, IsDeleted, IsEnabled, IsBlocked,
                CreatedDate, CreatedBy, AttendanceLimit,Remark, IsLeft, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled,
                LockoutEnabled, IsPasswordChange, IsMobileAccess, IsSelfiRequired, IsGeofencingRequired, IsProbationEvaluated,IsGTL,IsGMP, isInPunchMandatoryBiomatrix, AccessFailedCount
            )
            VALUES (
                @FullName, @LoginAlias, @LoginAlias, @Password, @Initial, @FirstName, @MiddleName, @LastName, @EmployeeCode,
                @AlfaEmployeeCode, @AlfaCode, @DateOfJoining, @BranchId, @GradeId, @ShiftMasterId, @CTC, @DesignationId,
                @GrossSalary, @CategoryId, @BasicSalary, @DepartmentId, @EmployeeTypeId, @DateOfBirth, @LoginAlias,
                @ReportingManagerId, @SubBranch, @EnrollNo, @CompanyId, @Overtime, @Latemark, @Earlymark, @Fullpf, @Pt,
                @Fixsalary, @Probation, @Trainee, @IsPFApplicable, @WeekOffDetailsId, @IsPermissionPunchInOut,
                @ProbationEndDate, @Gender, @PersonalEmailId, @FatherName, @MotherName, @BloodGroup, @Height, @MaritalStatus,
                @MarriageDate, @MarkIdentification, @Religion, @Caste, @CastCategory, @AadharCardNo, @PANNo, @Dispensary,
                @DoctorName, @DispensaryAddress, @UANNumber, @DrivingLicense, @DrivingLicenseExpiry, @RationCardType,
                @RationCardNo, @ProbationCompletionPeriod, @ProbationPeriodType, @ManagerProbationId, @ConfirmDate,
                @RetirementDate, @OfferDate, @TraineeCompletionPeriod, @TraineePeriodType, @NoOfChildren, @ESICNo, @PFNo,
                @PresentAddress, @PresentTehsil, @PresentDistrict, @PresentCity, @PresentStateId, @PresentPincode,
                @PresentThanaId, @PermanentAddress, @PermanentTehsil, @PermanentDistrict, @PermanentCity,
                @PermanentStateId, @PermanentPincode, @PermanentThanaId, @CountryId, @WorkPhone, @PersonalPhone,
                @OfficialEmail, @Nationality, @ExtensionNo, @MobileNo, @SameAsPresentAddress, @PrimaryPaymentMode,
                @PrimaryBankName, @PrimaryIFSCCode, @PrimaryAccountNumber, @PrimaryBankBranchName, @WagesTypes,
                @GroupJoiningDate, @BusinessSegmentId, @EmployeeSalaryReport, @EmployeePFReport, @EmployeePTReport,
                @EmployeeTaxReport, @EmployeeESIReport, @EmployeeNamePrmaryBank, 0, 1, 0, GETUTCDATE(), @CreatedBy,
                @AttendanceLimit,@Remark, @IsLeft, @EmailConfirmed, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnabled,
                @IsPasswordChange, @IsMobileAccess, @IsSelfiRequired, @IsGeofencingRequired, @IsProbationEvaluated,@IsGTL,@IsGMP, @isInPunchMandatoryBiomatrix, @AccessFailedCount
            );

            DECLARE @NewEmployeeId INT = SCOPE_IDENTITY();

            -- Call ManageHRMSUserRole SP
            EXEC ManageHRMSUserRole
                @Action = 'CREATE',
                @EmployeeId = @NewEmployeeId,
                @RoleId = @RoleId,
                @CompanyId = @CompanyId,
                @IsEnabled = @IsEnabled,
                @IsDeleted = @IsDeleted,
                @CreatedBy = @CreatedBy,
                @CreatedDate = @CreatedDate;

            -- Call USP_CalculateSalaryStructure SP
            EXEC USP_CalculateSalaryStructure
                @Action = 'CREATE',
                @EmployeeId = @NewEmployeeId,
                @CompanyId = @CompanyId,
                @GrossSalary = @GrossSalary,
                @BasicSalary = @BasicSalary,
                @IsPFApplicable = @IsPFApplicable;

            -- Call ManageUserCompanyPermission SP
            DECLARE @IsAdmin BIT = 0;
            SELECT @IsAdmin = CASE WHEN LOWER(Slug) = 'admin' THEN 1 ELSE 0 END
            FROM aspnetRoles
            WHERE Id = @RoleId AND IsDeleted = 0 AND IsEnabled = 1;

            EXEC ManageUserCompanyPermission
                @Action = 'CREATE',
                @EmployeeId = @NewEmployeeId,
                @CompanyId = @CompanyId,
                @IsAdmin = @IsAdmin,
                @CreatedBy = @CreatedBy;

            -- Call ManagePasswordHistory SP
            EXEC ManagePasswordHistory
                @Action = 'CREATE',
                @EMPID = @NewEmployeeId,
                @NewPassword = @Password,
                @CreatedBy = @CreatedBy;

            -- Call ManageReportingManagerDetails SP
            EXEC ManageReportingManagerDetails
                @Action = 'CREATE',
                @EmployeeId = @NewEmployeeId,
                @ReportingManagerId = @ReportingManagerId,
                @MethodName = 'In Person',
                @CreatedBy = @CreatedBy;

            -- Call sp_AddProbationEndDate SP if Probation is true
            IF @Probation = 1
            BEGIN
                EXEC sp_AddProbationEndDate
                    @Id = @NewEmployeeId,
                    @GradeId = @GradeId,
                    @DateOfJoining = @DateOfJoining;
            END

            COMMIT TRANSACTION;
            SET @Success = 1;
            SET @ResponseMessage = 'Employee created successfully.';
            SELECT @NewEmployeeId AS Id;
        END

        -- UPDATE_BASIC: Update basic employee info
        ELSE IF @Action = 'UPDATE_BASIC'
        BEGIN
            -- Old values ko JSON mein store karein
            DECLARE @OldValues_Basic NVARCHAR(MAX);
            SELECT @OldValues_Basic = (
                SELECT *
                FROM AspNetUsers
                WHERE Id = @Id
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            );

            UPDATE AspNetUsers
            SET
                FullName = ISNULL(@FullName, FullName),
                Initial = ISNULL(@Initial, Initial),
                FirstName = ISNULL(@FirstName, FirstName),
                MiddleName = ISNULL(@MiddleName, MiddleName),
                LastName = ISNULL(@LastName, LastName),
                EmployeeCode = ISNULL(@EmployeeCode, EmployeeCode),
                AlfaEmployeeCode = ISNULL(@AlfaEmployeeCode, AlfaEmployeeCode),
                AlfaCode = ISNULL(@AlfaCode, AlfaCode),
                DateOfJoining = ISNULL(@DateOfJoining, DateOfJoining),
                BranchId = ISNULL(@BranchId, BranchId),
                GradeId = ISNULL(@GradeId, GradeId),
                ShiftMasterId = ISNULL(@ShiftMasterId, ShiftMasterId),
                CTC = ISNULL(@CTC, CTC),
                DesignationId = ISNULL(@DesignationId, DesignationId),
                GrossSalary = ISNULL(@GrossSalary, GrossSalary),
                CategoryId = ISNULL(@CategoryId, CategoryId),
                BasicSalary = ISNULL(@BasicSalary, BasicSalary),
                DepartmentId = ISNULL(@DepartmentId, DepartmentId),
                EmployeeTypeId = ISNULL(@EmployeeTypeId, EmployeeTypeId),
                DateOfBirth = ISNULL(@DateOfBirth, DateOfBirth),
                LoginAlias = ISNULL(@LoginAlias, LoginAlias),
                Email = ISNULL(@LoginAlias, LoginAlias),
                ReportingManagerId = ISNULL(@ReportingManagerId, ReportingManagerId),
                SubBranch = ISNULL(@SubBranch, SubBranch),
                EnrollNo = ISNULL(@EnrollNo, EnrollNo),
                CompanyId = ISNULL(@CompanyId, CompanyId),
                Overtime = ISNULL(@Overtime, Overtime),
                Latemark = ISNULL(@Latemark, Latemark),
                Earlymark = ISNULL(@Earlymark, Earlymark),
                Fullpf = ISNULL(@Fullpf, Fullpf),
                Pt = ISNULL(@Pt, Pt),
                Fixsalary = ISNULL(@Fixsalary, Fixsalary),
                Probation = ISNULL(@Probation, Probation),
                Trainee = ISNULL(@Trainee, Trainee),
                IsPFApplicable = ISNULL(@IsPFApplicable, IsPFApplicable),
                WeekOffDetailsId = ISNULL(@WeekOffDetailsId, WeekOffDetailsId),
                IsPermissionPunchInOut = ISNULL(@IsPermissionPunchInOut, IsPermissionPunchInOut),
                ProbationEndDate = @ProbationEndDate,
                AttendanceLimit = ISNULL(@AttendanceLimit, AttendanceLimit),
			    Remark = ISNULL(@Remark, Remark),
			    IsGTL = ISNULL(@IsGTL, IsGTL),
			    IsGMP = ISNULL(@IsGMP, IsGMP),
                IsMobileAccess = ISNULL(@IsMobileAccess, IsMobileAccess), -- NEW
                IsSelfiRequired = ISNULL(@IsSelfiRequired, IsSelfiRequired), -- NEW
                IsGeofencingRequired = ISNULL(@IsGeofencingRequired, IsGeofencingRequired), -- NEW
                UpdatedDate = ISNULL(@UpdatedDate, GETUTCDATE()),
                UpdatedBy = @UpdatedBy
            WHERE Id = @Id;

            -- Probation end date update
            IF EXISTS (
                SELECT 1
                FROM AspNetUsers
                WHERE Id = @Id
                  AND Probation = 1
                  AND IsProbationEvaluated = 0
            )
            BEGIN
                DECLARE @DueConfirmationDurationMonth INT = 0;
                SELECT @DueConfirmationDurationMonth = DueConfirmationDurationMonth
                FROM Grade
                WHERE GradeId = @GradeId;

                SET @ProbationEndDate = DATEADD(MONTH, @DueConfirmationDurationMonth, @DateOfJoining);
                UPDATE AspNetUsers
                SET ProbationEndDate = @ProbationEndDate
                WHERE Id = @Id;
            END

            -- New values ko JSON mein store karein
            DECLARE @NewValues_Basic NVARCHAR(MAX);
            SELECT @NewValues_Basic = (
                SELECT *
                FROM AspNetUsers
                WHERE Id = @Id
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            );

            -- Log dalein
            DECLARE @ColumnName_Basic NVARCHAR(100);
            DECLARE @OldValue_Basic NVARCHAR(MAX);
            DECLARE @NewValue_Basic NVARCHAR(MAX);
            DECLARE @Columns_Basic TABLE (ColumnName NVARCHAR(100));
            INSERT INTO @Columns_Basic (ColumnName)
            VALUES
                ('FullName'), ('Initial'), ('FirstName'), ('MiddleName'), ('LastName'),
                ('EmployeeCode'), ('AlfaEmployeeCode'), ('AlfaCode'), ('DateOfJoining'),
                ('BranchId'), ('GradeId'), ('ShiftMasterId'), ('CTC'), ('DesignationId'),
                ('GrossSalary'), ('CategoryId'), ('BasicSalary'), ('DepartmentId'),
                ('EmployeeTypeId'), ('DateOfBirth'), ('LoginAlias'), ('ReportingManagerId'),
                ('SubBranch'), ('EnrollNo'), ('CompanyId'), ('Overtime'), ('Latemark'),
                ('Earlymark'), ('Fullpf'), ('Pt'), ('Fixsalary'), ('Probation'), ('Trainee'),
                ('IsPFApplicable'), ('WeekOffDetailsId'), ('IsPermissionPunchInOut'),
                ('ProbationEndDate'), ('AttendanceLimit'),('Remark'),('IsGTL'),('IsGMP'),
                ('IsMobileAccess'),('IsSelfiRequired'),('IsGeofencingRequired'); -- NEW

            DECLARE ColumnCursor_Basic CURSOR FOR
            SELECT ColumnName FROM @Columns_Basic;

            OPEN ColumnCursor_Basic;
            FETCH NEXT FROM ColumnCursor_Basic INTO @ColumnName_Basic;

            WHILE @@FETCH_STATUS = 0
            BEGIN
                SET @OldValue_Basic = JSON_VALUE(@OldValues_Basic, CONCAT('$.', @ColumnName_Basic));
                SET @NewValue_Basic = JSON_VALUE(@NewValues_Basic, CONCAT('$.', @ColumnName_Basic));

                IF (@OldValue_Basic <> @NewValue_Basic
                    OR (@OldValue_Basic IS NULL AND @NewValue_Basic IS NOT NULL)
                    OR (@OldValue_Basic IS NOT NULL AND @NewValue_Basic IS NULL))
                BEGIN
                    INSERT INTO EmployeeLog (
                        EmployeeId, ChangedBy, ChangedColumnName, OldValue, NewValue
                    )
                    VALUES (
                        @Id, @UpdatedBy, @ColumnName_Basic, ISNULL(@OldValue_Basic, 'NULL'), ISNULL(@NewValue_Basic, 'NULL')
                    );
                END
                FETCH NEXT FROM ColumnCursor_Basic INTO @ColumnName_Basic;
            END

            CLOSE ColumnCursor_Basic;
            DEALLOCATE ColumnCursor_Basic;

            -- Role and salary update logic
            IF NOT EXISTS (SELECT 1 FROM HRMSUserRoles WHERE EmployeeId = @Id AND IsDeleted = 0 AND IsEnabled = 1)
            BEGIN
                EXEC ManageHRMSUserRole
                    @Action = 'CREATE',
                    @EmployeeId = @Id,
                    @RoleId = @RoleId,
                    @CompanyId = @CompanyId,
                    @IsEnabled = @IsEnabled,
                    @IsDeleted = @IsDeleted;
            END
            ELSE
            BEGIN
                EXEC ManageHRMSUserRole
                    @Action = 'UPDATE',
                    @EmployeeId = @Id,
                    @RoleId = @RoleId;
            END

            IF NOT EXISTS (
                SELECT 1
                FROM EmployeeSalaryAllowance
                WHERE EmployeeId = @Id
                    AND IsDeleted = 0
                    AND IsEnabled = 1
            )
            BEGIN
                EXEC USP_CalculateSalaryStructure
                    @Action = 'CREATE',
                    @EmployeeId = @Id,
                    @CompanyId = @CompanyId,
                    @GrossSalary = @GrossSalary,
                    @BasicSalary = @BasicSalary,
                    @IsPFApplicable = @IsPFApplicable;
            END
            ELSE
            BEGIN
                EXEC USP_CalculateSalaryStructure
                    @Action = 'UPDATE',
                    @EmployeeId = @Id,
                    @CompanyId = @CompanyId,
                    @GrossSalary = @GrossSalary,
                    @BasicSalary = @BasicSalary,
                    @IsPFApplicable = @IsPFApplicable;
            END

            DECLARE @LastReportingManagerId INT;
            SELECT TOP 1 @LastReportingManagerId = ReportingManagerId
            FROM ReportingManagerDetails
            WHERE EmployeeId = @Id AND IsDeleted = 0 AND IsEnabled = 1
            ORDER BY ReportingManagerDetailsId DESC;

            IF (@ReportingManagerId <> @LastReportingManagerId)
            BEGIN
                INSERT INTO ReportingManagerDetails (
                    EmployeeId, ReportingManagerId, EffectedDate, IsDeleted, IsEnabled, MethodName, CreatedDate, CreatedBy
                )
                VALUES (
                    @Id, @ReportingManagerId, GETUTCDATE(), 0, 1, 'In Person', GETUTCDATE(), @UpdatedBy
                );
            END

            COMMIT TRANSACTION;
            SET @Success = 1;
            SET @ResponseMessage = 'Basic employee info updated successfully.';
            SELECT @Id AS Id;
        END

        -- UPDATE_PERSONAL: Update personal info
        ELSE IF @Action = 'UPDATE_PERSONAL'
        BEGIN
            -- Old values ko JSON mein store karein
            DECLARE @OldValues_Personal NVARCHAR(MAX);
            SELECT @OldValues_Personal = (
                SELECT *
                FROM AspNetUsers
                WHERE Id = @Id
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            );

            UPDATE AspNetUsers
            SET
                Gender = ISNULL(@Gender, Gender),
                PersonalEmailId = ISNULL(@PersonalEmailId, PersonalEmailId),
                FatherName = ISNULL(@FatherName, FatherName),
                MotherName = ISNULL(@MotherName, MotherName),
                BloodGroup = ISNULL(@BloodGroup, BloodGroup),
                Height = ISNULL(@Height, Height),
                MaritalStatus = ISNULL(@MaritalStatus, MaritalStatus),
                MarriageDate = ISNULL(@MarriageDate, MarriageDate),
                MarkIdentification = ISNULL(@MarkIdentification, MarkIdentification),
                Religion = ISNULL(@Religion, Religion),
                Caste = ISNULL(@Caste, Caste),
                CastCategory = ISNULL(@CastCategory, CastCategory),
                AadharCardNo = ISNULL(@AadharCardNo, AadharCardNo),
                PANNo = ISNULL(@PANNo, PANNo),
                Dispensary = ISNULL(@Dispensary, Dispensary),
                DoctorName = ISNULL(@DoctorName, DoctorName),
                DispensaryAddress = ISNULL(@DispensaryAddress, DispensaryAddress),
                UANNumber = ISNULL(@UANNumber, UANNumber),
                DrivingLicense = ISNULL(@DrivingLicense, DrivingLicense),
                DrivingLicenseExpiry = ISNULL(@DrivingLicenseExpiry, DrivingLicenseExpiry),
                RationCardType = ISNULL(@RationCardType, RationCardType),
                RationCardNo = ISNULL(@RationCardNo, RationCardNo),
                ProbationCompletionPeriod = ISNULL(@ProbationCompletionPeriod, ProbationCompletionPeriod),
                ProbationPeriodType = ISNULL(@ProbationPeriodType, ProbationPeriodType),
                ManagerProbationId = ISNULL(@ManagerProbationId, ManagerProbationId),
                ConfirmDate = ISNULL(@ConfirmDate, ConfirmDate),
                RetirementDate = ISNULL(@RetirementDate, RetirementDate),
                OfferDate = ISNULL(@OfferDate, OfferDate),
                TraineeCompletionPeriod = ISNULL(@TraineeCompletionPeriod, TraineeCompletionPeriod),
                TraineePeriodType = ISNULL(@TraineePeriodType, TraineePeriodType),
                NoOfChildren = ISNULL(@NoOfChildren, NoOfChildren),
                ESICNo = ISNULL(@ESICNo, ESICNo),
                PFNo = ISNULL(@PFNo, PFNo),
                UpdatedDate = ISNULL(@UpdatedDate, GETUTCDATE()),
                UpdatedBy = @UpdatedBy
            WHERE Id = @Id;

            -- New values ko JSON mein store karein
            DECLARE @NewValues_Personal NVARCHAR(MAX);
            SELECT @NewValues_Personal = (
                SELECT *
                FROM AspNetUsers
                WHERE Id = @Id
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            );

            -- Log dalein
            DECLARE @ColumnName_Personal NVARCHAR(100);
            DECLARE @OldValue_Personal NVARCHAR(MAX);
            DECLARE @NewValue_Personal NVARCHAR(MAX);
            DECLARE @Columns_Personal TABLE (ColumnName NVARCHAR(100));
            INSERT INTO @Columns_Personal (ColumnName)
            VALUES
                ('Gender'), ('PersonalEmailId'), ('FatherName'), ('MotherName'), ('BloodGroup'),
                ('Height'), ('MaritalStatus'), ('MarriageDate'), ('MarkIdentification'), ('Religion'),
                ('Caste'), ('CastCategory'), ('AadharCardNo'), ('PANNo'), ('Dispensary'),
                ('DoctorName'), ('DispensaryAddress'), ('UANNumber'), ('DrivingLicense'),
                ('DrivingLicenseExpiry'), ('RationCardType'), ('RationCardNo'),
                ('ProbationCompletionPeriod'), ('ProbationPeriodType'), ('ManagerProbationId'),
                ('ConfirmDate'), ('RetirementDate'), ('OfferDate'), ('TraineeCompletionPeriod'),
                ('TraineePeriodType'), ('NoOfChildren'), ('ESICNo'), ('PFNo');

            DECLARE ColumnCursor_Personal CURSOR FOR
            SELECT ColumnName FROM @Columns_Personal;

            OPEN ColumnCursor_Personal;
            FETCH NEXT FROM ColumnCursor_Personal INTO @ColumnName_Personal;

            WHILE @@FETCH_STATUS = 0
            BEGIN
                SET @OldValue_Personal = JSON_VALUE(@OldValues_Personal, CONCAT('$.', @ColumnName_Personal));
                SET @NewValue_Personal = JSON_VALUE(@NewValues_Personal, CONCAT('$.', @ColumnName_Personal));

                IF (@OldValue_Personal <> @NewValue_Personal
                    OR (@OldValue_Personal IS NULL AND @NewValue_Personal IS NOT NULL)
                    OR (@OldValue_Personal IS NOT NULL AND @NewValue_Personal IS NULL))
                BEGIN
                    INSERT INTO EmployeeLog (
                        EmployeeId, ChangedBy, ChangedColumnName, OldValue, NewValue
                    )
                    VALUES (
                        @Id, @UpdatedBy, @ColumnName_Personal, ISNULL(@OldValue_Personal, 'NULL'), ISNULL(@NewValue_Personal, 'NULL')
                    );
                END
                FETCH NEXT FROM ColumnCursor_Personal INTO @ColumnName_Personal;
            END

            CLOSE ColumnCursor_Personal;
            DEALLOCATE ColumnCursor_Personal;

            COMMIT TRANSACTION;
            SET @Success = 1;
            SET @ResponseMessage = 'Personal info updated successfully.';
            SELECT @Id AS Id;
        END

        -- UPDATE_CONTACT: Update contact info
        ELSE IF @Action = 'UPDATE_CONTACT'
        BEGIN
            -- Old values ko JSON mein store karein
            DECLARE @OldValues_Contact NVARCHAR(MAX);
            SELECT @OldValues_Contact = (
                SELECT *
                FROM AspNetUsers
                WHERE Id = @Id
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            );

            UPDATE AspNetUsers
            SET
                PresentAddress = ISNULL(@PresentAddress, PresentAddress),
                PresentTehsil = ISNULL(@PresentTehsil, PresentTehsil),
                PresentDistrict = ISNULL(@PresentDistrict, PresentDistrict),
                PresentCity = ISNULL(@PresentCity, PresentCity),
                PresentStateId = ISNULL(@PresentStateId, PresentStateId),
                PresentPincode = ISNULL(@PresentPincode, PresentPincode),
                PresentThanaId = ISNULL(@PresentThanaId, PresentThanaId),
                PermanentAddress = ISNULL(@PermanentAddress, PermanentAddress),
                PermanentTehsil = ISNULL(@PermanentTehsil, PermanentTehsil),
                PermanentDistrict = ISNULL(@PermanentDistrict, PermanentDistrict),
                PermanentCity = ISNULL(@PermanentCity, PermanentCity),
                PermanentStateId = ISNULL(@PermanentStateId, PermanentStateId),
                PermanentPincode = ISNULL(@PermanentPincode, PermanentPincode),
                PermanentThanaId = ISNULL(@PermanentThanaId, PermanentThanaId),
                CountryId = ISNULL(@CountryId, CountryId),
                WorkPhone = ISNULL(@WorkPhone, WorkPhone),
                PersonalPhone = ISNULL(@PersonalPhone, PersonalPhone),
                OfficialEmail = ISNULL(@OfficialEmail, OfficialEmail),
                Nationality = ISNULL(@Nationality, Nationality),
                ExtensionNo = ISNULL(@ExtensionNo, ExtensionNo),
                MobileNo = ISNULL(@MobileNo, MobileNo),
                SameAsPresentAddress = ISNULL(@SameAsPresentAddress, SameAsPresentAddress),
                UpdatedDate = ISNULL(@UpdatedDate, GETUTCDATE()),
                UpdatedBy = @UpdatedBy
            WHERE Id = @Id;

            -- New values ko JSON mein store karein
            DECLARE @NewValues_Contact NVARCHAR(MAX);
            SELECT @NewValues_Contact = (
                SELECT *
                FROM AspNetUsers
                WHERE Id = @Id
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            );

            -- Log dalein
            DECLARE @ColumnName_Contact NVARCHAR(100);
            DECLARE @OldValue_Contact NVARCHAR(MAX);
            DECLARE @NewValue_Contact NVARCHAR(MAX);
            DECLARE @Columns_Contact TABLE (ColumnName NVARCHAR(100));
            INSERT INTO @Columns_Contact (ColumnName)
            VALUES
                ('PresentAddress'), ('PresentTehsil'), ('PresentDistrict'), ('PresentCity'), ('PresentStateId'),
                ('PresentPincode'), ('PresentThanaId'), ('PermanentAddress'), ('PermanentTehsil'), ('PermanentDistrict'),
                ('PermanentCity'), ('PermanentStateId'), ('PermanentPincode'), ('PermanentThanaId'), ('CountryId'),
                ('WorkPhone'), ('PersonalPhone'), ('OfficialEmail'), ('Nationality'), ('ExtensionNo'), ('MobileNo'),
                ('SameAsPresentAddress');

            DECLARE ColumnCursor_Contact CURSOR FOR
            SELECT ColumnName FROM @Columns_Contact;

            OPEN ColumnCursor_Contact;
            FETCH NEXT FROM ColumnCursor_Contact INTO @ColumnName_Contact;

            WHILE @@FETCH_STATUS = 0
            BEGIN
                SET @OldValue_Contact = JSON_VALUE(@OldValues_Contact, CONCAT('$.', @ColumnName_Contact));
                SET @NewValue_Contact = JSON_VALUE(@NewValues_Contact, CONCAT('$.', @ColumnName_Contact));

                IF (@OldValue_Contact <> @NewValue_Contact
                    OR (@OldValue_Contact IS NULL AND @NewValue_Contact IS NOT NULL)
                    OR (@OldValue_Contact IS NOT NULL AND @NewValue_Contact IS NULL))
                BEGIN
                    INSERT INTO EmployeeLog (
                        EmployeeId, ChangedBy, ChangedColumnName, OldValue, NewValue
                    )
                    VALUES (
                        @Id, @UpdatedBy, @ColumnName_Contact, ISNULL(@OldValue_Contact, 'NULL'), ISNULL(@NewValue_Contact, 'NULL')
                    );
                END
                FETCH NEXT FROM ColumnCursor_Contact INTO @ColumnName_Contact;
            END

            CLOSE ColumnCursor_Contact;
            DEALLOCATE ColumnCursor_Contact;

            COMMIT TRANSACTION;
            SET @Success = 1;
            SET @ResponseMessage = 'Contact info updated successfully.';
            SELECT @Id AS Id;
        END

        -- UPDATE_SALARY: Update salary info
        ELSE IF @Action = 'UPDATE_SALARY'
        BEGIN
            -- Old values ko JSON mein store karein
            DECLARE @OldValues_Salary NVARCHAR(MAX);
            SELECT @OldValues_Salary = (
                SELECT *
                FROM AspNetUsers
                WHERE Id = @Id
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            );

            UPDATE AspNetUsers
            SET
                PrimaryPaymentMode = ISNULL(@PrimaryPaymentMode, PrimaryPaymentMode),
                PrimaryBankName = ISNULL(@PrimaryBankName, PrimaryBankName),
                PrimaryIFSCCode = ISNULL(@PrimaryIFSCCode, PrimaryIFSCCode),
                PrimaryAccountNumber = ISNULL(@PrimaryAccountNumber, PrimaryAccountNumber),
                PrimaryBankBranchName = ISNULL(@PrimaryBankBranchName, PrimaryBankBranchName),
                WagesTypes = ISNULL(@WagesTypes, WagesTypes),
                GroupJoiningDate = ISNULL(@GroupJoiningDate, GroupJoiningDate),
                BusinessSegmentId = ISNULL(@BusinessSegmentId, BusinessSegmentId),
                EmployeeSalaryReport = ISNULL(@EmployeeSalaryReport, EmployeeSalaryReport),
                EmployeePFReport = ISNULL(@EmployeePFReport, EmployeePFReport),
                EmployeePTReport = ISNULL(@EmployeePTReport, EmployeePTReport),
                EmployeeTaxReport = ISNULL(@EmployeeTaxReport, EmployeeTaxReport),
                EmployeeESIReport = ISNULL(@EmployeeESIReport, EmployeeESIReport),
                EmployeeNamePrmaryBank = ISNULL(@EmployeeNamePrmaryBank, EmployeeNamePrmaryBank),
                UpdatedDate = ISNULL(@UpdatedDate, GETUTCDATE()),
                UpdatedBy = @UpdatedBy
            WHERE Id = @Id;

            -- New values ko JSON mein store karein
            DECLARE @NewValues_Salary NVARCHAR(MAX);
            SELECT @NewValues_Salary = (
                SELECT *
                FROM AspNetUsers
                WHERE Id = @Id
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            );

            -- Log dalein
            DECLARE @ColumnName_Salary NVARCHAR(100);
            DECLARE @OldValue_Salary NVARCHAR(MAX);
            DECLARE @NewValue_Salary NVARCHAR(MAX);
            DECLARE @Columns_Salary TABLE (ColumnName NVARCHAR(100));
            INSERT INTO @Columns_Salary (ColumnName)
            VALUES
                ('PrimaryPaymentMode'), ('PrimaryBankName'), ('PrimaryIFSCCode'), ('PrimaryAccountNumber'),
                ('PrimaryBankBranchName'), ('WagesTypes'), ('GroupJoiningDate'), ('BusinessSegmentId'),
                ('EmployeeSalaryReport'), ('EmployeePFReport'), ('EmployeePTReport'), ('EmployeeTaxReport'),
                ('EmployeeESIReport'), ('EmployeeNamePrmaryBank');

            DECLARE ColumnCursor_Salary CURSOR FOR
            SELECT ColumnName FROM @Columns_Salary;

            OPEN ColumnCursor_Salary;
            FETCH NEXT FROM ColumnCursor_Salary INTO @ColumnName_Salary;

            WHILE @@FETCH_STATUS = 0
            BEGIN
                SET @OldValue_Salary = JSON_VALUE(@OldValues_Salary, CONCAT('$.', @ColumnName_Salary));
                SET @NewValue_Salary = JSON_VALUE(@NewValues_Salary, CONCAT('$.', @ColumnName_Salary));

                IF (@OldValue_Salary <> @NewValue_Salary
                    OR (@OldValue_Salary IS NULL AND @NewValue_Salary IS NOT NULL)
                    OR (@OldValue_Salary IS NOT NULL AND @NewValue_Salary IS NULL))
                BEGIN
                    INSERT INTO EmployeeLog (
                        EmployeeId, ChangedBy, ChangedColumnName, OldValue, NewValue
                    )
                    VALUES (
                        @Id, @UpdatedBy, @ColumnName_Salary, ISNULL(@OldValue_Salary, 'NULL'), ISNULL(@NewValue_Salary, 'NULL')
                    );
                END
                FETCH NEXT FROM ColumnCursor_Salary INTO @ColumnName_Salary;
            END

            CLOSE ColumnCursor_Salary;
            DEALLOCATE ColumnCursor_Salary;

            COMMIT TRANSACTION;
            SET @Success = 1;
            SET @ResponseMessage = 'Salary info updated successfully.';
            SELECT @Id AS Id;
        END

        -- DELETE: Soft delete employee
        ELSE IF @Action = 'DELETE'
        BEGIN
            UPDATE AspNetUsers
            SET
                IsDeleted = 1,
                IsEnabled = 0,
                DeletedDate = ISNULL(@DeletedDate, GETUTCDATE()),
                DeletedBy = @DeletedBy
            WHERE Id = @Id;

            COMMIT TRANSACTION;
            SET @Success = 1;
            SET @ResponseMessage = 'Employee deleted successfully.';
            SELECT @Id AS Id;
        END

        -- IsBlocked: Block/unblock employee
        ELSE IF @Action = 'IsBlocked'
        BEGIN
            UPDATE AspNetUsers
            SET
                IsBlocked = @IsBlocked
            WHERE Id = @Id;

            COMMIT TRANSACTION;
            SET @Success = 1;
            SET @ResponseMessage = 'Employee block status updated successfully.';
            SELECT @Id AS Id;
        END

        -- Invalid Action
        ELSE
        BEGIN
            SET @Success = 0;
            SET @ResponseMessage = 'Invalid action specified.';
            ROLLBACK TRANSACTION;
        END
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SET @Success = 0;
        SET @ResponseMessage = ERROR_MESSAGE();
    END CATCH
END;
GO


-- ============================================================================
-- 2) ManageEmployee (legacy — apparently unused today; no Razor/JS calls
--    EmployeeMasterAPIController's [HttpPost("UpdateEmployee")] endpoint,
--    which is the only C# caller of this SP, per grep across
--    HRMS.UI/wwwroot and the Areas views). Fixed anyway so it isn't a
--    landmine that throws "parameter not found" the moment anyone wires it
--    up or an external caller hits the still-live endpoint.
--    Changes: added @IsMobileAccess, @IsSelfiRequired, @IsGeofencingRequired
--    parameters; added them to the UPDATE branch's SET clause.
-- ============================================================================

ALTER PROCEDURE [dbo].[ManageEmployee]
    @Action NVARCHAR(10), -- 'CREATE', 'UPDATE', 'DELETE'
    @Id int = NULL,
    @Initial NVARCHAR(50) = NULL,
    @FirstName NVARCHAR(100) = NULL,
    @MiddleName NVARCHAR(100) = NULL,
    @LastName NVARCHAR(100) = NULL,
    @FullName NVARCHAR(max) = NULL,
    @EmployeeCode NVARCHAR(50) = NULL,
	@AlfaCode NVARCHAR(max) = NULL,
	@AlfaEmployeeCode NVARCHAR(max) = NULL,
    @DateOfJoining DATETIME = NULL,
    @BranchId INT = NULL,
    @GradeId INT = NULL,
    @ShiftMasterId NVARCHAR(100) = NULL,
    @CTC NVARCHAR(100) = NULL,
    @DesignationId INT = NULL,
    @GrossSalary DECIMAL(18,2) = NULL,
    @CategoryId NVARCHAR(100) = NULL,
    @BasicSalary DECIMAL(18,2) = NULL,
    @DepartmentId INT = NULL,
    @EmployeeTypeId NVARCHAR(100) = NULL,
    @DateOfBirth DATETIME = NULL,
    @LoginAlias NVARCHAR(100) = NULL,
    @Password NVARCHAR(100) = NULL,
    @ReportingManagerId int = NULL,
    @SubBranch NVARCHAR(100) = NULL,
    @EnrollNo NVARCHAR(100) = NULL,
    @CompanyId INT = NULL,
    @Overtime BIT = 0,
    @Latemark BIT = 0,
    @Earlymark BIT = 0,
    @Fullpf BIT = 0,
    @Pt BIT = 0,
    @Fixsalary BIT = 0,
    @Probation BIT = 0,
    @Trainee BIT = 0,
    @EmployeeProfileUrl NVARCHAR(255) = NULL,
    @EmployeeSignatureUrl NVARCHAR(255) = NULL,
    @IsDeleted BIT = 0,
    @IsEnabled BIT = 1,
    @IsBlocked BIT = 0,
    @CreatedDate DATETIME = NULL,
    @CreatedBy NVARCHAR(100) = NULL,
    @UpdatedDate DATETIME = NULL,
    @UpdatedBy NVARCHAR(100) = NULL,
    @DeletedDate DATETIME = NULL,
    @DeletedBy NVARCHAR(100) = NULL,
    @IsPFApplicable bit=1,

    @PhoneNumber NVARCHAR(50) = NULL, -- Include PhoneNumber parameter
     @WeekOffDetailsId int=null,
	@IsPermissionPunchInOut bit=0,

	@Gender NVARCHAR(20) = NULL,
    @PersonalEmailId NVARCHAR(100) = NULL,
    @FatherName NVARCHAR(100) = NULL,
    @MotherName NVARCHAR(100) = NULL,
    @BloodGroup NVARCHAR(10) = NULL,
    @Height NVARCHAR(50) = NULL,
    @MaritalStatus NVARCHAR(50) = NULL,
    @MarriageDate DATETIME = NULL,
    @MarkIdentification NVARCHAR(255) = NULL,
    @Religion NVARCHAR(100) = NULL,
    @Caste NVARCHAR(100) = NULL,
    @CastCategory NVARCHAR(100) = NULL,
    @AadharCardNo NVARCHAR(20) = NULL,
    @PANNo NVARCHAR(20) = NULL,
    @Dispensary NVARCHAR(255) = NULL,
    @DoctorName NVARCHAR(100) = NULL,
    @DispensaryAddress NVARCHAR(255) = NULL,
    @UANNumber NVARCHAR(20) = NULL,
    @DrivingLicense NVARCHAR(50) = NULL,
    @DrivingLicenseExpiry DATETIME = NULL,
    @RationCardType NVARCHAR(50) = NULL,
    @RationCardNo NVARCHAR(50) = NULL,
    @ProbationCompletionPeriod decimal = NULL,
    @ProbationPeriodType NVARCHAR(50) = NULL,
    @ManagerProbationId INT = NULL,
    @ConfirmDate DATETIME = NULL,
    @RetirementDate DATETIME = NULL,
    @OfferDate DATETIME = NULL,
    @TraineeCompletionPeriod decimal = NULL,
    @TraineePeriodType NVARCHAR(50) = NULL,
	@NoOfChildren NVARCHAR(50) = NULL,
	@ESICNo NVARCHAR(50) = NULL,
	@PFNo NVARCHAR(50) = NULL,
    -- Contact Details
    @PresentAddress NVARCHAR(max) = NULL,
    @PresentTehsil NVARCHAR(100) = NULL,
    @PresentDistrict NVARCHAR(100) = NULL,
    @PresentCity NVARCHAR(100) = NULL,
    @PresentStateId INT = NULL,
    @PresentPincode NVARCHAR(20) = NULL,
    @PresentThanaId INT = NULL,
    @PermanentAddress NVARCHAR(max) = NULL,
    @PermanentTehsil NVARCHAR(100) = NULL,
    @PermanentDistrict NVARCHAR(100) = NULL,
    @PermanentCity NVARCHAR(100) = NULL,
    @PermanentStateId INT = NULL,
    @PermanentPincode NVARCHAR(20) = NULL,
    @PermanentThanaId INT = NULL,
    @CountryId INT = NULL,
    @WorkPhone NVARCHAR(20) = NULL,
    @PersonalPhone NVARCHAR(20) = NULL,
    @OfficialEmail NVARCHAR(100) = NULL,
    @Nationality NVARCHAR(50) = NULL,
    @ExtensionNo NVARCHAR(10) = NULL,
    @MobileNo NVARCHAR(20) = NULL,
    @SameAsPresentAddress BIT = 0,

    -- Salary Report
    @PrimaryPaymentMode NVARCHAR(50) = NULL,
    @PrimaryBankName NVARCHAR(max) = NULL,
    @PrimaryIFSCCode NVARCHAR(20) = NULL,
    @PrimaryAccountNumber NVARCHAR(50) = NULL,
    @PrimaryBankBranchName NVARCHAR(100) = NULL,
    @WagesTypes NVARCHAR(50) = NULL,
    @GroupJoiningDate DATETIME = NULL,
    @BusinessSegmentId INT = NULL,
    @EmployeeSalaryReport NVARCHAR(MAX) = NULL,
    @EmployeePFReport NVARCHAR(MAX) = NULL,
    @EmployeePTReport NVARCHAR(MAX) = NULL,
    @EmployeeTaxReport NVARCHAR(MAX) = NULL,
    @EmployeeESIReport NVARCHAR(MAX) = NULL,
    @EmployeeNamePrmaryBank NVARCHAR(100) = NULL,
    @IsMobileAccess BIT = 0, -- NEW
    @IsSelfiRequired BIT = 0, -- NEW
    @IsGeofencingRequired BIT = 0 -- NEW

AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'UPDATE'
        BEGIN

		--set probation Date
		Declare @ProbationEndDate dateTime;

		IF @ProbationCompletionPeriod IS NOT NULL or @ProbationCompletionPeriod<>0
		BEGIN
			IF @ProbationPeriodType = 'Months'
			BEGIN
				SET @ProbationEndDate = DATEADD(MONTH, @ProbationCompletionPeriod, @DateOfJoining);

			END
			ELSE
			BEGIN
				SET @ProbationEndDate = DATEADD(DAY, @ProbationCompletionPeriod, @DateOfJoining);

			END
		END



            UPDATE AspNetUsers
            SET
                FullName = @FullName,
                Email = @LoginAlias,
                UserName = @LoginAlias,
                --Password = @Password,
                Initial = @Initial,
                FirstName = @FirstName,
                MiddleName = @MiddleName,
                LastName = @LastName,
                EmployeeCode = @EmployeeCode,
                AlfaEmployeeCode = @AlfaEmployeeCode,
                AlfaCode = @AlfaCode,
                DateOfJoining = @DateOfJoining,
                BranchId = @BranchId,
                GradeId = @GradeId,
                ShiftMasterId = @ShiftMasterId,
                CTC = @CTC,
                DesignationId = @DesignationId,
                GrossSalary = @GrossSalary,
                CategoryId = @CategoryId,
                BasicSalary = @BasicSalary,
                DepartmentId = @DepartmentId,
                EmployeeTypeId = @EmployeeTypeId,
                DateOfBirth = @DateOfBirth,
                LoginAlias = @LoginAlias,
                ReportingManagerId = @ReportingManagerId,
                SubBranch = @SubBranch,
                EnrollNo = @EnrollNo,
                CompanyId = @CompanyId,
                Overtime = @Overtime,
                Latemark = @Latemark,
                Earlymark = @Earlymark,
                Fullpf = @Fullpf,
                Pt = @Pt,
                Fixsalary = @Fixsalary,
                Probation = @Probation,
                Trainee = @Trainee,
				IsPFApplicable=@IsPFApplicable,
                --EmployeeProfileUrl = @EmployeeProfileUrl,
                --EmployeeSignatureUrl = @EmployeeSignatureUrl,
                UpdatedDate = ISNULL(@UpdatedDate, GETUTCDATE()),
                UpdatedBy = @UpdatedBy,
                WeekOffDetailsId = @WeekOffDetailsId,
                IsPermissionPunchInOut = @IsPermissionPunchInOut,
				ProbationEndDate=@ProbationEndDate,
                IsMobileAccess = @IsMobileAccess, -- NEW
                IsSelfiRequired = @IsSelfiRequired, -- NEW
                IsGeofencingRequired = @IsGeofencingRequired, -- NEW
                -- Personal Info
                Gender = @Gender,
                PersonalEmailId = @PersonalEmailId,
                FatherName = @FatherName,
                MotherName = @MotherName,
                BloodGroup = @BloodGroup,
                Height = @Height,
                MaritalStatus = @MaritalStatus,
                MarriageDate = @MarriageDate,
                MarkIdentification = @MarkIdentification,
                Religion = @Religion,
                Caste = @Caste,
                CastCategory = @CastCategory,
                AadharCardNo = @AadharCardNo,
                PANNo = @PANNo,
                Dispensary = @Dispensary,
                DoctorName = @DoctorName,
                DispensaryAddress = @DispensaryAddress,
                UANNumber = @UANNumber,
                DrivingLicense = @DrivingLicense,
                DrivingLicenseExpiry = @DrivingLicenseExpiry,
                RationCardType = @RationCardType,
                RationCardNo = @RationCardNo,
                ProbationCompletionPeriod = @ProbationCompletionPeriod,
                ProbationPeriodType = @ProbationPeriodType,
                ManagerProbationId = @ManagerProbationId,
                ConfirmDate = @ConfirmDate,
                RetirementDate = @RetirementDate,
                OfferDate = @OfferDate,
                TraineeCompletionPeriod = @TraineeCompletionPeriod,
                TraineePeriodType = @TraineePeriodType,
				 NoOfChildren=   @NoOfChildren,
				 ESICNo=	@ESICNo ,
					PFNo= @PFNo ,


                -- Contact Details
                PresentAddress = @PresentAddress,
                PresentTehsil = @PresentTehsil,
                PresentDistrict = @PresentDistrict,
                PresentCity = @PresentCity,
                PresentStateId = @PresentStateId,
                PresentPincode = @PresentPincode,
                PresentThanaId = @PresentThanaId,
                PermanentAddress = @PermanentAddress,
                PermanentTehsil = @PermanentTehsil,
                PermanentDistrict = @PermanentDistrict,
                PermanentCity = @PermanentCity,
                PermanentStateId = @PermanentStateId,
                PermanentPincode = @PermanentPincode,
                PermanentThanaId = @PermanentThanaId,
                CountryId = @CountryId,
                WorkPhone = @WorkPhone,
                PersonalPhone = @PersonalPhone,
                OfficialEmail = @OfficialEmail,
                Nationality = @Nationality,
                ExtensionNo = @ExtensionNo,
                MobileNo = @MobileNo,
                SameAsPresentAddress = @SameAsPresentAddress,

                -- Salary Report
                PrimaryPaymentMode = @PrimaryPaymentMode,
                PrimaryBankName = @PrimaryBankName,
                PrimaryIFSCCode = @PrimaryIFSCCode,
                PrimaryAccountNumber = @PrimaryAccountNumber,
                PrimaryBankBranchName = @PrimaryBankBranchName,
                WagesTypes = @WagesTypes,
                GroupJoiningDate = @GroupJoiningDate,
                BusinessSegmentId = @BusinessSegmentId,
                EmployeeSalaryReport = @EmployeeSalaryReport,
                EmployeePFReport = @EmployeePFReport,
                EmployeePTReport = @EmployeePTReport,
                EmployeeTaxReport = @EmployeeTaxReport,
                EmployeeESIReport = @EmployeeESIReport,
                EmployeeNamePrmaryBank = @EmployeeNamePrmaryBank

            WHERE Id = @Id;



			--Update reporting persion
		DECLARE @LastReportingManagerId INT;

			SELECT TOP 1 @LastReportingManagerId = ReportingManagerId
			FROM ReportingManagerDetails
			WHERE EmployeeId = @Id and isdeleted=0 and isenabled=1
			ORDER BY ReportingManagerDetailsId DESC;

			IF (@ReportingManagerId <> @LastReportingManagerId)
			BEGIN
				INSERT INTO ReportingManagerDetails (
					EmployeeId,
					ReportingManagerId,
					EffectedDate,
					IsDeleted,
					IsEnabled,
					MethodName,
					CreatedDate,
					CreatedBy
				)
				VALUES (
					@Id,
					@ReportingManagerId,
					GETUTCDATE(),
					0,
					1,
					'In Person',
					GETUTCDATE(),
					@UpdatedBy
				);
			END;


            SELECT @Id AS Id;
        END

        ELSE IF @Action = 'DELETE'
        BEGIN
            UPDATE AspNetUsers
            SET
                IsDeleted = 1,
                IsEnabled = 0,
                DeletedDate = ISNULL(@DeletedDate, GETUTCDATE()),
                DeletedBy = @DeletedBy
            WHERE Id = @Id;

            SELECT @Id AS Id;
        END

        ELSE IF @Action = 'IsBlocked'
        BEGIN
            UPDATE AspNetUsers
            SET
                IsBlocked = @IsBlocked
            WHERE Id = @Id;

            SELECT @Id AS Id;
        END
    END TRY
    BEGIN CATCH
        SELECT NULL AS Id;
    END CATCH
END
GO


-- ============================================================================
-- 3) GetRecordsForUpdate — this is the REAL live read-path behind the
--    "Basic Info" tab of AdditionalInformation.cshtml (EmployeeManageRepository
--    .GetRecordsForUpdate). EmployeeDetailViewModel.cs was already updated
--    with matching IsMobileAccess/IsSelfiRequired/IsGeofencingRequired
--    properties — Dapper will bind them automatically once the SELECT
--    returns these 3 columns. Without this change the edit form's toggles
--    will always come back empty/unchecked when reopening an employee, even
--    though the values were saved correctly.
--    Change: added U.IsMobileAccess, U.IsSelfiRequired, U.IsGeofencingRequired
--    to the first SELECT list only (the employee row keyed by @EmployeeId).
-- ============================================================================

ALTER PROCEDURE [dbo].[GetRecordsForUpdate]
@CompanyId int =0,
@EmployeeId int=0
AS
BEGIN
    SET NOCOUNT ON;

---------------------------------------------------------------------
	--Employee
    SELECT
    U.Id,U.FullName,
	U.Initial, U.FirstName,
	U.MiddleName, U.LastName,
	U.EmployeeCode, U.AlfaCode,
	U.AlfaEmployeeCode,
            U.DateOfJoining,
			U.BranchId,
			U.GradeId,
			U.ShiftMasterId,
			U.CTC,
			U.DesignationId,
			U.GrossSalary,
			U.CategoryId,
            U.BasicSalary,
			U.DepartmentId,
			U.EmployeeTypeId,
			U.DateOfBirth,
			U.LoginAlias,
			U.ReportingManagerId,
			U.SubBranch,
            U.EnrollNo, U.CompanyId,
			U.Overtime,
			U.Latemark,
			U.Earlymark, U.Fullpf,
			U.Pt,
			U.Fixsalary, U.Probation,
			U.Trainee,
            U.IsPFApplicable,
			U.WeekOffDetailsId, U.IsPermissionPunchInOut,
			U.EmployeeSignatureUrl,
			U.EmployeeProfileUrl,
			U.AttendanceLimit,
			U.Remark,
			U.Password,
			U.IsMobileAccess, -- NEW
			U.IsSelfiRequired, -- NEW
			U.IsGeofencingRequired, -- NEW
	   CreatedBy.FullName AS CreatedByName,
	r.RoleId
    FROM AspNetUsers U
   left join HrmsUserRoles r on r.EmployeeId=U.Id and r.isdeleted=0 and r.isenabled=1
   left join aspnetusers CreatedBy on CreatedBy.Id=U.CreatedBy
    WHERE U.IsDeleted = 0 and u.Isenabled=1 and U.Id=@EmployeeId and U.CompanyId=@companyId
---------------------------------------------------------------------
	--Branch Details (Optimized with direct join instead of STRING_SPLIT)
    SELECT b.BranchId, b.BranchName, b.BranchCode
    FROM Branch b

    WHERE b.IsDeleted = 0
      AND b.IsEnabled = 1;

-----------------------------------------------
	--Grade Details
	Select GradeId,GradeName from Grade where isdeleted=0 and isenabled=1

--------------------------------------------------
--Shifts details
select ShiftId,ShiftName from  [dbo].[ShiftMaster] where Isdeleted=0 and isenabled=1 and Isactive=1

-------------------------------------------------------
--Designation Details
 select DesignationId,DesignationName from designation where Isdeleted=0 and isenabled=1
 --AND (
 --     CompanyId = @CompanyId
 --     OR (@CompanyId = 11 AND CompanyId = 9)
 -- );

--------------------------------------------------------
 -- Category Details
select CategoryId,CategoryName from Category where isdeleted=0 and isenabled=1

-------------------------------------------------------
---Department Details
select DepartmentId,DepartmentName from Department where isdeleted=0 and Isenabled=1
--AND (
--      CompanyId = @CompanyId
--      OR (@CompanyId = 11 AND CompanyId = 9)
--  );

-----------------------------------------------------------
--EmployeeType Details
select EmployeeTypeId,EmployeeTypeName from EmployeeType where isdeleted=0 and isenabled=1

------------------------------------------------------------------
--Role details
select Id as RoleId,Name as RoleName from aspnetroles where IsDeleted=0 and IsEnabled=1

------------------------------------------------------------------
--Reporting Details (Added ISNULL check for clarity)
select Id as EmployeeId,FullName,EmployeeCode,
(FullName+' - '+EmployeeCode)as EmployeeName
from aspnetusers
where Isdeleted=0 and isenabled=1 and isnull(isleft,0)=0
order by EmployeeCode
END
GO


-- ============================================================================
-- 4) GetAllEmployeeAndLocation — the "Location Assign to Employee" picker's
--    employee list.
--
--    SUPERSEDED — corrected in
--    PENDING_APPLY_GetAllEmployeeAndLocation_MobileAccessOnly.sql. The
--    original change here required IsMobileAccess=1 AND
--    IsGeofencingRequired=1, which is backwards: Geo Fencing gets turned on
--    BY assigning a zone here, so requiring it up front meant nobody could
--    ever show up in the picker. Correct rule is IsMobileAccess=1 alone
--    (matches the C# filter in
--    HRMS_Infrastructure/Repository/OtherMaster/GeoLocationRepository.cs ->
--    GetAllEmployeeAndLocation()). Kept as IsMobileAccess-only below so
--    re-running this whole combined file can't silently revert that fix
--    again.
-- ============================================================================

ALTER proc GetAllEmployeeAndLocation
@CompanyId int
as
begin

select Id ,EmployeeCode,FullName from aspnetusers where isdeleted=0 and isEnabled=1
and isnull(isleft,0) =0
and CompanyId=@CompanyId
and isnull(IsMobileAccess,0)=1
----------------------------
select
  g.*,
  b.BranchName as GeoLocationName
  from GeoLocation g
  left join Branch b on b.BranchId=g.BranchId
  where g.isdeleted=0 and g.isEnabled=1
  and g.CompanyId=@CompanyId

end
GO
