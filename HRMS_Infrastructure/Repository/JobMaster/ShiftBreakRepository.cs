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
    public class ShiftBreakRepository : Repository<ShiftBreak>, IShiftBreakRepository
    {
        private HRMSDbContext _db;

        public ShiftBreakRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ShiftBreak> SoftDelete(DeleteRecordVM DeleteRecord)
        {
            var shift = await _db.ShiftBreaks.FirstOrDefaultAsync(asd => asd.BreakID == DeleteRecord.Id);
            if (shift == null)
            {
                return shift;
            }
            else
            {
                shift.IsEnabled = false;
                shift.IsDeleted = true;
                shift.DeletedDate = DateTime.UtcNow;
                shift.DeletedBy = DeleteRecord.DeletedBy;
                return shift;
            }
        }

        public async Task<bool> UpdateShiftBreak(ShiftBreak shiftBreak)
        {
            var shift = await _db.ShiftBreaks.FirstOrDefaultAsync(asd => asd.BreakID == shiftBreak.BreakID);
            if (shift == null)
            {
                return false;
            }
            shift.StartTime = shiftBreak.StartTime;
            shift.EndTime = shiftBreak.EndTime;
            shift.BreakName = shiftBreak.BreakName;
            shift.DeductHour = shiftBreak.DeductHour;
            shift.Duration = shiftBreak.Duration;
            shift.DeductBreak = shiftBreak.DeductBreak;
            shift.UpdatedBy = shiftBreak.UpdatedBy;
            shift.UpdatedDate = DateTime.UtcNow;
            return true;
        }
    }
}
