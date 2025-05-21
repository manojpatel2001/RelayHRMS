using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Grade",
                table: "AspNetUsers",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "EmployeeStringCode",
                table: "AspNetUsers",
                newName: "SubBranch");

            migrationBuilder.RenameColumn(
                name: "EmployeeNumberCode",
                table: "AspNetUsers",
                newName: "ReportingManager");

            migrationBuilder.RenameColumn(
                name: "Designation",
                table: "AspNetUsers",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "Department",
                table: "AspNetUsers",
                newName: "EnrollNo");

            migrationBuilder.RenameColumn(
                name: "Branch",
                table: "AspNetUsers",
                newName: "EmployeeSignatureUrl");

            migrationBuilder.AlterColumn<int>(
                name: "UserPrivilege",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DesignationId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeProfileUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GradeId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BranchId",
                table: "AspNetUsers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DesignationId",
                table: "AspNetUsers",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GradeId",
                table: "AspNetUsers",
                column: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Branch_BranchId",
                table: "AspNetUsers",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Department_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Designation_DesignationId",
                table: "AspNetUsers",
                column: "DesignationId",
                principalTable: "Designation",
                principalColumn: "DesignationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Grade_GradeId",
                table: "AspNetUsers",
                column: "GradeId",
                principalTable: "Grade",
                principalColumn: "GradeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Branch_BranchId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Department_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Designation_DesignationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Grade_GradeId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BranchId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DesignationId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GradeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeProfileUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "AspNetUsers",
                newName: "Grade");

            migrationBuilder.RenameColumn(
                name: "SubBranch",
                table: "AspNetUsers",
                newName: "EmployeeStringCode");

            migrationBuilder.RenameColumn(
                name: "ReportingManager",
                table: "AspNetUsers",
                newName: "EmployeeNumberCode");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "AspNetUsers",
                newName: "Designation");

            migrationBuilder.RenameColumn(
                name: "EnrollNo",
                table: "AspNetUsers",
                newName: "Department");

            migrationBuilder.RenameColumn(
                name: "EmployeeSignatureUrl",
                table: "AspNetUsers",
                newName: "Branch");

            migrationBuilder.AlterColumn<string>(
                name: "UserPrivilege",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
