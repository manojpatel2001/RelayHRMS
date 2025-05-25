using HRMS_Core.ControlPanel.CompanyInformation;
using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyInformation;
using HRMS_Core.VM.CompanyStructure;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Core.VM.JobMaster;
using HRMS_Core.VM.OtherMaster;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.DbContext
{
    public class HRMSDbContext : IdentityDbContext<HRMSUserIdentity, HRMSRoleIdentity, string>
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
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }


    }
}
