using Azure.Core;
using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Leave;
using HRMS_Core.VM.Report;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class AttendanceLockRepository :Repository<AttendanceLockRepository> ,IAttendanceLockRepository
    {


        private HRMSDbContext _db;

        public AttendanceLockRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }
        public Task AddAsync(AttendanceLock entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AttendanceLock>> GetAllAsync(Expression<Func<AttendanceLock, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public Task<AttendanceLock> GetAsync(Expression<Func<AttendanceLock, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EmployeeLockStatusViewModel>> GetEmployeeLockStatus(AttendanceLockParamVm model)
        {
            try
            {
                var result = await _db.Set<EmployeeLockStatusViewModel>()
                    .FromSqlInterpolated($"EXEC GetEmployeeLockStatus  @Month = {model.Month}, @Year = {model.Year}, @EmpIds = {model.EmployeeId} ,@StatusFilter={model.Status}")
                    .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetEmployeeLockStatus Error: " + ex.Message);
                return new List<EmployeeLockStatusViewModel>();
            }
        }

        public Task RemoveAsync(AttendanceLock entity)
        {
            throw new NotImplementedException();
        }
    }
}
