using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeeBaseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EmployeeType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EmployeeType",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "EmployeeType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "EmployeeType",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "EmployeeType",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmployeeType",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "EmployeeType",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "EmployeeType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "EmployeeType",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeeType");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EmployeeType");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "EmployeeType");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "EmployeeType");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "EmployeeType");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EmployeeType");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "EmployeeType");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "EmployeeType");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "EmployeeType");
        }
    }
}
