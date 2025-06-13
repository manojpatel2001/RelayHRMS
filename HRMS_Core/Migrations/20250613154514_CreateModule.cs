using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class CreateModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModuleDetailsId",
                table: "PageMaster",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ModuleDetails",
                columns: table => new
                {
                    ModuleDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: true),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleDetails", x => x.ModuleDetailsId);
                    table.ForeignKey(
                        name: "FK_ModuleDetails_CompanyDetails_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyDetails",
                        principalColumn: "CompanyId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PageMaster_ModuleDetailsId",
                table: "PageMaster",
                column: "ModuleDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleDetails_CompanyId",
                table: "ModuleDetails",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PageMaster_ModuleDetails_ModuleDetailsId",
                table: "PageMaster",
                column: "ModuleDetailsId",
                principalTable: "ModuleDetails",
                principalColumn: "ModuleDetailsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageMaster_ModuleDetails_ModuleDetailsId",
                table: "PageMaster");

            migrationBuilder.DropTable(
                name: "ModuleDetails");

            migrationBuilder.DropIndex(
                name: "IX_PageMaster_ModuleDetailsId",
                table: "PageMaster");

            migrationBuilder.DropColumn(
                name: "ModuleDetailsId",
                table: "PageMaster");
        }
    }
}
