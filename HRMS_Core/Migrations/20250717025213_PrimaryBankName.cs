using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class PrimaryBankName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryBankId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryBankName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryBankName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "PrimaryBankId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
