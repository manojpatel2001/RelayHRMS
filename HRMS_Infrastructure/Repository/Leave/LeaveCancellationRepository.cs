using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.VM.Leave;
using HRMS_Core.VM.Report;
using HRMS_Core.VM.Salary;
using HRMS_Infrastructure.Interface.Leave;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Leave
{
    public class LeaveCancellationRepository : Repository<LeaveCancellationReportViewModel> ,ILeaveCancellationRepository
    {

        private HRMSDbContext _db;
        private readonly string _connectionString;

        public LeaveCancellationRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;

        }

        public async Task<List<LeaveCancellationReportViewModel>> GetLeavecancellationReport(LeaveCancellationReportRequest vm)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@EmployeeId", vm.EmployeeId);
                    parameters.Add("@CompanyId", vm.CompanyId);
                    parameters.Add("@LeaveTypeId", vm.LeaveTypeId);
                    parameters.Add("@RecordType", vm.RecordType);
                    parameters.Add("@Month", vm.Month);
                    parameters.Add("@Year", vm.Year);

                    var result = await connection.QueryAsync<LeaveCancellationReportViewModel>(
                        "GetLeavecancellationReportForESS",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result.AsList();
                }
            }
            catch
            {
                return new List<LeaveCancellationReportViewModel>();
            }
        }

    }
}
