using HRMS_Core.DbContext;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.importData;
using HRMS_Infrastructure.Interface.Salary;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Salary
{
    public class EmpAttendanceRepository : Repository<EmpAttendanceImport>, IEmpAttendanceRepository
    {
        private readonly HRMSDbContext _db;

        public EmpAttendanceRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<EmpAttendanceVM>> GetEmpAttendanceDataAsync(SearchFilterModel filter)
        {

            try
            {
                var monthParam = new SqlParameter("@Month", (object?)filter.Month ?? DBNull.Value);
                var yearParam = new SqlParameter("@Year", (object?)filter.Year ?? DBNull.Value);
                var empCodeParam = new SqlParameter("@EmpCode", (object?)filter.EmpCode ?? DBNull.Value);
                var branchidParam = new SqlParameter("@BranchId", (object?)filter.BranchId ?? DBNull.Value);
                var startDateParam = new SqlParameter("@StartDate", (object?)filter.StartDate ?? DBNull.Value);
                var endDateParam = new SqlParameter("@EndDate", (object?)filter.EndDate ?? DBNull.Value);

                return await _db.Set<EmpAttendanceVM>()
              .FromSqlRaw("EXEC [dbo].[GetEmpAttendance] @Month, @Year, @EmpCode, @BranchId, @StartDate, @EndDate",
                  monthParam, yearParam, empCodeParam, branchidParam, startDateParam, endDateParam)
              .ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<EmpAttendanceVM>();
            }
        }

        public async Task<EmpAttendanceImport> SoftDelete(DeleteRecordVM DeleteRecord)
        {
            var Attendance = await _db.EmpAttendanceImport.FirstOrDefaultAsync(asd => asd.EmpAttendanceId == DeleteRecord.Id);
            if (Attendance == null)
            {
                return Attendance;
            }
            else
            {
                Attendance.IsEnabled = false;
                Attendance.IsDeleted = true;
                Attendance.DeletedDate = DateTime.UtcNow;
                Attendance.DeletedBy = DeleteRecord.DeletedBy;
                return Attendance;
            }
        }

        public async Task<bool> UpdateEmpAttendance(EmpAttendanceImport attendanceImport)
        {
            var existingRecord = await _db.EmpAttendanceImport.SingleOrDefaultAsync(asd => asd.EmpAttendanceId == attendanceImport.EmpAttendanceId);
            if (existingRecord == null)
            {
                return false;
            }
            existingRecord.Emp_ID = attendanceImport.Emp_ID;
            existingRecord.Cmp_ID = attendanceImport.Cmp_ID;
            existingRecord.Month = attendanceImport.Month;
            existingRecord.Year = attendanceImport.Year;
            existingRecord.Att_Detail = attendanceImport.Att_Detail;
            existingRecord.PresentDays = attendanceImport.PresentDays;
            existingRecord.WeeklyOff = attendanceImport.WeeklyOff;
            existingRecord.Holiday = attendanceImport.Holiday;
            existingRecord.Absent = attendanceImport.Absent;
            existingRecord.System_Date = attendanceImport.System_Date;
            existingRecord.Login_Id = attendanceImport.Login_Id;
            existingRecord.UpdatedBy = attendanceImport.UpdatedBy;
            existingRecord.UpdatedDate = DateTime.UtcNow;
            return true;
        }

    }
}
