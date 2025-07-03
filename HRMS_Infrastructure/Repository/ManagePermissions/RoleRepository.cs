using HRMS_Core.DbContext;
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
    public class RoleRepository : Repository<HRMSRoleIdentity>, IRoleRepository
    {
        private readonly HRMSDbContext _db;

        public RoleRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<HRMSRoleIdentity>> GetAllRoles()
        {
            try
            {
                return await _db.Set<HRMSRoleIdentity>().FromSqlInterpolated($"EXEC GetAllRoles").ToListAsync();
            }
            catch
            {
                return new List<HRMSRoleIdentity>();
            }
        }

       
        public async Task<VMCommonResult> CreateRole(HRMSRoleIdentity role)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageHRMSRole
                    @Action = {"CREATE"},
                    @Name = {role.Name},
                    @Description = {role.Description},
                    @Slug = {role.Slug},
                    @IsDeleted = {role.IsDeleted},
                    @IsEnabled = {role.IsEnabled}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateRole(HRMSRoleIdentity role)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageHRMSRole
                    @Action = {"UPDATE"},
                    @RoleId = {role.Id},
                    @Name = {role.Name},
                    @Description = {role.Description},
                    @Slug = {role.Slug}
                    
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteRole(DeleteRecordVM record)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageHRMSRole
                    @Action = {"DELETE"},
                    @RoleId = {record.Id},
                    @IsDeleted = 1,
                    @IsEnabled = 0
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
