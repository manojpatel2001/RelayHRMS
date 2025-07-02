using HRMS_Core.DbContext;
using HRMS_Core.ManagePermission;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface.ManagePermissions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.ManagePermissions
{
    public class RolePermissionRepository : Repository<RolePermission>, IRolePermissionRepository
    {
        private readonly HRMSDbContext _db;

        public RolePermissionRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<vmGetAllRolesWithPermissionByCompanyId>> GetAllRolesWithPermissionByCompanyId(int CompanyId)
        {
            try
            {
                return await _db.Set<vmGetAllRolesWithPermissionByCompanyId>().FromSqlInterpolated($"EXEC GetAllRolesWithPermissionByCompanyId @CompanyId={CompanyId}").ToListAsync();
            }
            catch
            {
                return new List<vmGetAllRolesWithPermissionByCompanyId>();
            }
        }
        public async Task<List<vmGetAllPermissionByRoleId>> GetAllPermissionByRoleId(vmRoleManagePermission vmRole)
        {
            try
            {
                return await _db.Set<vmGetAllPermissionByRoleId>().FromSqlInterpolated($"EXEC GetAllPermissionByRoleId @CompanyId={vmRole.CompanyId},@RoleId={vmRole.RoleId}").ToListAsync();
            }
            catch
            {
                return new List<vmGetAllPermissionByRoleId>();
            }
        }
        public async Task<VMCommonResult> CreateRolePermission(RolePermission permission)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageRolePermission
                    @Action = {"CREATE"},
                    @RoleId = {permission.RoleId},
                    @PermissionId = {permission.PermissionId},
                    @CompanyId = {permission.CompanyId},
                    @IsDeleted = {permission.IsDeleted},
                    @IsEnabled = {permission.IsEnabled},
                    @IsBlocked = {permission.IsBlocked},
                    @CreatedDate = {permission.CreatedDate},
                    @CreatedBy = {permission.CreatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateRolePermission(RolePermission permission)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageRolePermission
                    @Action = {"UPDATE"},
                    @RolePermissionId = {permission.RolePermissionId},
                    @RoleId = {permission.RoleId},
                    @PermissionId = {permission.PermissionId},
                    @UpdatedDate = {permission.UpdatedDate},
                    @UpdatedBy = {permission.UpdatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteRolePermission(vmRoleManagePermission delete)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageRolePermission
                    @Action = {"DELETE"},
                    @RoleId = {delete.RoleId},
                    @CompanyId = {delete.CompanyId},
                    @DeletedBy = {delete.DeletedBy}
              ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<List<RoleManagePermissionDto>> GetAllRolesWithPermissionByRoleId(vmRoleManagePermission vmRole)
        {
            try
            {
                DbConnection connection = _db.Database.GetDbConnection();
                await connection.OpenAsync();

                DbCommand command = connection.CreateCommand();
                command.CommandText = "GetAllRolesWithPermissionByRoleId"; // ✅ Your JSON-returning stored procedure
                command.CommandType = CommandType.StoredProcedure;

                var roleParam1 = command.CreateParameter();
                roleParam1.ParameterName = "@CompanyId";
                roleParam1.Value = vmRole.CompanyId;
                command.Parameters.Add(roleParam1);

                var roleParam2 = command.CreateParameter();
                roleParam2.ParameterName = "@RoleId"; // ✅ Correct name
                roleParam2.Value = vmRole.RoleId;
                command.Parameters.Add(roleParam2); // ✅ Add the right object

                using DbDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    string json = reader.GetString(0);
                    return JsonSerializer.Deserialize<List<RoleManagePermissionDto>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<RoleManagePermissionDto>();
                }

                return new List<RoleManagePermissionDto>();
            }
            catch
            {
                return new List<RoleManagePermissionDto>();
            }
        }
        public async Task<List<vmGetEmployeeRolesAndPermissions>> GetEmployeeRolesAndPermissions(int EmployeeId)
        {
            try
            {
                return await _db.Set<vmGetEmployeeRolesAndPermissions>().FromSqlInterpolated($"EXEC GetEmployeeRolesAndPermissions @EmployeeId={EmployeeId}").ToListAsync();
            }
            catch
            {
                return new List<vmGetEmployeeRolesAndPermissions>();
            }
        }
    }

}
