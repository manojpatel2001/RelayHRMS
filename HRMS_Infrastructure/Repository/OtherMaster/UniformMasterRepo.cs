using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface.OtherMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.OtherMaster
{
    public class UniformMasterRepo: Repository<UniformMaster>, IUniformMasterRepo
    {

        private readonly HRMSDbContext _db;
        public UniformMasterRepo(HRMSDbContext hRMSDbContext) : base(hRMSDbContext)
        {
            _db = hRMSDbContext;
        }

        public async Task<SP_Response> CreateUniformMaster(UniformMaster model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
            EXEC sp_UniformMaster_CRUD
            @Operation = {"CREATE"},
            @UniformName = {model.UniformName},
            @EffectiveDate = {model.EffectiveDate},
            @UniformRate = {model.UniformRate},
            @UniformDeductInstallment = {model.UniformDeductInstallment},
            @UniformRefundInstallment = {model.UniformRefundInstallment},
            @IsDeleted = {model.IsDeleted},
            @IsEnabled = {model.IsEnabled},
            @CreatedBy = {model.CreatedBy}
        ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> DeleteUniformMaster(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
            EXEC sp_UniformMaster_CRUD
            @Operation = {"DELETE"},
            @UniformID = {deleteRecord.Id},
            @DeletedBy = {deleteRecord.DeletedBy}
        ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> UpdateUniformMaster(UniformMaster model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
            EXEC sp_UniformMaster_CRUD
            @Operation = {"UPDATE"},
            @UniformID = {model.UniformID},
            @UniformName = {model.UniformName},
            @EffectiveDate = {model.EffectiveDate},
            @UniformRate = {model.UniformRate},
            @UniformDeductInstallment = {model.UniformDeductInstallment},
            @UniformRefundInstallment = {model.UniformRefundInstallment},
            @IsDeleted = {model.IsDeleted},
            @IsEnabled = {model.IsEnabled},
            @UpdatedBy = {model.UpdatedBy}
        ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }
    }
}
