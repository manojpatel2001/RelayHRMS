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
    public class ShiftMasterRepository : Repository<ShiftMaster>, IShiftMasterRepository
    {
        private HRMSDbContext _db;

        public ShiftMasterRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ShiftMaster> SoftDelete(DeleteRecordVM DeleteRecord)
        {
            var sm = await _db.ShiftMasters.FirstOrDefaultAsync(asd => asd.ShiftID == DeleteRecord.Id);
            if (sm == null)
            {
                return sm;
            }
            sm.IsEnabled = false;
            sm.IsDeleted = true;
            sm.DeletedDate = DateTime.UtcNow;
            sm.DeletedBy = DeleteRecord.DeletedBy;
            return sm;

        }

        public async Task<bool> UpdateShiftMaster(ShiftMaster shiftMaster)
        {
            var sm = await _db.ShiftMasters.SingleOrDefaultAsync(asd=>asd.ShiftID == shiftMaster.ShiftID);
            if (sm == null)
            {
                return false;
            }
            sm.IsSplitShift = shiftMaster.IsSplitShift;
            sm.ShiftName = shiftMaster.ShiftName;
            sm.AutoShiftChange = shiftMaster.AutoShiftChange;
            sm.Duration = shiftMaster.Duration;
            sm.EndTime = shiftMaster.EndTime;
            sm.IsHalfDay = shiftMaster.IsHalfDay;
            sm.IsTrainingShift = shiftMaster.IsTrainingShift;
            //sm.ShiftBreaks = shiftMaster.ShiftBreaks;
            sm.StartTime = shiftMaster.StartTime;
            sm.UpdatedBy = shiftMaster.UpdatedBy;
            sm.UpdatedDate = DateTime.UtcNow;
            return true;
        }
    }
}
