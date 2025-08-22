using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Core.VM.Ess.InOut;
using HRMS_Core.VM.importData;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class EmployeeInOutRepository : Repository<EmployeeInOutRecord>, IEmployeeInOutRepository
    {
        private readonly HRMSDbContext _db;

        public EmployeeInOutRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<AttendanceInOutReportVM>> AttendancefirstInOutReport(int empid, string Month, string Year)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@empid", empid),
                    new SqlParameter("@month", Month),
                    new SqlParameter("@year", Year)
                };

                var result = await _db.Set<AttendanceInOutReportVM>()
                    .FromSqlRaw("EXEC AttendanceReport_First_In_Out_Sp @empid, @month, @year", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<AttendanceInOutReportVM>();
            }
        }

        public async Task<List<AttendanceInOutReportVM>> AttendanceMultipleInOutReport(int empid, string Month, string Year)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@empid", empid),
                    new SqlParameter("@month", Month),
                    new SqlParameter("@year", Year)
                };

                var result = await _db.Set<AttendanceInOutReportVM>()
                    .FromSqlRaw("EXEC AttendanceReport_Multiple_In_Out_Sp @empid, @month, @year", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<AttendanceInOutReportVM>();
            }
        }

        public async Task<VMCommonResult> CreateEmpInOut(vmInOut Record)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC USP_InsertAttendanceLog
                        @EmployeeId = {Record.EmployeeId},
                        @CompanyId = {Record.CompanyId},
                        @Mode = {Record.Mode},
                        @PunchDateTime = {Record.PunchDateTime},
                        @CreatedBy = {Record.CreatedBy}
                    
                ").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = null };
            }
        }

 
        public async Task<List<EmployeeInOutReportVM>> GetEmployeeInOutReport(EmployeeInOutFilterVM outFilterVM)
        {
            try
            {
                var empCodeParam = new SqlParameter("@EmpId", (object?)outFilterVM.EmpId ?? DBNull.Value);
                var monthParam = new SqlParameter("@StartDate", (object?)outFilterVM.StartDate ?? DBNull.Value);
                var yearParam = new SqlParameter("@EndDate", (object?)outFilterVM.EndDate ?? DBNull.Value);
                var recordtypeParam = new SqlParameter("@RecordType", (object?)outFilterVM.RecordType ?? DBNull.Value);

                return await _db.Set<EmployeeInOutReportVM>()
              .FromSqlRaw("EXEC [dbo].[GetEmployeeInOutReport]  @EmpId, @StartDate,@EndDate, @RecordType",
                   empCodeParam, monthParam, yearParam, recordtypeParam)
              .ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<EmployeeInOutReportVM>();
            }
        }


        public async Task<List<VMInOutRecord>> GetInOutRecord(int empid, string month, string year)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@empid", empid),
                    new SqlParameter("@month", month),
                    new SqlParameter("@year", year)
                };

                var result = await _db.Set<VMInOutRecord>()
                    .FromSqlRaw("EXEC First_In_Out_Sp @empid, @month, @year", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<VMInOutRecord>();
            }
        }



        public async Task<List<vmGetMonthlyAttendanceLog>> GetMonthlyAttendanceLog(vmInOutParameter vmInOutParameter)
        {
            try
            {
                var result= await _db.Set<vmGetMonthlyAttendanceLog>()
                                .FromSqlInterpolated($"EXEC GetMonthlyAttendanceLog  @MonthNumber={vmInOutParameter.MonthNumber}, @Year={vmInOutParameter.year},  @EmployeeId = {vmInOutParameter.EmployeeId},@CompanyId={vmInOutParameter.CompanyId}")
                                .ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<vmGetMonthlyAttendanceLog>();
            }
        }
        public async Task<List<vmGetMonthlyAttendanceDetails>> GetMonthlyAttendanceDetails(vmInOutParameter vmInOutParameter)
        {
            try
            {
                var result= await _db.Set<vmGetMonthlyAttendanceDetails>()
                                .FromSqlInterpolated($"EXEC GetMonthlyAttendanceDetails  @MonthNumber={vmInOutParameter.MonthNumber}, @Year={vmInOutParameter.year},  @EmployeeId = {vmInOutParameter.EmployeeId},@CompanyId={vmInOutParameter.CompanyId}")
                                .ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<vmGetMonthlyAttendanceDetails>();
            }
        }
        public async Task<List<vmGetMonthlyAttendanceDetails>> GetDateWiseAttendanceDetails(vmInOutParameter vmInOutParameter)
        {
            try
            {
                var result= await _db.Set<vmGetMonthlyAttendanceDetails>()
                                .FromSqlInterpolated($"EXEC GetDateWiseAttendanceDetails  @FromDate={vmInOutParameter.FromDate}, @ToDate={vmInOutParameter.ToDate},  @EmployeeId = {vmInOutParameter.EmployeeId},@CompanyId={vmInOutParameter.CompanyId},@MemberId={vmInOutParameter.MemberId}")
                                .ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<vmGetMonthlyAttendanceDetails>();
            }
        }

        public async Task<List<vmGetEmployeesByReportingManager>> GetEmployeesByReportingManager(int EmployeeId)
        {
            try
            {
                var result = await _db.Set<vmGetEmployeesByReportingManager>()
                                .FromSqlInterpolated($"EXEC GetEmployeesByReportingManager  @EmployeeId={EmployeeId}")
                                .ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<vmGetEmployeesByReportingManager>();
            }
        }
        public async Task<List<VMInOutRecord>> GetMultipleInOutRecordAsync(int empid, string Month, string Year)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@empid", empid),
                    new SqlParameter("@month", Month),
                    new SqlParameter("@year", Year)
                };

                var result = await _db.Set<VMInOutRecord>()
                    .FromSqlRaw("EXEC Multiple_In_Out_Sp @empid, @month, @year", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<VMInOutRecord>();
            }
        }

        public async Task<bool> UpdateEmployeeOutTimeAsync(int empId, DateTime forDate, DateTime outTime, string updatedBy)
        {
            try
            {
                string sql = @"
                    EXEC SPUpdateOutTimeEmployee 
                        @Emp_Id = {0}, 
                        @For_Date = {1}, 
                        @Out_Time = {2}, 
                        @UpdatedBy = {3}";

                int rowsAffected = await _db.Database.ExecuteSqlRawAsync(sql, empId, forDate, outTime, updatedBy);
                return rowsAffected > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<VMCommonResult> CreateAttendanceDetails(AttendanceDetailsViewModel Record)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC SP_AttendanceDetails
                        @Action = {"INSERT"},
                        @EmployeeId = {Record.EmployeeId},
                        @ShiftDate = {Record.ShiftDate},
                        @InTime = {Record.InTime},
                        @OutTime = {Record.OutTime},
                        @WorkingHours = {Record.WorkingHours},                
                        @SalaryDay = {Record.SalaryDay},
                        @CreatedOn = {Record.CreatedOn}
                    
                ").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = null };
            }
        }


        public async Task<VMCommonResult> UpdateAttendanceDetails(AttendanceDetailsViewModel model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC SP_AttendanceDetails
                        @Action = {"UPDATE"},
                         @Id = {model.AttendanceDetailsid},
                        @EmployeeId = {model.EmployeeId},
                        @ShiftDate = {model.ShiftDate},
                        @InTime = {model.InTime},
                        @OutTime = {model.OutTime},
                        @WorkingHours = {model.WorkingHours},              
                        @SalaryDay = {model.SalaryDay},
                        @CreatedOn = {model.CreatedOn}
                    
                ").ToListAsync();
                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = null };
            }
        }
    }
}
