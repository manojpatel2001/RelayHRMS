using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.Data.SqlClient;
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

        public async Task<List<AttendanceRegularizationVM>> GetAttendanceRegularization(AttendanceRegularizationSearchFilterVM attendance)
        {
            try
            {
                var searchbyParam = new SqlParameter("@SearchBy", (object?)attendance.SearchBy ?? DBNull.Value);
                var searchforParam = new SqlParameter("@SearchValue", (object?)attendance.SearchValue ?? DBNull.Value);
                var fromdateParam = new SqlParameter("@FromDate", (object?)attendance.FromDate ?? DBNull.Value);
                var todateParam = new SqlParameter("@ToDate", (object?)attendance.ToDate ?? DBNull.Value);
                var statustypeParam = new SqlParameter("@Status", (object?)attendance.Status ?? DBNull.Value);

                return await _db.Set<AttendanceRegularizationVM>()
              .FromSqlRaw("EXEC [dbo].[GetAttendanceRegularizationSearch] @SearchBy, @SearchValue, @FromDate,@ToDate, @Status",
                  searchbyParam, searchforParam, fromdateParam, todateParam, statustypeParam)
              .ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<AttendanceRegularizationVM>();
            }
        }

        public async Task<List<AttendanceRegularization>> SoftDelete(DeleteRecordVModel DeleteRecord)
        {
            var deletedRecords = new List<AttendanceRegularization>();

            foreach (var id in DeleteRecord.Id)
            {
                var attendance = await _db.AttendanceRegularization
                    .FirstOrDefaultAsync(x => x.AttendanceRegularizationId == id );

                if (attendance != null)
                {
                    attendance.IsEnabled = false;
                    attendance.IsDeleted = true;
                    attendance.DeletedDate = DateTime.UtcNow;
                    attendance.DeletedBy = DeleteRecord.DeletedBy;
                    deletedRecords.Add(attendance);

                }
            }

            return deletedRecords;
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


        public async Task<bool> Update(AttendanceRegularization Record)
        {
            var existingRecord = await _db.EmployeeInOutRecord.SingleOrDefaultAsync(asd => asd.Emp_Id == Record.EmpId && asd.For_Date == Record.ForDate);
            if (existingRecord == null)
            {
                return false;
            }

            existingRecord.In_Time = Record.InTime;
            existingRecord.Out_Time = Record.OutTime;
            //   existingRecord.Duration = Record.Duration;
            existingRecord.Reason = Record.Reason;
            existingRecord.UpdatedBy = Record.UpdatedBy;
            existingRecord.UpdatedDate = DateTime.UtcNow;
            return true;
        }

    }
}

