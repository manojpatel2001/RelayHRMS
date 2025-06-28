using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class addmonthandyear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Earning",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Earning",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Deduction",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Deduction",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "Earning");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Earning");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "Deduction");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Deduction");
        }
    }
}
