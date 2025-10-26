using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.VM.OtherMaster;
using HRMS_Infrastructure.Interface.OtherMaster;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.OtherMaster
{
    public class LeaveTransactionRepository: ILeaveTransactionRepository
    {

        private HRMSDbContext _db;
        private readonly string _connectionString;

        public LeaveTransactionRepository(HRMSDbContext db) 
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }
        public async Task<APIResponse> InsertLeaveTransaction(InsertLeaveTransactionRequest request)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@EmpId", request.EmpId ?? (object)DBNull.Value);
                    parameters.Add("@LeaveTypeId", request.LeaveTypeId ?? (object)DBNull.Value);
                    parameters.Add("@TransactionType", request.TransactionType ?? (object)DBNull.Value);
                    parameters.Add("@LeaveValue", request.LeaveValue ?? (object)DBNull.Value);
                    parameters.Add("@Reason", request.Reason ?? (object)DBNull.Value);
                    parameters.Add("@CreatedBy", request.CreatedBy ?? (object)DBNull.Value);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync(
                        "Manage_InsertLeaveTransaction",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = null; // No data to return, just success/failure
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred: {ex.Message}";
                response.Data = null;
            }
            return response;
        }

        public async Task<APIResponse> GetAllLeaveTypeByCompanyId(int companyId)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var result = await connection.QueryAsync<dynamic>(
                        "GetAllLeaveTypeByCompanyId",
                        new { CompanyId = companyId },
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null || !result.Any())
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "No records found.";
                        return response;
                    }

                    response.isSuccess = true;
                    response.ResponseMessage = "Success!";
                    response.Data = result; // Return the list of leave types
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred: {ex.Message}";
                response.Data = null;
            }
            return response;
        }

        public async Task<APIResponse> GetLeaveBalanceReport(int companyId)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var result = await connection.QueryAsync<dynamic>(
                        "GetLeaveBalanceReport",
                        new { CompId = companyId },
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null || !result.Any())
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "No records found.";
                        return response;
                    }

                    response.isSuccess = true;
                    response.ResponseMessage = "Success!";
                    response.Data = result; // Return the list of leave types
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred: {ex.Message}";
                response.Data = null;
            }
            return response;
        }



    }
}
