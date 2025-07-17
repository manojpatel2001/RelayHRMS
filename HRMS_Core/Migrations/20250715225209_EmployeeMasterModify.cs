using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeMasterModify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AadharCardNo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BloodGroup",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BusinessSegmentId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CastCategory",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Caste",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dispensary",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DispensaryAddress",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DrivingLicense",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DrivingLicenseExpiry",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeESIReport",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeNamePrmaryBank",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeePFReport",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeePTReport",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeSalaryReport",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeTaxReport",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtensionNo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GroupJoiningDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerProbationId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaritalStatus",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarkIdentification",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MarriageDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobileNo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OfferDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficialEmail",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PANNo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentAddress",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PermanentCityId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentDistrict",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentPincode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PermanentStateId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentTehsil",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PermanentThanaId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalEmailId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalPhone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresentAddress",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PresentCityId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresentDistrict",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresentPincode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PresentStateId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresentTehsil",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PresentThanaId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryAccountNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryBankBranchName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrimaryBankId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryIFSCCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryPaymentMode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProbationCompletionPeriod",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProbationPeriodType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RationCardNo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RationCardType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Religion",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RetirementDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SameAsPresentAddress",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TraineeCompletionPeriod",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TraineePeriodType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UANNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WagesTypes",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkPhone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AadharCardNo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BloodGroup",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BusinessSegmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CastCategory",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Caste",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ConfirmDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Dispensary",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DispensaryAddress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DoctorName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DrivingLicense",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DrivingLicenseExpiry",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeESIReport",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeNamePrmaryBank",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeePFReport",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeePTReport",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeSalaryReport",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeTaxReport",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ExtensionNo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FatherName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GroupJoiningDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ManagerProbationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MarkIdentification",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MarriageDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MobileNo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MotherName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OfferDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OfficialEmail",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PANNo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PermanentAddress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PermanentCityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PermanentDistrict",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PermanentPincode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PermanentStateId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PermanentTehsil",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PermanentThanaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PersonalEmailId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PersonalPhone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PresentAddress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PresentCityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PresentDistrict",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PresentPincode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PresentStateId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PresentTehsil",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PresentThanaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PrimaryAccountNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PrimaryBankBranchName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PrimaryBankId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PrimaryIFSCCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PrimaryPaymentMode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProbationCompletionPeriod",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProbationPeriodType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RationCardNo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RationCardType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Religion",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RetirementDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SameAsPresentAddress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TraineeCompletionPeriod",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TraineePeriodType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UANNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WagesTypes",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WorkPhone",
                table: "AspNetUsers");
        }
    }
}
