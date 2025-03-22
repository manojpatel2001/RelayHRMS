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

        public async Task<Reason> SoftDelete(DeleteRecordVM DeleteRecord)
        {
            var reason = await _db.Reasons.FirstOrDefaultAsync(asd => asd.ReasonId == DeleteRecord.Id);
            if (reason == null)
            {
                return reason;
            }
            else
            {
                reason.IsEnabled = false;
                reason.IsDeleted = true;
                reason.DeletedDate = DateTime.UtcNow;
                reason.DeletedBy = DeleteRecord.DeletedBy;
                return reason;
            }
        }

        public async Task<bool> UpdateReason(Reason reason)
        {
            var existingRecord = await _db.Reasons.SingleOrDefaultAsync(asd => asd.ReasonId== reason.ReasonId);
            if (existingRecord == null)
            {
                return false;
            }
            existingRecord.ReasonName = reason.ReasonName;
            existingRecord.ReasonType = reason.ReasonType;
            existingRecord.IsCommentMandatory = reason.IsCommentMandatory;
            existingRecord.UpdatedBy = reason.UpdatedBy;
            existingRecord.UpdatedDate = DateTime.UtcNow;
            return true;
        }
    }
}
