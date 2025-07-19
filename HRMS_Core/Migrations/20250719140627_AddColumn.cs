using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class AddColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Insurance",
                table: "Deduction",
                newName: "TermInsurance");

            migrationBuilder.AddColumn<decimal>(
                name: "ChildEducationAllowance",
                table: "Earning",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GroupMedical",
                table: "Deduction",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChildEducationAllowance",
                table: "Earning");

            migrationBuilder.DropColumn(
                name: "GroupMedical",
                table: "Deduction");

            migrationBuilder.RenameColumn(
                name: "TermInsurance",
                table: "Deduction",
                newName: "Insurance");
        }
    }
}
