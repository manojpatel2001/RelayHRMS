using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class CreateTable_PrivilegeMaster_PageMaster_PrivilegeDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PageMaster",
                columns: table => new
                {
                    PageMasterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AliasPageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnderPageMasterId = table.Column<int>(type: "int", nullable: true),
                    PageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_PageMaster", x => x.PageMasterId);
                });

            migrationBuilder.CreateTable(
                name: "PrivilegeMaster",
                columns: table => new
                {
                    PrivilegeMasterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrivilegeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    BranchId_Multi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId_Multi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerticalId_Multi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivilegeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_PrivilegeMaster", x => x.PrivilegeMasterId);
                    table.ForeignKey(
                        name: "FK_PrivilegeMaster_CompanyDetails_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyDetails",
                        principalColumn: "CompanyId");
                });

            migrationBuilder.CreateTable(
                name: "PrivilegeDetails",
                columns: table => new
                {
                    PrivilegeDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrivilegeMasterId = table.Column<int>(type: "int", nullable: true),
                    PageId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    Is_View = table.Column<bool>(type: "bit", nullable: true),
                    Is_Edit = table.Column<bool>(type: "bit", nullable: true),
                    Is_Save = table.Column<bool>(type: "bit", nullable: true),
                    Is_Delete = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_PrivilegeDetails", x => x.PrivilegeDetailsId);
                    table.ForeignKey(
                        name: "FK_PrivilegeDetails_CompanyDetails_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyDetails",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_PrivilegeDetails_PageMaster_PageId",
                        column: x => x.PageId,
                        principalTable: "PageMaster",
                        principalColumn: "PageMasterId");
                    table.ForeignKey(
                        name: "FK_PrivilegeDetails_PrivilegeMaster_PrivilegeMasterId",
                        column: x => x.PrivilegeMasterId,
                        principalTable: "PrivilegeMaster",
                        principalColumn: "PrivilegeMasterId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegeDetails_CompanyId",
                table: "PrivilegeDetails",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegeDetails_PageId",
                table: "PrivilegeDetails",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegeDetails_PrivilegeMasterId",
                table: "PrivilegeDetails",
                column: "PrivilegeMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegeMaster_CompanyId",
                table: "PrivilegeMaster",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivilegeDetails");

            migrationBuilder.DropTable(
                name: "PageMaster");

            migrationBuilder.DropTable(
                name: "PrivilegeMaster");
        }
    }
}
