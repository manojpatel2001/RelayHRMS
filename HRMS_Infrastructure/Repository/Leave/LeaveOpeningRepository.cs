using HRMS_Core.DbContext;
using HRMS_Core.Leave;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.Leave;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Leave
{
    public class LeaveOpeningRepository : Repository<LeaveOpening>, ILeaveOpeningRepository
    {

        private HRMSDbContext _db;

        public LeaveOpeningRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<LeaveOpening> SoftDelete(DeleteRecordVM DeleteRecord)
        {
            var LeaveOpening = await _db.LeaveOpening.FirstOrDefaultAsync(asd => asd.LeaveOpeningId == DeleteRecord.Id);
            if (LeaveOpening == null)
            {
                return LeaveOpening;
            }
            else
            {
                LeaveOpening.IsEnabled = false;
                LeaveOpening.IsDeleted = true;
                LeaveOpening.DeletedDate = DateTime.UtcNow;
                LeaveOpening.DeletedBy = DeleteRecord.DeletedBy;
                return LeaveOpening;
            }
        }

        public async Task<bool> UpdateleaveOpening(LeaveOpening leaveOpening)
        {
            var existingRecord = await _db.LeaveOpening.SingleOrDefaultAsync(asd => asd.LeaveOpeningId == leaveOpening.LeaveOpeningId);
            if (existingRecord == null)
            {
                return false;
            }
            existingRecord.EMP_Id = leaveOpening.EMP_Id;
            existingRecord.comp_id = leaveOpening.comp_id;
            existingRecord.LeaveId = leaveOpening.LeaveId;
            existingRecord.Opening = leaveOpening.Opening;
            existingRecord.EffectiveDate = leaveOpening.EffectiveDate;
            existingRecord.Grade = leaveOpening.Grade;
            existingRecord.UpdatedBy = leaveOpening.UpdatedBy;
            existingRecord.UpdatedDate = DateTime.UtcNow;
            return true;
        }
    }
}
