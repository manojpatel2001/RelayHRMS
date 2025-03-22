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

    }
}
