using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class createWeekOffDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeekOffDetails",
                columns: table => new
                {
                    WeekOffDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    SundayWeekOffDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MondayWeekOffDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TuesdayWeekOffDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WednesdayWeekOffDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThursdayWeekOffDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FridayWeekOffDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaturdayWeekOffDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_WeekOffDetails", x => x.WeekOffDetailsId);
                    table.ForeignKey(
                        name: "FK_WeekOffDetails_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeekOffDetails_BranchId",
                table: "WeekOffDetails",
                column: "BranchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeekOffDetails");
        }
    }
}
