using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class updateBranch_tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branch_City_CityId",
                table: "Branch");

            migrationBuilder.DropIndex(
                name: "IX_Branch_CityId",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Branch");

            migrationBuilder.AddColumn<string>(
                name: "CityName",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityName",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "CountryName",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Branch");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Branch",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branch_CityId",
                table: "Branch",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_City_CityId",
                table: "Branch",
                column: "CityId",
                principalTable: "City",
                principalColumn: "CityID");
        }
    }
}
