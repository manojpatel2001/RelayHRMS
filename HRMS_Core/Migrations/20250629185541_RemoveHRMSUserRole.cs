using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveHRMSUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "AspNetUserRoles",
                type: "int",
                nullable: true);

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "AspNetUserRoles");
        }
    }
}
