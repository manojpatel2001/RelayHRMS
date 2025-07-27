using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class modify_department_city_companydetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyDetails_City_CityId",
                table: "CompanyDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyDetails_State_StateId",
                table: "CompanyDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompanyDetails_CityId",
                table: "CompanyDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompanyDetails_StateId",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "MinimumWages",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "OJTApplicable",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "CensusNumber",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "CityName",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "ContractorBranch",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "ESICNo",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "PFNo",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "RegistrationCertificateNo",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "SalaryStartDate",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "WardNumber",
                table: "Branch");

            migrationBuilder.RenameColumn(
                name: "SortingNo",
                table: "Department",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Department",
                newName: "DepartmentCode");

            migrationBuilder.RenameColumn(
                name: "Zone",
                table: "Branch",
                newName: "GSTIN_No");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Department",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BranchIds",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Branch",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Branch",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "Branch",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "BranchIds",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Branch");

            migrationBuilder.RenameColumn(
                name: "DepartmentCode",
                table: "Department",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Department",
                newName: "SortingNo");

            migrationBuilder.RenameColumn(
                name: "GSTIN_No",
                table: "Branch",
                newName: "Zone");

            migrationBuilder.AddColumn<int>(
                name: "MinimumWages",
                table: "Department",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OJTApplicable",
                table: "Department",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CensusNumber",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityName",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ContractorBranch",
                table: "Branch",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ESICNo",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PFNo",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationCertificateNo",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SalaryStartDate",
                table: "Branch",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WardNumber",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDetails_CityId",
                table: "CompanyDetails",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDetails_StateId",
                table: "CompanyDetails",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyDetails_City_CityId",
                table: "CompanyDetails",
                column: "CityId",
                principalTable: "City",
                principalColumn: "CityID");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyDetails_State_StateId",
                table: "CompanyDetails",
                column: "StateId",
                principalTable: "State",
                principalColumn: "StateId");
        }
    }
}
