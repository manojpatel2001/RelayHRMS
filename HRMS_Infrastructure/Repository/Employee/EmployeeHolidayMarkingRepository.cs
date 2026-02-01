using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.JobMaster;
using HRMS_Core.VM.ManagePermision;
using HRMS_Core.VM.OtherMaster;
using HRMS_Infrastructure.Interface.Employee;
using HRMS_Infrastructure.Repository.Salary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class EmployeeHolidayMarkingRepository : Repository<EmployeeHolidayMarkingRepository>, IEmployeeHolidayMarkingRepository
    {

        private HRMSDbContext _db;

        public EmployeeHolidayMarkingRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public Task AddAsync(EmployeeHolidayMarking entity)
        {
            throw new NotImplementedException();
        }

        public async Task<SP_Response> CreateEmployeeHoliday(EmployeeHolidayMarking model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
            EXEC sp_EmployeeHolidayMarking_CRUD
            @Operation = {"CREATE"},
            @EmployeeId = {model.EmployeeId},
            @HolidayDate = {model.HolidayDate},
            @Marked = {model.Marked},
            @HolidayName = {model.HolidayName},
            @CreatedBy = {model.CreatedBy}
        ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }


        public async Task<SP_Response> DeleteEmployeeHoliday( int Id ,int DeletedBy)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                EXEC sp_EmployeeHolidayMarking_CRUD
                    @Operation = {"DELETE"},
                    @EmployeeHolidayId = {Id},
                    @DeletedBy = {DeletedBy}
            ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Some thing went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Some thing went wrong!" };
            }

        }

        public Task<IEnumerable<EmployeeHolidayMarking>> GetAllAsync(Expression<Func<EmployeeHolidayMarking, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EmployeeHolidayMarkingViewModel>> GetAllEmployeeHoliday()
        {
            try
            {
                var result = await _db.Set<EmployeeHolidayMarkingViewModel>()
                    .FromSqlRaw("EXEC [dbo].[GetEmployeeHolidayMarkingDetails]")
                    .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
            
                Console.WriteLine(ex.Message);
                throw; 
            }
        }


        public Task<EmployeeHolidayMarking> GetAsync(Expression<Func<EmployeeHolidayMarking, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeeHolidayMarking?> GetEmpHolidayById(vmCommonGetById filter)
        {
            try
            {
                var result = await _db.Set<EmployeeHolidayMarking>().FromSqlInterpolated($@"
            EXEC sp_EmployeeHolidayMarking_CRUD
            @Operation = {"GET"},
            @EmployeeHolidayId = {filter.Id}
        ").ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }


        public Task RemoveAsync(EmployeeHolidayMarking entity)
        {
            throw new NotImplementedException();
        }
        public async Task<SP_Response> UpdateEmployeeHoliday(EmployeeHolidayMarking model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
            EXEC sp_EmployeeHolidayMarking_CRUD
            @Operation = {"UPDATE"},
            @EmployeeHolidayId = {model.EmployeeHolidayId},
            @EmployeeId = {model.EmployeeId},
            @HolidayDate = {model.HolidayDate},
            @Marked = {model.Marked},
            @HolidayName = {model.HolidayName},
            @UpdatedBy = {model.UpdatedBy}
        ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

    }
}
