using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class shiftmaster_and_shiftbreaks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShiftMaster",
                columns: table => new
                {
                    ShiftID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsHalfDay = table.Column<bool>(type: "bit", nullable: false),
                    AutoShiftChange = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsTrainingShift = table.Column<bool>(type: "bit", nullable: false),
                    IsSplitShift = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftMaster", x => x.ShiftID);
                });

            migrationBuilder.CreateTable(
                name: "ShiftBreak",
                columns: table => new
                {
                    BreakID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftID = table.Column<int>(type: "int", nullable: false),
                    BreakName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: true),
                    DeductBreak = table.Column<bool>(type: "bit", nullable: false),
                    DeductHour = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftBreak", x => x.BreakID);
                    table.ForeignKey(
                        name: "FK_ShiftBreak_ShiftMaster_ShiftID",
                        column: x => x.ShiftID,
                        principalTable: "ShiftMaster",
                        principalColumn: "ShiftID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftBreak_ShiftID",
                table: "ShiftBreak",
                column: "ShiftID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShiftBreak");

            migrationBuilder.DropTable(
                name: "ShiftMaster");
        }
    }
}
