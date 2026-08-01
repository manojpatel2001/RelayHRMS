-- Combined INSERT/UPDATE/DELETE/GET procedure for EmployeeCertification,
-- called from HRMS_Infrastructure\Repository\Employee\EmployeeCertificationRepository.cs
-- via Dapper, discriminated by @Operation. Mirrors sp_NewsAnnouncement_CRUD.sql.

CREATE OR ALTER PROCEDURE [dbo].[sp_EmployeeCertification_CRUD]
    @Operation VARCHAR(10),
    @EmployeeCertificationId INT = NULL,
    @EmployeeId INT = NULL,
    @CertificationName VARCHAR(200) = NULL,
    @IssuingBody VARCHAR(200) = NULL,
    @IssueDate DATE = NULL,
    @ExpiryDate DATE = NULL,
    @CertificateUrl VARCHAR(500) = NULL,
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
            INSERT INTO [dbo].[EmployeeCertification]
            (EmployeeId, CertificationName, IssuingBody, IssueDate, ExpiryDate, CertificateUrl, IsEnabled, CreatedDate, CreatedBy)
            VALUES
            (@EmployeeId, @CertificationName, @IssuingBody, @IssueDate, @ExpiryDate, @CertificateUrl, @IsEnabled, GETDATE(), @CreatedBy);

            SELECT 1 AS Success, 'Certification created successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'UPDATE'
        BEGIN
            UPDATE [dbo].[EmployeeCertification]
            SET
                CertificationName = ISNULL(@CertificationName, CertificationName),
                IssuingBody = ISNULL(@IssuingBody, IssuingBody),
                IssueDate = ISNULL(@IssueDate, IssueDate),
                ExpiryDate = ISNULL(@ExpiryDate, ExpiryDate),
                CertificateUrl = ISNULL(@CertificateUrl, CertificateUrl),
                IsEnabled = ISNULL(@IsEnabled, IsEnabled),
                UpdatedDate = GETDATE(),
                UpdatedBy = @UpdatedBy
            WHERE EmployeeCertificationId = @EmployeeCertificationId;

            SELECT 1 AS Success, 'Certification updated successfully!' AS ResponseMessage;
        END
        ELSE IF @Operation = 'DELETE'
        BEGIN
            UPDATE [dbo].[EmployeeCertification]
            SET IsDeleted = 1, IsEnabled = 0, DeletedDate = GETDATE(), DeletedBy = @DeletedBy
            WHERE EmployeeCertificationId = @EmployeeCertificationId;

            SELECT 1 AS Success, 'Certification deleted successfully!' AS ResponseMessage;
        END
    END TRY
    BEGIN CATCH
        SELECT -1 AS Success, 'Something went wrong!' AS ResponseMessage;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE [dbo].[GetEmployeeCertificationByEmployeeId]
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[EmployeeCertification]
    WHERE EmployeeId = @EmployeeId AND IsDeleted = 0
    ORDER BY IssueDate DESC;
END
