using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.JobMaster;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface.JobMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.JobMaster
{
    public class DepartmentRepository : Repository<Department> , IDepartmentRepository
    {
        private readonly HRMSDbContext _db;

        public DepartmentRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateDepartment(Department model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageDepartment
                    @Action = {"CREATE"},
                    @DepartmentName = {model.DepartmentName},
                    @DepartmentCode = {model.DepartmentCode},
                    @CompanyId = {model.CompanyId},
                    @IsActive = {model.IsActive},
                    @CreatedBy = {model.CreatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateDepartment(Department model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageDepartment
                    @Action = {"UPDATE"},
                    @DepartmentId = {model.DepartmentId},
                    @DepartmentName = {model.DepartmentName},
                    @DepartmentCode = {model.DepartmentCode},
                    @CompanyId = {model.CompanyId},
                    @IsActive = {model.IsActive},
                    @UpdatedBy = {model.UpdatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteDepartment(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageDepartment
                    @Action = {"DELETE"},
                    @DepartmentId = {deleteRecord.Id},
                    @DeletedBy = {deleteRecord.DeletedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<List<Department>> GetAllDepartments(vmCommonGetById filters)
        {
            try
            {
                var result = await _db.Set<Department>().FromSqlInterpolated($@"
                EXEC GetAllDepartments
                    @IsDeleted = {filters.IsDeleted},
                    @IsEnabled = {filters.IsEnabled},
                    @DepartmentName = {filters.Title}
            ").ToListAsync();

                return result;
            }
            catch
            {
                return new List<Department>();
            }
        }

        public async Task<Department?> GetDepartmentById(vmCommonGetById filter)
        {
            try
            {
                var result = await _db.Set<Department>().FromSqlInterpolated($@"
                EXEC GetDepartmentById
                    @DepartmentId = {filter.Id},
                    @IsDeleted = {filter.IsDeleted},
                    @IsEnabled = {filter.IsEnabled}
            ").ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        public async Task<vmCheckExistDepartmentCode?> CheckExistDepartmentCode(vmCommonGetById filter)
        {
            try
            {
                var result = await _db.Set<vmCheckExistDepartmentCode>().FromSqlInterpolated($@"
                EXEC CheckExistDepartmentCode
                   @DepartmentCode = {filter.Title}
            ").ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
    }
}
