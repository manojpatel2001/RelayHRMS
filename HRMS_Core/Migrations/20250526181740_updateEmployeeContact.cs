using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class updateEmployeeContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "EmployeeContact");

            migrationBuilder.DropColumn(
                name: "PermanentThana",
                table: "EmployeeContact");

            migrationBuilder.DropColumn(
                name: "PresentThana",
                table: "EmployeeContact");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "EmployeeContact",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PermanentThanaId",
                table: "EmployeeContact",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PresentThanaId",
                table: "EmployeeContact",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContact_CountryId",
                table: "EmployeeContact",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContact_PermanentThanaId",
                table: "EmployeeContact",
                column: "PermanentThanaId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContact_PresentThanaId",
                table: "EmployeeContact",
                column: "PresentThanaId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeContact_Country_CountryId",
                table: "EmployeeContact",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeContact_Thana_PermanentThanaId",
                table: "EmployeeContact",
                column: "PermanentThanaId",
                principalTable: "Thana",
                principalColumn: "ThanaId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeContact_Thana_PresentThanaId",
                table: "EmployeeContact",
                column: "PresentThanaId",
                principalTable: "Thana",
                principalColumn: "ThanaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeContact_Country_CountryId",
                table: "EmployeeContact");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeContact_Thana_PermanentThanaId",
                table: "EmployeeContact");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeContact_Thana_PresentThanaId",
                table: "EmployeeContact");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeContact_CountryId",
                table: "EmployeeContact");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeContact_PermanentThanaId",
                table: "EmployeeContact");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeContact_PresentThanaId",
                table: "EmployeeContact");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "EmployeeContact");

            migrationBuilder.DropColumn(
                name: "PermanentThanaId",
                table: "EmployeeContact");

            migrationBuilder.DropColumn(
                name: "PresentThanaId",
                table: "EmployeeContact");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "EmployeeContact",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentThana",
                table: "EmployeeContact",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresentThana",
                table: "EmployeeContact",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
