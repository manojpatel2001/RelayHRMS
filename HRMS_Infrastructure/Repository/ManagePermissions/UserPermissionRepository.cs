using HRMS_Core.DbContext;
using HRMS_Core.ManagePermission;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface.ManagePermissions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.ManagePermissions
{
    public class UserPermissionRepository: IUserPermissionRepository
    {
        private readonly HRMSDbContext _db;

        public UserPermissionRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateUserPermission(UserPermission permission)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageUserPermission
                    @Action = {"CREATE"},
                    @EmployeeId = {permission.EmployeeId},
                    @PermissionIds = {permission.PermissionIds},
                    @CompanyId = {permission.CompanyId}
                    
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<List<vmGetAllEmployeeListByCompanyId>> GetAllEmployeeListByCompanyId(int CompanyId)
        {
            try
            {
                return await _db.Set<vmGetAllEmployeeListByCompanyId>().FromSqlInterpolated($"EXEC GetAllEmployeeListByCompanyId @CompanyId={CompanyId}").ToListAsync();
            }
            catch
            {
                return new List<vmGetAllEmployeeListByCompanyId>();
            }
        }

        public async Task<List<vmGetAllUserWithPermissionByCompanyId>> GetAllUserWithPermissionByCompanyId(int CompanyId)
        {
            try
            {
                return await _db.Set<vmGetAllUserWithPermissionByCompanyId>().FromSqlInterpolated($"EXEC GetAllUserWithPermissionByCompanyId @CompanyId={CompanyId}").ToListAsync();
            }
            catch
            {
                return new List<vmGetAllUserWithPermissionByCompanyId>();
            }
        }

        public async Task<List<vmGetAllPermissionByEmployeeId>> GetAllPermissionByEmployeeId(vmRoleManagePermission permission)
        {
            try
            {
                return await _db.Set<vmGetAllPermissionByEmployeeId>().FromSqlInterpolated($"EXEC GetAllPermissionByEmployeeId @CompanyId={permission.CompanyId},@EmployeeId={permission.EmployeeId}").ToListAsync();
            }
            catch
            {
                return new List<vmGetAllPermissionByEmployeeId>();
            }
        }

        public async Task<VMCommonResult> DeleteUserPermission(vmRoleManagePermission delete)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageUserPermission
                    @Action = {"DELETE"},
                    @EmployeeId = {delete.EmployeeId},
                    @CompanyId = {delete.CompanyId}
              ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }


    }
}
