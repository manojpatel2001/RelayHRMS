using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class AddfieldInCompanydetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDate",
                table: "CompanyDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisplayOnLogin",
                table: "CompanyDetails",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LetterHeadFooterUrl",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LetterHeadHeaderUrl",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EffectiveDate",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "IsDisplayOnLogin",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "LetterHeadFooterUrl",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "LetterHeadHeaderUrl",
                table: "CompanyDetails");
        }
    }
}
