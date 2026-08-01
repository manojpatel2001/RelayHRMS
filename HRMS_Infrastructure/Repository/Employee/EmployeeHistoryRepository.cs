using HRMS_Core.DbContext;
using HRMS_Core.VM.Ess.RecentActivity;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class EmployeeHistoryRepository : IEmployeeHistoryRepository
    {
        private readonly HRMSDbContext _db;

        public EmployeeHistoryRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<List<vmEmployeeLifecycleEvent>> GetEmployeeLifecycleTimeline(int employeeId, int companyId)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@EmployeeId", employeeId),
                    new SqlParameter("@CompanyId", companyId)
                };

                var result = await _db.Set<vmEmployeeLifecycleEvent>()
                    .FromSqlRaw("EXEC sp_GetEmployeeLifecycleTimeline @EmployeeId, @CompanyId", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<vmEmployeeLifecycleEvent>();
            }
        }
    }
}
