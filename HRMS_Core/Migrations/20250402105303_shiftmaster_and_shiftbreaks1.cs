using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class shiftmaster_and_shiftbreaks1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ShiftMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ShiftMaster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ShiftMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ShiftMaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "ShiftMaster",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ShiftMaster",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "ShiftMaster",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ShiftMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ShiftMaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ShiftBreak",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ShiftBreak",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ShiftBreak",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ShiftBreak",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "ShiftBreak",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ShiftBreak",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "ShiftBreak",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ShiftBreak",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ShiftBreak",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ShiftMaster");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ShiftMaster");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ShiftMaster");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ShiftMaster");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "ShiftMaster");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ShiftMaster");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "ShiftMaster");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ShiftMaster");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ShiftMaster");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ShiftBreak");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ShiftBreak");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ShiftBreak");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ShiftBreak");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "ShiftBreak");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ShiftBreak");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "ShiftBreak");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ShiftBreak");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ShiftBreak");
        }
    }
}
