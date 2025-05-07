using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class CreateCompanyDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyDetails",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    StateId = table.Column<int>(type: "int", nullable: true),
                    PinCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PfTrustNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PfApplicable = table.Column<bool>(type: "bit", nullable: true),
                    PFNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EsicApplicable = table.Column<bool>(type: "bit", nullable: true),
                    ESICNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TanNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PanNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DomainName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LwfNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCodeSetting = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InOutDuration = table.Column<int>(type: "int", nullable: true),
                    HierarchyDesignation = table.Column<bool>(type: "bit", nullable: true),
                    EmployeeLicense = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractorCompany = table.Column<bool>(type: "bit", nullable: true),
                    DigitalSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDigitalSignature = table.Column<bool>(type: "bit", nullable: true),
                    SelectWeekOffDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlternateWeekOffDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlternateFullWeekOff = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyLogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DigitalSignatureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DigitalSignaturePassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_CompanyDetails", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_CompanyDetails_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "CityID");
                    table.ForeignKey(
                        name: "FK_CompanyDetails_State_StateId",
                        column: x => x.StateId,
                        principalTable: "State",
                        principalColumn: "StateId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDetails_CityId",
                table: "CompanyDetails",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDetails_StateId",
                table: "CompanyDetails",
                column: "StateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyDetails");
        }
    }
}
