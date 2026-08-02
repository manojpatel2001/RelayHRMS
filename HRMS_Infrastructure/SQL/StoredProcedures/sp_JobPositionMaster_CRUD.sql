-- CRUD for JobPositionMaster, called from JobPositionRepository via Dapper,
-- discriminated by @Operation. Mirrors sp_EmployeeRecruitmentDetails_CRUD.sql.

CREATE OR ALTER PROCEDURE [dbo].[sp_JobPositionMaster_CRUD]
    @Operation VARCHAR(10),
    @JobPositionId INT = NULL,
    @ManpowerRequisitionId INT = NULL,
    @PositionTitle VARCHAR(200) = NULL,
    @DesignationId INT = NULL,
    @DepartmentId INT = NULL,
    @BranchId INT = NULL,
    @CompanyId INT = NULL,
    @GradeId INT = NULL,
    @EmploymentType VARCHAR(50) = NULL,
    @VacancyCount INT = 1,
    @MinExperienceYears DECIMAL(4,1) = NULL,
    @MaxExperienceYears DECIMAL(4,1) = NULL,
    @MinEducationLevel VARCHAR(50) = NULL,
    @RequiredSkills VARCHAR(MAX) = NULL,
    @PreferredSkills VARCHAR(MAX) = NULL,
    @JobDescription VARCHAR(MAX) = NULL,
    @MinBudget DECIMAL(18,2) = NULL,
    @MaxBudget DECIMAL(18,2) = NULL,
    @Priority VARCHAR(10) = NULL,
    @OwnerRecruiterId INT = NULL,
    @ReportingManagerId INT = NULL,
    @Status VARCHAR(20) = NULL,
    @TargetClosureDate DATE = NULL,
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
            INSERT INTO [dbo].[JobPositionMaster]
            (ManpowerRequisitionId, PositionTitle, DesignationId, DepartmentId, BranchId, CompanyId, GradeId,
             EmploymentType, VacancyCount, MinExperienceYears, MaxExperienceYears, MinEducationLevel,
             RequiredSkills, PreferredSkills, JobDescription, MinBudget, MaxBudget, Priority,
             OwnerRecruiterId, ReportingManagerId, Status, TargetClosureDate, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@ManpowerRequisitionId, @PositionTitle, @DesignationId, @DepartmentId, @BranchId, @CompanyId, @GradeId,
             @EmploymentType, @VacancyCount, @MinExperienceYears, @MaxExperienceYears, @MinEducationLevel,
             @RequiredSkills, @PreferredSkills, @JobDescription, @MinBudget, @MaxBudget, @Priority,
             @OwnerRecruiterId, @ReportingManagerId, ISNULL(@Status, 'Open'), @TargetClosureDate, @IsEnabled, GETDATE(), @CreatedBy);

            SELECT SCOPE_IDENTITY() AS NewId, 1 AS Success, 'Job position created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[JobPositionMaster]
            SET
                PositionTitle = ISNULL(@PositionTitle, PositionTitle),
                DesignationId = @DesignationId,
                DepartmentId = @DepartmentId,
                BranchId = @BranchId,
                GradeId = @GradeId,
                EmploymentType = @EmploymentType,
                VacancyCount = ISNULL(@VacancyCount, VacancyCount),
                MinExperienceYears = @MinExperienceYears,
                MaxExperienceYears = @MaxExperienceYears,
                MinEducationLevel = @MinEducationLevel,
                RequiredSkills = @RequiredSkills,
                PreferredSkills = @PreferredSkills,
                JobDescription = @JobDescription,
                MinBudget = @MinBudget,
                MaxBudget = @MaxBudget,
                Priority = @Priority,
                OwnerRecruiterId = @OwnerRecruiterId,
                ReportingManagerId = @ReportingManagerId,
                Status = ISNULL(@Status, Status),
                TargetClosureDate = @TargetClosureDate,
                IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(),
                UpdatedBy = @UpdatedBy
            WHERE JobPositionId = @JobPositionId;

            SELECT 1 AS Success, 'Job position updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[JobPositionMaster]
            SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy
            WHERE JobPositionId = @JobPositionId;

            SELECT 1 AS Success, 'Job position deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, ERROR_MESSAGE() AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetAllJobPositionMaster]
    @CompanyId INT = NULL,
    @BranchId INT = NULL,
    @Status VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT jp.*, d.DesignationName, dep.DepartmentName, b.BranchName, g.GradeName
    FROM [dbo].[JobPositionMaster] jp
    LEFT JOIN [dbo].[Designation] d ON d.DesignationId = jp.DesignationId
    LEFT JOIN [dbo].[Department] dep ON dep.DepartmentId = jp.DepartmentId
    LEFT JOIN [dbo].[Branch] b ON b.BranchId = jp.BranchId
    LEFT JOIN [dbo].[Grade] g ON g.GradeId = jp.GradeId
    WHERE jp.IsDeleted = 0
      AND (@CompanyId IS NULL OR jp.CompanyId = @CompanyId)
      AND (@BranchId IS NULL OR jp.BranchId = @BranchId)
      AND (@Status IS NULL OR jp.Status = @Status)
    ORDER BY jp.CreatedDate DESC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetJobPositionMasterById]
    @JobPositionId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT jp.*, d.DesignationName, dep.DepartmentName, b.BranchName, g.GradeName
    FROM [dbo].[JobPositionMaster] jp
    LEFT JOIN [dbo].[Designation] d ON d.DesignationId = jp.DesignationId
    LEFT JOIN [dbo].[Department] dep ON dep.DepartmentId = jp.DepartmentId
    LEFT JOIN [dbo].[Branch] b ON b.BranchId = jp.BranchId
    LEFT JOIN [dbo].[Grade] g ON g.GradeId = jp.GradeId
    WHERE jp.JobPositionId = @JobPositionId AND jp.IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetJobPositionsByManpowerRequisitionId]
    @ManpowerRequisitionId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[JobPositionMaster]
    WHERE ManpowerRequisitionId = @ManpowerRequisitionId AND IsDeleted = 0
    ORDER BY CreatedDate DESC;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetJobPositionDropdown]
    @CompanyId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT JobPositionId, PositionTitle, Status
    FROM [dbo].[JobPositionMaster]
    WHERE IsDeleted = 0 AND IsEnabled = 1 AND (@CompanyId IS NULL OR CompanyId = @CompanyId)
    ORDER BY PositionTitle;
END
GO
