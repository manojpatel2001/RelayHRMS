using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class updategredetbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sorting_No",
                table: "Grade",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sorting_No",
                table: "Grade");
        }
    }
}
