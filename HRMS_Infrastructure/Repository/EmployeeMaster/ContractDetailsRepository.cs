using HRMS_Core.DbContext;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface.EmployeeMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.EmployeeMaster
{
    public class ContractDetailsRepository : IContractDetailsRepository
    {
        private readonly HRMSDbContext _db;

        public ContractDetailsRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateContractDetail(ContractDetails model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageContractDetails
                    @Action = {"CREATE"},
                    @EmployeeId = {model.EmployeeId},
                    @ProjectDetailsId = {model.ProjectDetailsId},
                    @StartDate = {model.StartDate},
                    @EndDate = {model.EndDate},
                    @Comment = {model.Comment},
                    @IsRenew = {model.IsRenew},
                    @IsContractRenew = {model.IsContractRenew},
                    @CreatedBy = {model.CreatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateContractDetail(ContractDetails model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageContractDetails
                    @Action = {"UPDATE"},
                    @ContractDetailsId = {model.ContractDetailsId},
                    @EmployeeId = {model.EmployeeId},
                    @ProjectDetailsId = {model.ProjectDetailsId},
                    @StartDate = {model.StartDate},
                    @EndDate = {model.EndDate},
                    @Comment = {model.Comment},
                    @IsRenew = {model.IsRenew},
                    @IsContractRenew = {model.IsContractRenew},
                    @UpdatedBy = {model.UpdatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteContractDetail(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageContractDetails
                    @Action = {"DELETE"},
                    @ContractDetailsId = {deleteRecord.Id},
                    @DeletedBy = {deleteRecord.DeletedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<ContractDetails?> GetContractDetailById(vmCommonGetById vmCommonGetById)
        {
            try
            {
                var result = await _db.Set<ContractDetails>().FromSqlInterpolated($@"
                EXEC GetContractDetailById
                    @ContractDetailsId = {vmCommonGetById.Id},
                    @IsDeleted = {vmCommonGetById.IsDeleted},
                    @IsEnabled = {vmCommonGetById.IsEnabled}
            ").ToListAsync();

                return result?.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<ContractDetails>> GetAllContractDetails(vmCommonGetById vmCommonGetById)
        {
            try
            {
                var result = await _db.Set<ContractDetails>().FromSqlInterpolated($@"
                EXEC GetAllContractDetails
                    @EmployeeId = {vmCommonGetById.Id},
                    @IsDeleted = {vmCommonGetById.IsDeleted},
                    @IsEnabled = {vmCommonGetById.IsEnabled}
            ").ToListAsync();

                return result;
            }
            catch
            {
                return new List<ContractDetails>();
            }
        }
    }

}
