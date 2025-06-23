using HRMS_Core.DbContext;
using HRMS_Core.PrivilegeSetting;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.PrivilegeSetting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.PrivilegeSetting
{
    public class PrivilegeDetailsRepository : Repository<PrivilegeDetails>, IPrivilegeDetailsRepository
    {
        private readonly HRMSDbContext _db;

        public PrivilegeDetailsRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreatePrivilegeDetails(PrivilegeDetails privilegeDetails)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC [dbo].[ManagePrivilegeDetails]
                @Action = {"CREATE"},
                @PrivilegeDetailsId = {privilegeDetails.PrivilegeDetailsId},
                @PrivilegeMasterId = {privilegeDetails.PrivilegeMasterId},
                @PageId = {privilegeDetails.PageId},
                @CompanyId = {privilegeDetails.CompanyId},
                @Is_View = {privilegeDetails.Is_View},
                @Is_Edit = {privilegeDetails.Is_Edit},
                @Is_Save = {privilegeDetails.Is_Save},
                @Is_Delete = {privilegeDetails.Is_Delete},
                @CreatedDate = {privilegeDetails.CreatedDate},
                @CreatedBy = {privilegeDetails.CreatedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception ex)
            {
                // Log exception details here if needed
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<VMCommonResult> UpdatePrivilegeDetails(PrivilegeDetails privilegeDetails)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC [dbo].[ManagePrivilegeDetails]
                @Action = {"UPDATE"},
                @PrivilegeDetailsId = {privilegeDetails.PrivilegeDetailsId},
                @PrivilegeMasterId = {privilegeDetails.PrivilegeMasterId},
                @PageId = {privilegeDetails.PageId},
                @CompanyId = {privilegeDetails.CompanyId},
                @Is_View = {privilegeDetails.Is_View},
                @Is_Edit = {privilegeDetails.Is_Edit},
                @Is_Save = {privilegeDetails.Is_Save},
                @Is_Delete = {privilegeDetails.Is_Delete},
                @PrivilegeName = {privilegeDetails.PrivilegeMaster?.PrivilegeName},
                @BranchId_Multi = {privilegeDetails.PrivilegeMaster?.BranchId_Multi},
                @DepartmentId_Multi = {privilegeDetails.PrivilegeMaster?.DepartmentId_Multi},
                @VerticalId_Multi = {privilegeDetails.PrivilegeMaster?.VerticalId_Multi},
                @PrivilegeType = {privilegeDetails.PrivilegeMaster?.PrivilegeType},
                @UpdatedDate = {DateTime.UtcNow},
                @UpdatedBy = {privilegeDetails.UpdatedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception ex)
            {
                // Log exception details here if needed
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeletePrivilegeDetails(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC [dbo].[ManagePrivilegeDetails]
                @Action = {"DELETE"},
                @PrivilegeDetailsId = {deleteRecord.Id},
                @DeletedDate = {DateTime.UtcNow},
                @DeletedBy = {deleteRecord.DeletedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception ex)
            {
                // Log exception details here if needed
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<PrivilegeDetails?> GetPrivilegeDetailsById(int privilegeDetailsId)
        {
            try
            {
                var result = await _db.Set<PrivilegeDetails>()
                                      .FromSqlInterpolated($"EXEC GetPrivilegeDetailsById @PrivilegeDetailsId = {privilegeDetailsId}")
                                      .ToListAsync();
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Log exception details here if needed
                return null;
            }
        }

    }
}
