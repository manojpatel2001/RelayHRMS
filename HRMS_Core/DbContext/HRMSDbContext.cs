using HRMS_Core.Master.JobMaster;
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

    }
}
