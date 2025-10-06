using HRMS_Core.DbContext;
using HRMS_Core.VM.Leave;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Interface.Report;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Report
{
    public class ReportRepository : IReportRepository
    {
        private readonly HRMSDbContext _db;

        public ReportRepository(HRMSDbContext db)
        {
            _db = db;
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
    }
}
