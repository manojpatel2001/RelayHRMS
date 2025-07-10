using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class LeaveApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeaveApplication",
                columns: table => new
                {
                    LeaveApplicationid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmplooyeId = table.Column<int>(type: "int", nullable: true),
                    ReportingManagerId = table.Column<int>(type: "int", nullable: true),
                    LeaveType = table.Column<int>(type: "int", nullable: true),
                    ApplicationType = table.Column<bool>(type: "bit", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    No_Of_Date = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Todate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reason = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Responsible = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cancel_Weekoff = table.Column<bool>(type: "bit", nullable: true),
                    Send_Intimate = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_LeaveApplication", x => x.LeaveApplicationid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaveApplication");
        }
    }
}
