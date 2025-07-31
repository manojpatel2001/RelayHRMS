using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class designationtableaddcompid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AlterColumn<bool>(
                name: "RepeatAnnually",
                table: "HolidayMaster",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HalfDay",
                table: "HolidayMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OptionalHoliday",
                table: "HolidayMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PresentCompulsory",
                table: "HolidayMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SMS",
                table: "HolidayMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Designation",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HalfDay",
                table: "HolidayMaster");

            migrationBuilder.DropColumn(
                name: "OptionalHoliday",
                table: "HolidayMaster");

            migrationBuilder.DropColumn(
                name: "PresentCompulsory",
                table: "HolidayMaster");

            migrationBuilder.DropColumn(
                name: "SMS",
                table: "HolidayMaster");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Designation");

            migrationBuilder.AlterColumn<bool>(
                name: "RepeatAnnually",
                table: "HolidayMaster",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "HolidayMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
