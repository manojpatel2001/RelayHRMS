using HRMS_Core.DbContext;
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
    public class HRMSUserRoleRepository : Repository<HRMSUserRole>, IHRMSUserRoleRepository
    {
        private readonly HRMSDbContext _db;

        public HRMSUserRoleRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateUserRole(HRMSUserRole role)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageHRMSUserRole
                    @Action = {"CREATE"},
                     @UserRoleId={role.UserRoleId},
                    @EmployeeId = {role.EmployeeId},
                    @RoleId = {role.RoleId},
                    @CompanyId = {role.CompanyId},
                    @IsEnabled = {role.IsEnabled},
                    @IsDeleted = {role.IsDeleted},
                    @CreatedBy = {role.CreatedBy},
                    @CreatedDate = {role.CreatedDate}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateUserRole(HRMSUserRole role)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageHRMSUserRole
                    @Action = {"UPDATE"},
                    @EmployeeId = {role.EmployeeId},
                    @RoleId = {role.RoleId}
                    
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteUserRole(DeleteRecordVM record)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageHRMSUserRole
                    @Action = {"DELETE"},
                    @UserId = {record.Id}
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
