using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Leave;
using HRMS_Core.VM.Report;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Interface.Report;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Report
{
    public class ReportRepository : IReportRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;
        public ReportRepository(HRMSDbContext db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<List<MobileUserViewModel>> GetActiveOrInactiveMobileUsers(string Action, int Compid)
        {
            try
            {
                var parameters = new[]
                {


                    new SqlParameter("@Action", Action),
                    new SqlParameter("@CompId", Compid)

                    };

                var result = await _db.Set<MobileUserViewModel>()
                    .FromSqlRaw("EXEC sp_GetActiveInactiveMobileuser @Action, @CompId", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<MobileUserViewModel>();
            }
        }

        public async Task<List<ActiveorInactiveUsers>> GetActiveOrInactiveUsers(string Action,int Compid )
        {
            try
            {
                var parameters = new[]
                {
           
               
                    new SqlParameter("@Action", Action),
                    new SqlParameter("@CompId", Compid) 

                    };

                var result = await _db.Set<ActiveorInactiveUsers>()
                    .FromSqlRaw("EXEC sp_GetAndUpdateUserStatus @Action, @CompId", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<ActiveorInactiveUsers>();
            }
        }

        public async Task<List<CompoffLapseReminderViewModel>> GetCompoffLapseReminder(DateTime SelectedDate, int LapseDays)
        {
            try
            {
                var parameters = new[]
                {


                    new SqlParameter("@SelectedDate", SelectedDate),
                    new SqlParameter("@LapseDays", LapseDays)

                    };

                var result = await _db.Set<CompoffLapseReminderViewModel>()
                    .FromSqlRaw("EXEC GetCompoffLapseReminder @SelectedDate, @LapseDays", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<CompoffLapseReminderViewModel>();
            }
        }

        public async Task<(List<EmployeeLeaveApplication>, List<EmployeeLeaveStatus>)>
       GetEmployeeMonthlyLeaveStatus(string EmpId, int SelectedMonth, int SelectedYear ,int CompId)
        {
            using (var connection = new SqlConnection(_db.Database.GetConnectionString()))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeId", EmpId);
                parameters.Add("@SelectedMonth", SelectedMonth);
                parameters.Add("@SelectedYear", SelectedYear);
                parameters.Add("@CompId", CompId);

                using (var multi = await connection.QueryMultipleAsync(
                    "sp_GetEmployeeMonthlyLeaveStatus",
                    parameters,
                    commandType: CommandType.StoredProcedure))
                {
                    var leaveApplications = (await multi.ReadAsync<EmployeeLeaveApplication>()).ToList();
                    var leaveStatuses = (await multi.ReadAsync<EmployeeLeaveStatus>()).ToList();

                    return (leaveApplications, leaveStatuses);
                }
            }
        }
        public async Task<List<EmployeeYearlyLeaveStatus>> GetEmployeeYearlyLeaveStatus(string empId, int compId, int year)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@Year", year);
                parameters.Add("@EmployeeId", string.IsNullOrEmpty(empId) ? (object)DBNull.Value : empId);
                parameters.Add("@CompId", compId);

                var result =( await connection.QueryAsync<EmployeeYearlyLeaveStatus>(
                    "sp_GetEmployeeYearlyLeaveStatus",
                    parameters,
                    commandType: CommandType.StoredProcedure
                )).ToList();

                return result;
            }
        }
        public async Task<List<HolidayViewModel>> GetHolidaysForYear(int Year)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Year", Year)

                    };

                var result = await _db.Set<HolidayViewModel>()
                    .FromSqlRaw("EXEC GetHolidaysForYear  @Year", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<HolidayViewModel>();
            }
        }

        public async Task<List<LeaveBalanceViewModelForAdmin>> GetLeaveBalanceForAdmin(LeaveBalance_ParamForAdmin vm)
        {
            try
            {
                // Convert List of Employee Codes to comma-separated string
                string empCodes = vm.EmployeeCodes != null && vm.EmployeeCodes.Any()
                    ? string.Join(",", vm.EmployeeCodes)
                    : string.Empty;

                if (string.IsNullOrEmpty(empCodes))
                {
                    return new List<LeaveBalanceViewModelForAdmin>();
                }

                var parameters = new[]
                {
                new SqlParameter("@CompId", (object?)vm.CompId ?? DBNull.Value),
                new SqlParameter("@EmpCodes", empCodes),  // Changed: @EmpIds to @EmpCodes
                new SqlParameter("@AsOfDate", (object?)vm.AsOfDate ?? DBNull.Value),
                new SqlParameter("@LeaveType", (object?)vm.Status ?? DBNull.Value)
            };

                var result = await _db.Set<LeaveBalanceViewModelForAdmin>()
                    .FromSqlRaw("EXEC GetLeaveBalanceAdmin @CompId, @EmpCodes, @AsOfDate, @LeaveType", parameters)
                    .ToListAsync();

                return result ?? new List<LeaveBalanceViewModelForAdmin>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetLeaveBalanceAdmin Error: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                return new List<LeaveBalanceViewModelForAdmin>();
            }
        }

        public async Task<List<UsedLeavesSummary>> GetUsedLeavesSummary()
        {
            try
            {            
                var result = await _db.Set<UsedLeavesSummary>()
                    .FromSqlRaw("EXEC GetUsedLeavesSummary")
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<UsedLeavesSummary>();
            }
        }

        public async Task<List<MonthlySalarySummaryViewModel>> GetYearlySalaryReportForAdmin(int StartYear, int EndYear)
        {
            try
            {
                var parameters = new[]
                {


                    new SqlParameter("@FinancialYearStartYear", StartYear),
                    new SqlParameter("@FinancialYearEndYear", EndYear)

                    };

                var result = await _db.Set<MonthlySalarySummaryViewModel>()
                    .FromSqlRaw("EXEC usp_GetMonthlyTotalNetSalary @FinancialYearStartYear, @FinancialYearEndYear", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<MonthlySalarySummaryViewModel>();
            }
        }

        public async Task<SP_Response> UpdateMobileUsers(UpdateMobileUserStatusRequest model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                EXEC sp_UpdateMobileUserStatus
                    @Action = {model.Action},
                    @EmpCode = {model.EmpId},
                    @UpdatedBy = {model.UpdatedBy}
            ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Some thing went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Some thing went wrong!" };
            }
        }

    }
}
