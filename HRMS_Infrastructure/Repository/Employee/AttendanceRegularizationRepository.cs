using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class AttendanceRegularizationRepository : Repository<AttendanceRegularization>, IAttendanceRegularizationRepository
    {
        private readonly HRMSDbContext _db;

        public AttendanceRegularizationRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<AttendanceRegularization> SoftDelete(DeleteRecordVM DeleteRecord)
        {

            var attendance = await _db.AttendanceRegularization.FirstOrDefaultAsync(asd => asd.AttendanceRegularizationId == DeleteRecord.Id);
            if (attendance == null)
            {
                return attendance;
            }
            else
            {
                attendance.IsEnabled = false;
                attendance.IsDeleted = true;
                attendance.DeletedDate = DateTime.UtcNow;
                attendance.DeletedBy = DeleteRecord.DeletedBy;
                return attendance;
            }
        }


        public async Task<bool> UpdateAttendanceRegularization(AttendanceRegularization updatedRecord)
        {

            var existingRecord = await _db.AttendanceRegularization
                .SingleOrDefaultAsync(x => x.AttendanceRegularizationId == updatedRecord.AttendanceRegularizationId);

            if (existingRecord == null)
            {
                return false; // Record not found
            }

            existingRecord.EmpId = updatedRecord.EmpId;
            existingRecord.FullName = updatedRecord.FullName;
            existingRecord.BranchName = updatedRecord.BranchName;
            existingRecord.ForDate = updatedRecord.ForDate;
            existingRecord.ShiftTime = updatedRecord.ShiftTime;
            existingRecord.InTime = updatedRecord.InTime;
            existingRecord.OutTime = updatedRecord.OutTime;
            existingRecord.Duration = updatedRecord.Duration;
            existingRecord.Status = updatedRecord.Status;
            existingRecord.Reason = updatedRecord.Reason;
            existingRecord.IsApproved = updatedRecord.IsApproved;
            existingRecord.IsPending = updatedRecord.IsPending;
            existingRecord.IsRejected = updatedRecord.IsRejected;
            existingRecord.UpdatedBy = updatedRecord.UpdatedBy;
            existingRecord.UpdatedDate = DateTime.UtcNow;
            return true;
        }

    }
}

