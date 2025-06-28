using HRMS_Core.DbContext;
using HRMS_Core.ManagePermission;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.ManagePermissions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<VMCommonResult> DeleteRolePermission(DeleteRecordVM delete)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageRolePermission
                    @Action = {"DELETE"},
                    @RolePermissionId = {delete.Id},
                    @DeletedDate = {delete.DeletedDate},
                    @DeletedBy = {delete.DeletedBy}
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
