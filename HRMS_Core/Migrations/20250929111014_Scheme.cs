using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class Scheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Scheme Master",
                table: "Scheme Master");

            migrationBuilder.RenameTable(
                name: "Scheme Master",
                newName: "SchemeMaster");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchemeMaster",
                table: "SchemeMaster",
                column: "SchemeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SchemeMaster",
                table: "SchemeMaster");

            migrationBuilder.RenameTable(
                name: "SchemeMaster",
                newName: "Scheme Master");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Scheme Master",
                table: "Scheme Master",
                column: "SchemeID");
        }
    }
}
