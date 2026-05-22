using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class AddLeftEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeftEmployee",
                columns: table => new
                {
                    LeftID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CmpID = table.Column<int>(type: "int", nullable: true),
                    EmpID = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    LeftDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeftReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegAcceptDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsTerminate = table.Column<bool>(type: "bit", nullable: true),
                    UniformReturn = table.Column<bool>(type: "bit", nullable: true),
                    ExitInterview = table.Column<bool>(type: "bit", nullable: true),
                    NoticePeriod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeath = table.Column<bool>(type: "bit", nullable: true),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsFnFApplicable = table.Column<bool>(type: "bit", nullable: true),
                    RptManagerID = table.Column<int>(type: "int", nullable: true),
                    IsRetire = table.Column<bool>(type: "bit", nullable: true),
                    RequestAprID = table.Column<int>(type: "int", nullable: true),
                    LeftReasonValue = table.Column<int>(type: "int", nullable: true),
                    LeftReasonText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Res_Id = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_LeftEmployee", x => x.LeftID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeftEmployee");
        }
    }
}
