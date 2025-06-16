using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class AddPagePanelId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PagePanelId",
                table: "PageMaster",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PageMaster_PagePanelId",
                table: "PageMaster",
                column: "PagePanelId");

            migrationBuilder.AddForeignKey(
                name: "FK_PageMaster_PagePanel_PagePanelId",
                table: "PageMaster",
                column: "PagePanelId",
                principalTable: "PagePanel",
                principalColumn: "PagePanelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageMaster_PagePanel_PagePanelId",
                table: "PageMaster");

            migrationBuilder.DropIndex(
                name: "IX_PageMaster_PagePanelId",
                table: "PageMaster");

            migrationBuilder.DropColumn(
                name: "PagePanelId",
                table: "PageMaster");
        }
    }
}
