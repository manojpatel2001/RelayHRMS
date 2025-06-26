using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeInOutUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeInOutRecord_AspNetUsers_Emp_Id",
                table: "EmployeeInOutRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeInOutRecord_CompanyDetails_Com_Id",
                table: "EmployeeInOutRecord");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeInOutRecord_Com_Id",
                table: "EmployeeInOutRecord");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeInOutRecord_Emp_Id",
                table: "EmployeeInOutRecord");

            migrationBuilder.AlterColumn<int>(
                name: "Emp_Id",
                table: "EmployeeInOutRecord",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Emp_Id",
                table: "EmployeeInOutRecord",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInOutRecord_Com_Id",
                table: "EmployeeInOutRecord",
                column: "Com_Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInOutRecord_Emp_Id",
                table: "EmployeeInOutRecord",
                column: "Emp_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeInOutRecord_AspNetUsers_Emp_Id",
                table: "EmployeeInOutRecord",
                column: "Emp_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeInOutRecord_CompanyDetails_Com_Id",
                table: "EmployeeInOutRecord",
                column: "Com_Id",
                principalTable: "CompanyDetails",
                principalColumn: "CompanyId");
        }
    }
}
