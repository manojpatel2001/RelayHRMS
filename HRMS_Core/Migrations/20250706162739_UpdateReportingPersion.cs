using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReportingPersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportingManager",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "ReportingManagerId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportingManagerId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ReportingManager",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
