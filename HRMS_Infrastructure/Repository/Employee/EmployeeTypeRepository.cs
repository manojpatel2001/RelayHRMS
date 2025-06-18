using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class EmployeeTypeRepository:Repository<EmployeeType>, IEmployeeTypeRepository
    {
        private readonly HRMSDbContext _db;

        public EmployeeTypeRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateEmployeeType(EmployeeType employeeType)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageEmployeeType
                    @Action = {"CREATE"},
                    @EmployeeTypeName = {employeeType.EmployeeTypeName},
                    @IsDeleted = {employeeType.IsDeleted},
                    @IsEnabled = {employeeType.IsEnabled},
                    @IsBlocked = {employeeType.IsBlocked},
                    @CreatedDate = {employeeType.CreatedDate},
                    @CreatedBy = {employeeType.CreatedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception ex)
            {
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<VMCommonResult> UpdateEmployeeType(EmployeeType employeeType)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageEmployeeType
                    @Action = {"UPDATE"},
                    @EmployeeTypeId = {employeeType.EmployeeTypeId},
                    @EmployeeTypeName = {employeeType.EmployeeTypeName},
                    @UpdatedDate = {DateTime.UtcNow},
                    @UpdatedBy = {employeeType.UpdatedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception ex)
            {
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<VMCommonResult> DeleteEmployeeType(DeleteRecordVM employeeType)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageEmployeeType
                    @Action = {"DELETE"},
                    @EmployeeTypeId = {employeeType.Id},
                    @DeletedDate = {DateTime.UtcNow},
                    @DeletedBy = {employeeType.DeletedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception ex)
            {
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<EmployeeType?> GetEmployeeTypeById(int employeeTypeId)
        {
            try
            {
                var result = await _db.Set<EmployeeType>()
                                      .FromSqlInterpolated($"EXEC GetEmployeeTypeById @EmployeeTypeId = {employeeTypeId}")
                                      .ToListAsync();

                return result.FirstOrDefault() ?? null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<EmployeeType>> GetAllEmployeeTypes()
        {
            try
            {
                var result = await _db.Set<EmployeeType>()
                                      .FromSqlInterpolated($"EXEC GetAllEmployeeTypes")
                                      .ToListAsync();

                return result ?? new List<EmployeeType>();
            }
            catch (Exception ex)
            {
                return new List<EmployeeType>();
            }
        }

    }
}
