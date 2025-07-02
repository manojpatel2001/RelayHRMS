using HRMS_Core.ControlPanel.CompanyInformation;
using HRMS_Core.Employee;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.Leave;
using HRMS_Core.ManagePermission;
using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.PrivilegeSetting;
using HRMS_Core.Salary;
using HRMS_Core.SuperAdmin;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyInformation;
using HRMS_Core.VM.CompanyStructure;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Core.VM.importData;
using HRMS_Core.VM.JobMaster;
using HRMS_Core.VM.ManagePermision;
using HRMS_Core.VM.OtherMaster;
using HRMS_Core.VM.PrivilegeSetting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HRMS_Core.VM.importData.GetAllDeductionData;
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
        public DbSet<TicketType> TicketType { get; set; }
        public DbSet<TicketPriority> TicketPriority { get; set; }
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

        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<UserPermission> UserPermission { get; set; }

       
        public DbSet<Earning> Earning { get; set; }
        public DbSet<Deduction> Deduction { get; set; }

        public DbSet<SuperAdminDetails> SuperAdminDetails { get; set; }

        public DbSet<EmpAttendanceImport> EmpAttendanceImport { get; set; }
        public DbSet<LeaveMaster> LeaveMaster { get; set; }
        public DbSet<LeaveDetails> LeaveDetails { get; set; }



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
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }


    }
}
