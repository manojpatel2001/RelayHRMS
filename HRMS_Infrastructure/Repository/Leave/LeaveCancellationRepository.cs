using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.VM;
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
    public class LeaveCancellationRepository : Repository<LeaveCancellationRequestVM> ,ILeaveCancellationRepository
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

        public async Task<SP_Response> CreateLeavecancellation(LeaveCancellationRequestVM model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "INSERT");
                parameters.Add("@LeaveCancellationId", model.LeaveCancellationId);
                parameters.Add("@EmployeeId", model.EmployeeId);
                parameters.Add("@FromDate", model.FromDate);
                parameters.Add("@ToDate", model.ToDate);
                parameters.Add("@NoOfDate", model.NoOfDate);
                parameters.Add("@Reason", model.Reason);
                parameters.Add("@LeaveCancelReasonId", model.LeaveCancelReasonId); // Updated parameter name
                parameters.Add("@LeaveStatus", model.LeaveStatus);
                parameters.Add("@LeaveTypeId", model.LeaveTypeId);
                parameters.Add("@CreatedBy", model.CreatedBy);
                parameters.Add("@IsEnabled", model.IsEnabled);
                parameters.Add("@IsDeleted", model.IsDeleted);
                parameters.Add("@IsApproved", model.IsApproved);
                parameters.Add("@IsRejected", model.IsRejected);

                var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                    "sp_LeaveCancellationRequest_CRUD",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }

        public async Task<SP_Response> UpdateLeavecancellation(updateLeaveCancellationRequestVM model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
      
                parameters.Add("@LeaveCancellationId", model.LeaveCancellationId);
                parameters.Add("@EmployeeId", model.EmployeeId);        
                parameters.Add("@UpdatedBy", model.UpdatedBy);           
                parameters.Add("@IsApproved", model.IsApproved);
                parameters.Add("@IsRejected", model.IsRejected);

                var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                    "sp_UpdateLeaveCancellationStatus",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }

        public async Task<SP_Response> DeleteLeavecancellation(DeleteRecordVM deleteRecord)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "DELETE");
                parameters.Add("@LeaveCancellationId", deleteRecord.Id);
                parameters.Add("@DeletedBy", deleteRecord.DeletedBy);

                // Execute the stored procedure
                var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                    "sp_LeaveCancellationRequest_CRUD",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }

        public async Task<List<EmpLeaveCancellationRequestReportViewModel>> GetEmpLeaveCancellationRequestReport(LeaveCancellationRequestFilterViewModel vm)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@FromDate", vm.FromDate);
                    parameters.Add("@ToDate", vm.ToDate);
                    parameters.Add("@LeaveStatus", vm.LeaveStatus);
                    parameters.Add("@EmployeeId", vm.EmployeeId);
         

                    var result = await connection.QueryAsync<EmpLeaveCancellationRequestReportViewModel>(
                        "GetEmpLeaveCancellationRequestReport",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result.AsList();
                }
            }
            catch
            {
                return new List<EmpLeaveCancellationRequestReportViewModel>();
            }
        }

        public async Task<List<EmpLeaveCancellationRequestReportViewModel>> GetReportingWiseLeaveCancellationRequestReport(vmLeaveCancellationReportFilter vm)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@SearchBy", vm.SearchBy);
                    parameters.Add("@SearchValue", vm.SearchValue);
                    parameters.Add("@FromDate", vm.FromDate);
                    parameters.Add("@ToDate", vm.ToDate);
                    parameters.Add("@LeaveStatus", vm.LeaveStatus);
                    parameters.Add("@EmployeeId", vm.EmployeeId);


                    var result = await connection.QueryAsync<EmpLeaveCancellationRequestReportViewModel>(
                        "GetReportingWiseLeaveCancellationRequestReport",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result.AsList();
                }
            }
            catch
            {
                return new List<EmpLeaveCancellationRequestReportViewModel>();
            }
        }
    }
}
