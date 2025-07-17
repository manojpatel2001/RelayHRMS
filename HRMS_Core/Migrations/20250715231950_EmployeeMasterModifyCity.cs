using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeMasterModifyCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermanentCityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PresentCityId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "PermanentCity",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresentCity",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermanentCity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PresentCity",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "PermanentCityId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PresentCityId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
