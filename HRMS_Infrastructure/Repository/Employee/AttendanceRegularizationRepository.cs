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
using static System.Net.Mime.MediaTypeNames;

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
                var userlogin = new SqlParameter("@LoggedInUserId", (object?)attendance.LoggedInUserId ?? DBNull.Value);

                return await _db.Set<AttendanceRegularizationVM>()
              .FromSqlRaw("EXEC [dbo].[GetAttendanceRegularizationSearch] @SearchBy, @SearchValue, @FromDate,@ToDate, @Status,@LoggedInUserId",
                  searchbyParam, searchforParam, fromdateParam, todateParam, statustypeParam, userlogin)
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


        public async Task<bool> Update(AttendanceRegularization Record , int empInOutId)
        {
            var existingRecord = await _db.EmployeeInOutRecord.SingleOrDefaultAsync(asd => asd.Emp_IO_Id==empInOutId  && asd.Emp_Id == Record.EmpId && asd.For_Date == Record.ForDate);
            if (existingRecord == null)
            {
                return false;
            }

            existingRecord.In_Time = Record.InTime;
            existingRecord.Out_Time = Record.OutTime;
            existingRecord.Duration = Record.Duration.ToString();
            existingRecord.Reason = Record.Reason;
            existingRecord.UpdatedBy = Record.UpdatedBy;
            existingRecord.UpdatedDate = DateTime.UtcNow;
            return true;
        }

        public async Task<List<EmpInOutVM>> GetEmployeeInOut(int? EmpId, DateTime? ForDate)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@EmpId", EmpId),
                    new SqlParameter("@ForDate", ForDate),              
                };

                var result = await _db.Set<EmpInOutVM>()
                    .FromSqlRaw("EXEC GetEmployeeInOut @EmpId, @ForDate ", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<EmpInOutVM>();
            }
        }

        public async Task<VMCommonResult> Create(AttendanceRegularization model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC SP_AttendanceRegularization
                    @Action = {"INSERT"},
                    @EmpId = {model.EmpId},
                    @FullName = {model.FullName},
                    @BranchName = {model.BranchName},
                    @ForDate = {model.ForDate},
                    @ShiftTime = {model.ShiftTime},
                    @InTime = {model.InTime},
                    @OutTime = {model.OutTime},
                    @Day = {model.Day},
                    @Reason = {model.Reason},
                    @Status = {model.Status},
                    @CreatedBy = {model.CreatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> Update(AttendanceRegularization model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC SP_AttendanceRegularization
                    @Action = {"UPDATE"},
                    @Id ={model.AttendanceRegularizationId}, 
                    @EmpId = {model.EmpId},
                    @FullName = {model.FullName},
                    @BranchName = {model.BranchName},
                    @ForDate = {model.ForDate},
                    @ShiftTime = {model.ShiftTime},
                    @InTime = {model.InTime},
                    @OutTime = {model.OutTime},
                    @Day = {model.Day},
                    @Reason = {model.Reason},
                    @Status = {model.Status},
                    @CreatedBy = {model.CreatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> Delete(DeleteRecordVModel deleteRecord)
        {
            try
            {
                VMCommonResult finalResult = new VMCommonResult();

                foreach (int id in deleteRecord.Id)
                {
                    var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC SP_AttendanceRegularization
                    @Action = {"DELETE"},
                    @Id = {id},
                    @CreatedBy = {deleteRecord.DeletedBy}
            ").ToListAsync();

                    finalResult = result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
                }

                return finalResult;
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<List<AttendanceDetails>> GetAttendanceDetails(EmployeeInOutFilterVM outFilterVM)
        {
            try
            {          
                var monthParam = new SqlParameter("@StartDate", (object?)outFilterVM.StartDate ?? DBNull.Value);
                var yearParam = new SqlParameter("@EndDate", (object?)outFilterVM.EndDate ?? DBNull.Value);
                var empCodeParam = new SqlParameter("@EmpId", (object?)outFilterVM.EmpId ?? DBNull.Value);

                return await _db.Set<AttendanceDetails>()
              .FromSqlRaw("EXEC SP_GetAttendanceDetails @StartDate,@EndDate,@EmpId",
                    monthParam, yearParam, empCodeParam)
              .ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<AttendanceDetails>();
            }
        }
    }
}

