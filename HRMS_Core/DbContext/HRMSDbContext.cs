using HRMS_Core.ControlPanel.CompanyInformation;
using HRMS_Core.Employee;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.Leave;
using HRMS_Core.ManagePermission;
using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.Notifications;
using HRMS_Core.PrivilegeSetting;
using HRMS_Core.Salary;
using HRMS_Core.SuperAdmin;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyInformation;
using HRMS_Core.VM.CompanyStructure;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Core.VM.Ess.InOut;
using HRMS_Core.VM.importData;
using HRMS_Core.VM.JobMaster;
using HRMS_Core.VM.Leave;
using HRMS_Core.VM.ManagePermision;
using HRMS_Core.VM.Notifications;
using HRMS_Core.VM.OtherMaster;
using HRMS_Core.VM.PrivilegeSetting;
using HRMS_Core.VM.Salary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeePersonalInfo = HRMS_Core.EmployeeMaster.EmployeePersonalInfo;

namespace HRMS_Core.DbContext
{
    public class HRMSDbContext : IdentityDbContext<HRMSUserIdentity, HRMSRoleIdentity,int >
    {
        public HRMSDbContext(DbContextOptions<HRMSDbContext> options) : base(options)
        {

        }

        public DbSet<HRMSUserIdentity> HRMSUserIdentities { get; set; }

