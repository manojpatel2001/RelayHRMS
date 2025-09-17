using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class EmployeeIncrementRespository: IEmployeeIncrementRespository
    {
        private readonly HRMSDbContext _db;

        public EmployeeIncrementRespository(HRMSDbContext db) 
        {
            _db = db;
        }
        public async Task<List<IncrementReason>> GetAllIncrementReason()
        {
            try
            {
                var result = await _db.Set<IncrementReason>()
                                      .FromSqlInterpolated($"EXEC GetAllIncrementReason")
                                      .ToListAsync();

                return result ?? new List<IncrementReason>();
            }
            catch (Exception ex)
            {
                return new List<IncrementReason>();
            }
        }
    }
}
