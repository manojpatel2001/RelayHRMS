using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HRMS_Core.DbContext
{
    public class HRMSDbContextFactory : IDesignTimeDbContextFactory<HRMSDbContext>
    {
        public HRMSDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HRMSDbContext>();

            optionsBuilder.UseSqlServer("Data Source=15.235.82.113,36729;Initial Catalog=RELAYHRMS_DEV;User ID=HrmsDev;Password=Hrms@1234;Trust Server Certificate=True;");
            return new HRMSDbContext(optionsBuilder.Options);
        }
    }
}

