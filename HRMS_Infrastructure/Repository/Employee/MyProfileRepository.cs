using HRMS_Core.DbContext;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class MyProfileRepository : IMyProfileRepository
    {
        private readonly HRMSDbContext _db;

        public MyProfileRepository(HRMSDbContext db) 
        {
            _db = db;
        }

        public async Task<List<vmMyProfile>> GetEmployeeProfile(int employeeId, int companyId)
        {

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@EmployeeId", employeeId),
                    new SqlParameter("@CompanyId", companyId),
              
                };

                var result = await _db.Set<vmMyProfile>()
                    .FromSqlRaw("EXEC GetMyProfile @EmployeeId, @CompanyId", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<vmMyProfile>();
            }
        }
    }
}