        public DbSet<Branch> Branch { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<CityCategory> CityCategories { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Reason> Reasons { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<ShiftMaster> ShiftMasters { get; set; }
        public DbSet<ShiftBreak> ShiftBreaks { get; set; }

        public DbSet<OrganizationPolicy> OrganizationPolicy { get; set; }
        
        public DbSet<BankMaster> BankMaster { get; set; }
        public DbSet<WeekOffDetails> WeekOffDetails { get; set; }
        public DbSet<HolidayMaster> HolidayMaster { get; set; }
        public DbSet<WarningMaster> WarningMaster { get; set; }
        public DbSet<LevelWiseCardMapping> LevelWiseCardMapping { get; set; }
        public DbSet<CompanyDetails> CompanyDetails { get; set; }
        public DbSet<DirectorDetails> DirectorDetails { get; set; }
        public DbSet<EmployeePersonalInfo> EmployeePersonalInfo { get; set; }
        public DbSet<EmployeeContact> EmployeeContact { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Thana> Thana { get; set; }
        //public DbSet<PageMaster> PageMaster { get; set; }
        //public DbSet<PrivilegeMaster> PrivilegeMaster { get; set; }
        //public DbSet<PrivilegeDetails> PrivilegeDetails { get; set; }
        public DbSet<ModuleDetails> ModuleDetails { get; set; }
        public DbSet<PagePanel> PagePanel { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<EmployeeType> EmployeeType { get; set; }

        public DbSet<EmployeeInOutRecord> EmployeeInOutRecord { get; set; }



        public DbSet<HRMSRoleIdentity> HRMSRoleIdentity { get; set; }
        public DbSet<HRMSUserRole> HRMSUserRoles { get; set; }

       
        public DbSet<Earning> Earning { get; set; }
        public DbSet<Deduction> Deduction { get; set; }

        public DbSet<SuperAdminDetails> SuperAdminDetails { get; set; }

        public DbSet<EmpAttendanceImport> EmpAttendanceImport { get; set; }
        public DbSet<LeaveMaster> LeaveMaster { get; set; }
        public DbSet<LeaveDetails> LeaveDetails { get; set; }
        public DbSet<Comp_Off_Details> Comp_Off_Details { get; set; }
        public DbSet<LeaveOpening> LeaveOpening { get; set; }

        public DbSet<LeaveApplication> LeaveApplication { get; set; }

        public DbSet<PasswordHistory> PasswordHistory { get; set; }
        public DbSet<AttendanceRegularization> AttendanceRegularization { get; set; }
        public DbSet<LeftEmployee> LeftEmployee { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // For SP return type
            modelBuilder.Entity<VMCommonResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<VMEmpResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllTicketTypes>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllWeekOffDetails>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllHolidayMaster>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllCity>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllCompanyDetails>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllDirectorDetails>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllCompanyDetailsList>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllEmployee>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetNextEmployeeCode>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmPageMaster>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllPrivilegeMasterByCompanyId>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllRolesWithPermissionByCompanyId>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllPermissionByRoleId>().HasNoKey().ToView(null);

            modelBuilder.Entity<vmGetEmployeeRolesAndPermissions>().HasNoKey().ToView(null);

            modelBuilder.Entity<VMInOutRecord>().HasNoKey().ToView(null);
            modelBuilder.Entity<SearchFilterModel>().HasNoKey().ToView(null);
            modelBuilder.Entity<GetAllDeductionData>().HasNoKey().ToView(null);
            modelBuilder.Entity<GetAllEarningData>().HasNoKey().ToView(null);
            modelBuilder.Entity<EmpAttendanceVM>().HasNoKey().ToView(null);
            modelBuilder.Entity<BranchUserStatsModel>().HasNoKey().ToView(null);

            modelBuilder.Entity<PermissionDto>().HasNoKey().ToView(null);

            modelBuilder.Entity<VMCompOffDetails>().HasNoKey().ToView(null);

            modelBuilder.Entity<VMLeaveApplicationSearchResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<VmLeaveApplicationforApprove>().HasNoKey().ToView(null);

            modelBuilder.Entity<EmployeeSalaryAllowanceVM>().HasNoKey().ToView(null);

            modelBuilder.Entity<vmGetLiveEmployeeSalaryAllowance>().HasNoKey().ToView(null);
            modelBuilder.Entity<AttendanceInOutReportVM>().HasNoKey().ToView(null);
            modelBuilder.Entity<VMGetExistEmployeeCode>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmUpdateEmployee>().HasNoKey().ToView(null);
            modelBuilder.Entity<EmergencyContact>().HasNoKey().ToView(null);
            modelBuilder.Entity<RelationShip>().HasNoKey().ToView(null);
            modelBuilder.Entity<BankDetails>().HasNoKey().ToView(null);
            modelBuilder.Entity<BusinessSegment>().HasNoKey().ToView(null);

            modelBuilder.Entity<EmployeeInOutFilterVM>().HasNoKey().ToView(null);
            modelBuilder.Entity<EmployeeInOutReportVM>().HasNoKey().ToView(null);

            modelBuilder.Entity<EmployeeAttendanceReportVm>().HasNoKey().ToView(null);
            modelBuilder.Entity<AttendanceRegularizationVM>().HasNoKey().ToView(null);
            modelBuilder.Entity<AttendanceRegularizationSearchFilterVM>().HasNoKey().ToView(null);
            modelBuilder.Entity<DeleteRecordVModel>().HasNoKey().ToView(null);

            modelBuilder.Entity<AttachmentDetails>().HasNoKey().ToView(null);
            modelBuilder.Entity<ProjectDetails>().HasNoKey().ToView(null);
            modelBuilder.Entity<ContractDetails>().HasNoKey().ToView(null);
            modelBuilder.Entity<ReportingManagerDetails>().HasNoKey().ToView(null);

            modelBuilder.Entity<WishesReportVM>().HasNoKey().ToView(null);
            modelBuilder.Entity<RecentEmployeeVM>().HasNoKey().ToView(null);

            modelBuilder.Entity<EmpInOutVM>().HasNoKey().ToView(null);
            modelBuilder.Entity<LeaveTypevm>().HasNoKey().ToView(null);
            modelBuilder.Entity<SalaryReportDTO>().HasNoKey().ToView(null);
            modelBuilder.Entity<MonthlySalaryRequestViewModel>().HasNoKey().ToView(null);
            modelBuilder.Entity<SalaryDetailViewModel>().HasNoKey().ToView(null);
            modelBuilder.Entity<SalaryDetailsParameterVm>().HasNoKey().ToView(null);

            modelBuilder.Entity<UpcommingholidaysVM>().HasNoKey().ToView(null);

            modelBuilder.Entity<SalaryDetailForGetById>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllBranches>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllCityByStateId>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmCheckExistDepartmentCode>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmCheckExistBranchCode>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllBranchesListByCompanyId>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllCompanyDetailsForGrid>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmMyProfile>().HasNoKey().ToView(null);
            modelBuilder.Entity<EmployeeDirectoryResultVM>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetMonthlyAttendanceLog>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetMonthlyAttendanceDetails>().HasNoKey().ToView(null);
            modelBuilder.Entity<EmployeeDirectIndirectReport>().HasNoKey().ToView(null);

            modelBuilder.Entity<EmployeePersonalInformationVM>().HasNoKey().ToView(null);
            modelBuilder.Entity<GetCountDirectOrIndirectEmployeesVM>().HasNoKey().ToView(null);

            modelBuilder.Entity<AttendanceDetailsViewModel>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetEmployeesByReportingManager>().HasNoKey().ToView(null);

            modelBuilder.Entity<SP_Response>().HasNoKey().ToView(null);
            modelBuilder.Entity<Permission>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllEmployeeListByCompanyId>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllUserWithPermissionByCompanyId>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllPermissionByEmployeeId>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetEmployeeById>().HasNoKey().ToView(null);
            modelBuilder.Entity<VmLeftEmployee>().HasNoKey().ToView(null);
            modelBuilder.Entity<MyTeamleavesVM>().HasNoKey().ToView(null);
            modelBuilder.Entity<BranchViewModel>().HasNoKey().ToView(null);
            modelBuilder.Entity<EmployeeViewModel>().HasNoKey().ToView(null);

            modelBuilder.Entity<NotificationRemainders>().HasNoKey().ToView(null);
            modelBuilder.Entity<vmGetAllNotificationByUserId>().HasNoKey().ToView(null);

            modelBuilder.Entity<salaryslipParam>().HasNoKey().ToView(null);
            modelBuilder.Entity<LeaveApprovalReportVM>().HasNoKey().ToView(null);
            modelBuilder.Entity<LeaveApp_Param>().HasNoKey().ToView(null);
            modelBuilder.Entity<LeaveBalanceViewModel>().HasNoKey().ToView(null);
            modelBuilder.Entity<ActiveLeaveDetailsvm>().HasNoKey().ToView(null);
            modelBuilder.Entity<AttendanceDetails>().HasNoKey().ToView(null);
            modelBuilder.Entity<TicketType>().HasNoKey().ToView(null);
            modelBuilder.Entity<TicketPriority>().HasNoKey().ToView(null);
            modelBuilder.Entity<TicketStatus>().HasNoKey().ToView(null);




        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }


    }
}
