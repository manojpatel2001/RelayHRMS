-- Combined INSERT/UPDATE/DELETE/GET procedure for EmployeeRecruitmentDetails,
-- called from HRMS_Infrastructure\Repository\Employee\EmployeeRecruitmentDetailsRepository.cs
-- via Dapper, discriminated by @Operation. Mirrors sp_NewsAnnouncement_CRUD.sql.

CREATE OR ALTER PROCEDURE [dbo].[sp_EmployeeRecruitmentDetails_CRUD]
    @Operation VARCHAR(10),
    @EmployeeRecruitmentDetailsId INT = NULL,
    @EmployeeId INT = NULL,
    @ApplicationDate DATE = NULL,
    @InterviewDate DATE = NULL,
    @InterviewerName VARCHAR(200) = NULL,
    @Source VARCHAR(50) = NULL,
    @OfferDate DATE = NULL,
    @OfferAcceptedDate DATE = NULL,
    @Remarks VARCHAR(500) = NULL,
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
            INSERT INTO [dbo].[EmployeeRecruitmentDetails]
            (EmployeeId, ApplicationDate, InterviewDate, InterviewerName, Source, OfferDate, OfferAcceptedDate, Remarks, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@EmployeeId, @ApplicationDate, @InterviewDate, @InterviewerName, @Source, @OfferDate, @OfferAcceptedDate, @Remarks, @IsEnabled, GETDATE(), @CreatedBy);

            SELECT 1 AS Success, 'Recruitment details created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[EmployeeRecruitmentDetails]
            SET
                ApplicationDate = ISNULL(@ApplicationDate, ApplicationDate),
                InterviewDate = ISNULL(@InterviewDate, InterviewDate),
                InterviewerName = ISNULL(@InterviewerName, InterviewerName),
                Source = ISNULL(@Source, Source),
                OfferDate = ISNULL(@OfferDate, OfferDate),
                OfferAcceptedDate = ISNULL(@OfferAcceptedDate, OfferAcceptedDate),
                Remarks = ISNULL(@Remarks, Remarks),
                IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(),
                UpdatedBy = @UpdatedBy
            WHERE EmployeeRecruitmentDetailsId = @EmployeeRecruitmentDetailsId;

            SELECT 1 AS Success, 'Recruitment details updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[EmployeeRecruitmentDetails]
            SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy
            WHERE EmployeeRecruitmentDetailsId = @EmployeeRecruitmentDetailsId;

            SELECT 1 AS Success, 'Recruitment details deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, 'Something went wrong!' AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetEmployeeRecruitmentDetailsByEmployeeId]
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[EmployeeRecruitmentDetails]
    WHERE EmployeeId = @EmployeeId AND IsDeleted = 0
    ORDER BY ApplicationDate DESC;
END
