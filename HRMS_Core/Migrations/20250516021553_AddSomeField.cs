using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmployeeCodeSetting",
                table: "CompanyDetails",
                newName: "TdsDeductor");

            migrationBuilder.AddColumn<bool>(
                name: "AlphaNumericCode",
                table: "CompanyDetails",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CitAddress",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CitCity",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CitPin",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfFactorySetup",
                table: "CompanyDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DigitsForEmployeeCode",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FactoryLicenseNo",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FactoryLicenseOffice",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FactoryRegistrationNo",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FactoryType",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GstTravelExpenses",
                table: "CompanyDetails",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HrManager",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HrManagerDesignation",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "IssueDate",
                table: "CompanyDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerDesignation",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaxEmployeeCode",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NatureOfBusiness",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SampleCode",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlphaNumericCode",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "CitAddress",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "CitCity",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "CitPin",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "DateOfFactorySetup",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "DigitsForEmployeeCode",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "FactoryLicenseNo",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "FactoryLicenseOffice",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "FactoryRegistrationNo",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "FactoryType",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "FatherName",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "GstTravelExpenses",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "HrManager",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "HrManagerDesignation",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "IssueDate",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "ManagerDesignation",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "MaxEmployeeCode",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "NatureOfBusiness",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "SampleCode",
                table: "CompanyDetails");

            migrationBuilder.RenameColumn(
                name: "TdsDeductor",
                table: "CompanyDetails",
                newName: "EmployeeCodeSetting");
        }
    }
}
