using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class ModifyWeekOff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeekOffDetails_Branch_BranchId",
                table: "WeekOffDetails");

            migrationBuilder.DropIndex(
                name: "IX_WeekOffDetails_BranchId",
                table: "WeekOffDetails");

            migrationBuilder.DropColumn(
                name: "FridayWeekOffDay",
                table: "WeekOffDetails");

            migrationBuilder.DropColumn(
                name: "MondayWeekOffDay",
                table: "WeekOffDetails");

            migrationBuilder.DropColumn(
                name: "SaturdayWeekOffDay",
                table: "WeekOffDetails");

            migrationBuilder.DropColumn(
                name: "SundayWeekOffDay",
                table: "WeekOffDetails");

            migrationBuilder.DropColumn(
                name: "ThursdayWeekOffDay",
                table: "WeekOffDetails");

            migrationBuilder.DropColumn(
                name: "TuesdayWeekOffDay",
                table: "WeekOffDetails");

            migrationBuilder.RenameColumn(
                name: "WednesdayWeekOffDay",
                table: "WeekOffDetails",
                newName: "WeekOffDay");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "WeekOffDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "WeekOffDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PunchType",
                table: "EmployeeInOutRecord",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeekOffDetailsId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "WeekOffDetails");

            migrationBuilder.DropColumn(
                name: "PunchType",
                table: "EmployeeInOutRecord");

            migrationBuilder.DropColumn(
                name: "WeekOffDetailsId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "WeekOffDay",
                table: "WeekOffDetails",
                newName: "WednesdayWeekOffDay");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "WeekOffDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FridayWeekOffDay",
                table: "WeekOffDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MondayWeekOffDay",
                table: "WeekOffDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaturdayWeekOffDay",
                table: "WeekOffDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SundayWeekOffDay",
                table: "WeekOffDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThursdayWeekOffDay",
                table: "WeekOffDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TuesdayWeekOffDay",
                table: "WeekOffDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeekOffDetails_BranchId",
                table: "WeekOffDetails",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeekOffDetails_Branch_BranchId",
                table: "WeekOffDetails",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
