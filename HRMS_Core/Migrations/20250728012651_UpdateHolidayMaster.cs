using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHolidayMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HolidayMaster_Branch_BranchId",
                table: "HolidayMaster");

            migrationBuilder.DropIndex(
                name: "IX_HolidayMaster_BranchId",
                table: "HolidayMaster");

            migrationBuilder.DropColumn(
                name: "ApprovalMaxLimit",
                table: "HolidayMaster");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "HolidayMaster");

            migrationBuilder.DropColumn(
                name: "HalfDay",
                table: "HolidayMaster");

            migrationBuilder.DropColumn(
                name: "OptionalHoliday",
                table: "HolidayMaster");

            migrationBuilder.DropColumn(
                name: "PresentCompulsory",
                table: "HolidayMaster");

            migrationBuilder.DropColumn(
                name: "SMS",
                table: "HolidayMaster");

            migrationBuilder.DropColumn(
                name: "State",
                table: "HolidayMaster");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ToDate",
                table: "HolidayMaster",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "RepeatAnnually",
                table: "HolidayMaster",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "MultipleHoliday",
                table: "HolidayMaster",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FromDate",
                table: "HolidayMaster",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "HolidayMaster",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "HolidayMaster",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "HolidayMaster");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "HolidayMaster");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ToDate",
                table: "HolidayMaster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "RepeatAnnually",
                table: "HolidayMaster",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "MultipleHoliday",
                table: "HolidayMaster",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FromDate",
                table: "HolidayMaster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalMaxLimit",
                table: "HolidayMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "HolidayMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HalfDay",
                table: "HolidayMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OptionalHoliday",
                table: "HolidayMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PresentCompulsory",
                table: "HolidayMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SMS",
                table: "HolidayMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "HolidayMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HolidayMaster_BranchId",
                table: "HolidayMaster",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_HolidayMaster_Branch_BranchId",
                table: "HolidayMaster",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
