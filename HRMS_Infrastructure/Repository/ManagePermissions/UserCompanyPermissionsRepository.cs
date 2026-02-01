using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyInformation;
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
    public class UserCompanyPermissionsRepository: IUserCompanyPermissionsRepository
    {
        private readonly HRMSDbContext _db;

        public UserCompanyPermissionsRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<SP_Response> CreateUserCompanyPermissions(VMUserCompanyPermission model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                EXEC ManageUserCompanyPermission
                    @Action = {"CREATE"},
                    @EmployeeId = {model.EmployeeId},
                    @CompanyId = {model.CompanyId},
                    @IsAdmin = {model.IsAdmin},
                    @CreatedBy = {model.CreatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new SP_Response { Success = 0,ResponseMessage="Some thing went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Some thing went wrong!" };
            }
        }

        public async Task<SP_Response> UpdateUserCompanyPermissions(VMUserCompanyPermission model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                EXEC ManageUserCompanyPermission
                    @Action = {"UPDATE"},
                    @UserCompanyPermissionId = {model.UserCompanyPermissionId},
                    @EmployeeId = {model.EmployeeId},
                    @CompanyId = {model.CompanyId},
                    @IsAdmin = {model.IsAdmin},
                    @UpdatedDate = {model.UpdatedDate},
                    @UpdatedBy = {model.UpdatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Some thing went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Some thing went wrong!" };
            }
        }

        public async Task<SP_Response> DeleteUserCompanyPermissions(DeleteRecordVM model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                EXEC ManageUserCompanyPermission
                    @Action = {"DELETE"},
                    @UserCompanyPermissionId = {model.Id},
                    @DeletedDate = {model.DeletedDate},
                    @DeletedBy = {model.DeletedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Some thing went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Some thing went wrong!" };
            }
        }
        public async Task<List<vmGetAllCompanyDetailsList>> GetCompanyPermissionsListByEmployeeId(int EmployeeId)
        {
            try
            {
                var result = await _db.Set<vmGetAllCompanyDetailsList>().FromSqlInterpolated($@"
                EXEC GetCompanyPermissionsListByEmployeeId
                    @EmployeeId = {EmployeeId}
            ").ToListAsync();

                return result;
            }
            catch
            {
                return new List<vmGetAllCompanyDetailsList>() ;
            }
        }
    }
}
