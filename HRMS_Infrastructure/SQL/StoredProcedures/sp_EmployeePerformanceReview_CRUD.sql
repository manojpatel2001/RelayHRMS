-- Combined INSERT/UPDATE/DELETE/GET procedure for EmployeePerformanceReview,
-- called from HRMS_Infrastructure\Repository\Employee\EmployeePerformanceReviewRepository.cs
-- via Dapper, discriminated by @Operation. Mirrors sp_NewsAnnouncement_CRUD.sql.

CREATE OR ALTER PROCEDURE [dbo].[sp_EmployeePerformanceReview_CRUD]
    @Operation VARCHAR(10),
    @EmployeePerformanceReviewId INT = NULL,
    @EmployeeId INT = NULL,
    @ReviewPeriodStart DATE = NULL,
    @ReviewPeriodEnd DATE = NULL,
    @ReviewDate DATE = NULL,
    @ReviewerId INT = NULL,
    @Rating INT = NULL,
    @Strengths VARCHAR(1000) = NULL,
    @AreasOfImprovement VARCHAR(1000) = NULL,
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
            INSERT INTO [dbo].[EmployeePerformanceReview]
            (EmployeeId, ReviewPeriodStart, ReviewPeriodEnd, ReviewDate, ReviewerId, Rating, Strengths, AreasOfImprovement, Status, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@EmployeeId, @ReviewPeriodStart, @ReviewPeriodEnd, @ReviewDate, @ReviewerId, @Rating, @Strengths, @AreasOfImprovement, ISNULL(@Status, 'Draft'), @IsEnabled, GETDATE(), @CreatedBy);

            SELECT 1 AS Success, 'Performance review created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[EmployeePerformanceReview]
            SET
                ReviewPeriodStart = ISNULL(@ReviewPeriodStart, ReviewPeriodStart),
                ReviewPeriodEnd = ISNULL(@ReviewPeriodEnd, ReviewPeriodEnd),
                ReviewDate = ISNULL(@ReviewDate, ReviewDate),
                ReviewerId = ISNULL(@ReviewerId, ReviewerId),
                Rating = ISNULL(@Rating, Rating),
                Strengths = ISNULL(@Strengths, Strengths),
                AreasOfImprovement = ISNULL(@AreasOfImprovement, AreasOfImprovement),
                Status = ISNULL(@Status, Status),
                IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(),
                UpdatedBy = @UpdatedBy
            WHERE EmployeePerformanceReviewId = @EmployeePerformanceReviewId;

            SELECT 1 AS Success, 'Performance review updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[EmployeePerformanceReview]
            SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy
            WHERE EmployeePerformanceReviewId = @EmployeePerformanceReviewId;

            SELECT 1 AS Success, 'Performance review deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, 'Something went wrong!' AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetEmployeePerformanceReviewByEmployeeId]
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[EmployeePerformanceReview]
    WHERE EmployeeId = @EmployeeId AND IsDeleted = 0
    ORDER BY ReviewDate DESC;
END
