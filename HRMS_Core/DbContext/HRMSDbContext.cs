using HRMS_Core.Master.JobMaster;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // For SP return type
            modelBuilder.Entity<VMCommonResult>().HasNoKey().ToView(null);
        }

    }
}
