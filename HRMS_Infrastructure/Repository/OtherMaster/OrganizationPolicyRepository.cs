using HRMS_Core.DbContext;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.OtherMaster;
using HRMS_Infrastructure.Interface.OtherMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.OtherMaster
{
    public class OrganizationPolicyRepository:Repository<OrganizationPolicy>, IOrganizationPolicyRepository
    {
        private HRMSDbContext _db;

        public OrganizationPolicyRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateOrganizationPolicy(vmOrganizationPolicy policy)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC CreateOrganizationPolicy 
                    @OrganizationPolicyName = {policy.OrganizationPolicyName},
                    @ToolTip = {policy.ToolTip},
                    @FromDate = {policy.FromDate},
                    @ToDate = {policy.ToDate},
                    @Sorting = {policy.Sorting},
                    @DocumentUrl = {policy.DocumentUrl},
                    @Grouping = {policy.Grouping},
                    @GroupingValue = {policy.GroupingValue},
                    @IsDeleted = {policy.IsDeleted},
                    @IsEnabled = {policy.IsEnabled},
                    @IsBlocked = {policy.IsBlocked},
                    @CreatedDate = {policy.CreatedDate},
                    @CreatedBy = {policy.CreatedBy},
                    @DeletedDate = {policy.DeletedDate},
                    @DeletedBy = {policy.DeletedBy},
                    @UpdatedDate = {policy.UpdatedDate},
                    @UpdatedBy = {policy.UpdatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult
                {
                    Id = 0,
                };
            }
            catch (Exception ex)
            {
                
                return new VMCommonResult
                {
                    Id= 0,
                };
            }
        }

        public async Task<VMCommonResult> DeleteOrganizationPolicy(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC DeleteOrganizationPolicy 
                        @OrganizationPolicyId = {deleteRecordVM.Id},
                        @DeletedBy = {deleteRecordVM.DeletedBy},
                        @DeletedDate = {deleteRecordVM.DeletedDate}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateOrganizationPolicy(vmOrganizationPolicy policy)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC UpdateOrganizationPolicy 
                        @OrganizationPolicyId = {policy.OrganizationPolicyId},
                        @OrganizationPolicyName = {policy.OrganizationPolicyName},
                        @ToolTip = {policy.ToolTip},
                        @FromDate = {policy.FromDate},
                        @ToDate = {policy.ToDate},
                        @Sorting = {policy.Sorting},
                        @DocumentUrl = {policy.DocumentUrl},
                        @Grouping = {policy.Grouping},
                        @GroupingValue = {policy.GroupingValue},
                        @UpdatedDate = {policy.UpdatedDate},
                        @UpdatedBy = {policy.UpdatedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return new VMCommonResult { Id = 0 };
            }
        }
    }
}
