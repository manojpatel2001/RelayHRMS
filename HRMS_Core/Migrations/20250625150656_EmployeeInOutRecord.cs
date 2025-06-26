using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeInOutRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeInOutRecord",
                columns: table => new
                {
                    Emp_IO_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Emp_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Com_Id = table.Column<int>(type: "int", nullable: true),
                    For_Date = table.Column<DateOnly>(type: "date", nullable: true),
                    In_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Out_Time = table.Column<DateOnly>(type: "date", nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ip_adrress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    In_Date_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Out_Date_Time = table.Column<DateOnly>(type: "date", nullable: true),
                    Skip_Count = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Late_Calc_Not_App = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chk_By_Superior = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sup_Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Half_Full_day = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Is_Cancel_Late_In = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Is_Cancel_Early_Out = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Is_Default_In = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Is_Default_Out = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cmp_prp_in_flag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cmp_prp_out_flag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_Cmp_purpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    App_Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Apr_Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    System_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Other_Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManualEntryFlag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    statusFlag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    In_Admin_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Out_Admin_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_EmployeeInOutRecord", x => x.Emp_IO_Id);
                    table.ForeignKey(
                        name: "FK_EmployeeInOutRecord_AspNetUsers_Emp_Id",
                        column: x => x.Emp_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeInOutRecord_CompanyDetails_Com_Id",
                        column: x => x.Com_Id,
                        principalTable: "CompanyDetails",
                        principalColumn: "CompanyId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInOutRecord_Com_Id",
                table: "EmployeeInOutRecord",
                column: "Com_Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInOutRecord_Emp_Id",
                table: "EmployeeInOutRecord",
                column: "Emp_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeInOutRecord");
        }
    }
}
