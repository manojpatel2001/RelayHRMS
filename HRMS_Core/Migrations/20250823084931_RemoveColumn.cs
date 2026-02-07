using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Carry_forword_leave",
                table: "LeaveMaster");

            migrationBuilder.DropColumn(
                name: "Leave_Paid_Unpaid",
                table: "LeaveMaster");

            migrationBuilder.DropColumn(
                name: "Leave_Type",
                table: "LeaveMaster");

            migrationBuilder.DropColumn(
                name: "TotalLeave",
                table: "LeaveMaster");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Carry_forword_leave",
                table: "LeaveMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Leave_Paid_Unpaid",
                table: "LeaveMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Leave_Type",
                table: "LeaveMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotalLeave",
                table: "LeaveMaster",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
