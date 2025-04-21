using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "City",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "City");
        }
    }
}
