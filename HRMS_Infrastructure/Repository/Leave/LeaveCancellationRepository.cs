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
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@CreatedBy", model.CreatedBy);
                    parameters.Add("@LeaveCancelReasonId", model.LeaveCancelReasonId);
                    parameters.Add("@LeaveApplicationId", model.LeaveApplicationId);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_LeaveCancellationRequest_CRUD",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result;
                }
            }
            catch (SqlException sqlEx)
            {
     
                throw new Exception($"Database error: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                // Log general exceptions
                // You can also return a custom SP_Response indicating failure
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }


        public async Task<SP_Response> UpdateLeavecancellation(updateLeaveCancellationRequestVM model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
      
                parameters.Add("@LeaveCancellationId", model.LeaveCancellationId);              
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

        public async Task<LeaveCancellationRequestVM?> GetLeavecancellationById(int leaveCancellationId)
        {

            try
            {
                var result = await _db.Set<LeaveCancellationRequestVM>()
                    .FromSqlInterpolated($"EXEC sp_GetLeaveCancellationById @leaveCancellationId = {leaveCancellationId}")
                    .ToListAsync();
                return result.FirstOrDefault() ?? null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<EmpLeaveCancellationRequestReportViewModel>> GetLeaveCancellationReportAdmin(vmLeaveCancellationReportFilterAdmin vm)
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
                    parameters.Add("@CompanyId", vm.Companyid);


                    var result = await connection.QueryAsync<EmpLeaveCancellationRequestReportViewModel>(
                        "GetLeaveCancellationReportAdmin",
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
