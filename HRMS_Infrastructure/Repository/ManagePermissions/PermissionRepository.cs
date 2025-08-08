using HRMS_Core.DbContext;
using HRMS_Core.ManagePermission;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface.ManagePermissions;
using Microsoft.Data.SqlClient;
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
    public class PermissionRepository : IPermissionRepository
    {
        private readonly HRMSDbContext _db;

        public PermissionRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<List<Permission>> GetAllPermissions(vmPermissionPara vmPermissionPara)
        {
            try
            {
                return await _db.Set<Permission>().FromSqlInterpolated($"EXEC GetAllPermissions @PermissionName={vmPermissionPara.PermissionName},@Slug={vmPermissionPara.Slug}").ToListAsync();
            }
            catch
            {
                return new List<Permission>();
            }
        }

        public async Task<Permission?> GetPermissionById(vmCommonGetById vmCommonGetById)
        {
            try
            {
                var result = await _db.Set<Permission>()
                    .FromSqlInterpolated($"EXEC GetPermissionById @PermissionId = {vmCommonGetById.Id},@IsDeleted = {vmCommonGetById.IsDeleted},@IsEnabled = {vmCommonGetById.IsEnabled}").ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task<VMCommonResult> CreatePermission(Permission permission)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManagePermission
                    @Action = {"CREATE"},
                    @PermissionName = {permission.PermissionName},
                    @Slug = {permission.Slug},
                    @Description = {permission.Description},
                    @PermissionUrl = {permission.PermissionUrl},
                    @GroupName = {permission.GroupName},
                    @PermissionType = {permission.PermissionType},
                    @IsActive = {permission.IsActive}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdatePermission(Permission permission)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManagePermission
                    @Action = {"UPDATE"},
                    @PermissionId = {permission.PermissionId},
                    @PermissionName = {permission.PermissionName},
                    @Slug = {permission.Slug},
                    @Description = {permission.Description},
                    @PermissionUrl = {permission.PermissionUrl},
                    @GroupName = {permission.GroupName},
                    @PermissionType = {permission.PermissionType},
                    @IsActive = {permission.IsActive}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeletePermission(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManagePermission
                    @Action = {"DELETE"},
                    @PermissionId = {deleteRecord.Id}
                    
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }




        public async Task<List<PermissionDto>> GetAllGroupPermissionList()
        {
            try
            {
                return await _db.Set<PermissionDto>().FromSqlInterpolated($"EXEC GetAllGroupPermissionList").ToListAsync();
            }
            catch
            {
                return new List<PermissionDto>();
            }
        }


    }

}
