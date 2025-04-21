using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.JobMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.JobMaster
{
    public class ReasonRepository : Repository<Reason> , IReasonRepository
    {
        private readonly HRMSDbContext _db;

        public ReasonRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<Reason>> GetAllReasons()
        {
            try
            {
                return await _db.Set<Reason>()
                                .FromSqlInterpolated($"EXEC GetAllReasons")
                                .ToListAsync();
            }
            catch (Exception)
            {
                return new List<Reason>();
            }
        }

        public async Task<Reason?> GetByReasonId(int reasonId)
        {
            try
            {
                var result = await _db.Set<Reason>()
                                      .FromSqlInterpolated($"EXEC GetByReasonId @ReasonId = {reasonId}")
                                      .ToListAsync();

                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<VMCommonResult> CreateReason(Reason reason)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageReason 
                        @Action = {"CREATE"},
                        @ReasonName = {reason.ReasonName},
                        @ReasonType = {reason.ReasonType},
                        @IsActive = {reason.IsActive},
                        @GatePassType = {reason.GatePassType},
                        @IsCommentMandatory = {reason.IsCommentMandatory},
                        @IsDeleted = {reason.IsDeleted},
                        @IsEnabled = {reason.IsEnabled},
                        @IsBlocked = {reason.IsBlocked},
                        @CreatedDate = {reason.CreatedDate},
                        @CreatedBy = {reason.CreatedBy},
                        @UpdatedDate = {reason.UpdatedDate},
                        @UpdatedBy = {reason.UpdatedBy},
                        @DeletedDate = {reason.DeletedDate},
                        @DeletedBy = {reason.DeletedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateReason(Reason reason)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageReason 
                        @Action = {"UPDATE"},
                        @ReasonId = {reason.ReasonId},
                        @ReasonName = {reason.ReasonName},
                        @ReasonType = {reason.ReasonType},
                        @IsActive = {reason.IsActive},
                        @GatePassType = {reason.GatePassType},
                        @IsCommentMandatory = {reason.IsCommentMandatory},
                        @UpdatedDate = {reason.UpdatedDate},
                        @UpdatedBy = {reason.UpdatedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteReason(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageReason 
                        @Action = {"DELETE"},
                        @ReasonId = {deleteRecordVM.Id},
                        @DeletedDate = {deleteRecordVM.DeletedDate},
                        @DeletedBy = {deleteRecordVM.DeletedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }


    }
}
