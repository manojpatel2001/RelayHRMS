using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Core.Migrations
{
    /// <inheritdoc />
    public partial class CreateEmployeeAdditionInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Country", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Initial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfJoining = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Branch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Shift = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CTC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrossSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserPrivilege = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoginAlias = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Thana",
                columns: table => new
                {
                    ThanaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThanaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Thana", x => x.ThanaId);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeContact",
                columns: table => new
                {
                    EmployeeContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PresentAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PresentTehsil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PresentDistrict = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PresentCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PresentState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PresentPincode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PresentThana = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentTehsil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentDistrict = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentPincode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentThana = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonalPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficialEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtensionNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SameAsPresentAddress = table.Column<bool>(type: "bit", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_EmployeeContact", x => x.EmployeeContactId);
                    table.ForeignKey(
                        name: "FK_EmployeeContact_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId");
                });

            migrationBuilder.CreateTable(
                name: "EmployeePersonalInfo",
                columns: table => new
                {
                    EmployeePersonalInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonalEmailId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BloodGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarriageDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MarkIdentification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Caste = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CastCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadharCardNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PANNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dispensary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DispensaryAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UANNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrivingLicense = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrivingLicenseExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RationCardType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RationCardNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkedInId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProbationCompletionPeriod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProbationPeriodType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerProbation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfirmDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RetirementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfferDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TraineeCompletionPeriod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TraineePeriodType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CanteenCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TallyLedgerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMetroCity = table.Column<bool>(type: "bit", nullable: true),
                    AdultWorkerNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhysicalDisability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinimumWageSkillType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuranceNo = table.Column<bool>(type: "bit", nullable: true),
                    DressCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShirtSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PantSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShoeSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_EmployeePersonalInfo", x => x.EmployeePersonalInfoId);
                    table.ForeignKey(
                        name: "FK_EmployeePersonalInfo_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContact_EmployeeId",
                table: "EmployeeContact",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePersonalInfo_EmployeeId",
                table: "EmployeePersonalInfo",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "EmployeeContact");

            migrationBuilder.DropTable(
                name: "EmployeePersonalInfo");

            migrationBuilder.DropTable(
                name: "Thana");

            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}
