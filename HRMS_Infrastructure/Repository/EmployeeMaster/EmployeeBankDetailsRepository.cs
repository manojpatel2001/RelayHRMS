using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Infrastructure.Interface.EmployeeMaster;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.EmployeeMaster
{
    public class EmployeeBankDetailsRepository : IEmployeeBankDetailsRepository
    {
        private readonly string _connectionString;
        private readonly HRMSDbContext _db;

        public EmployeeBankDetailsRepository(HRMSDbContext db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> CreateEmployeeBankDetails(EmployeeBankDetailsModel model)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@BankId", model.BankId);
                    parameters.Add("@BankName", model.BankName);
                    parameters.Add("@CreatedBy", model.CreatedBy);
                    parameters.Add("@Action", "INSERT");
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync(
                        "usp_EmployeeBankDetails_Manage",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = model.BankId;
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

        public async Task<APIResponse> UpdateEmployeeBankDetails(EmployeeBankDetailsModel model)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@BankId", model.BankId);
                    parameters.Add("@BankName", model.BankName);
                    parameters.Add("@UpdatedBy", model.UpdatedBy);
                    parameters.Add("@Action", "UPDATE");
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync(
                        "usp_EmployeeBankDetails_Manage",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = model.BankId;
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

        public async Task<APIResponse> DeleteEmployeeBankDetails(DeleteRecordVM delete)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@BankId", delete.Id);
                    parameters.Add("@DeletedBy", delete.DeletedBy);
                    parameters.Add("@Action", "DELETE");
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync(
                        "usp_EmployeeBankDetails_Manage",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = delete.Id;
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

        public async Task<APIResponse> GetAllEmployeeBankDetails()
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var result = await connection.QueryAsync<dynamic>(
                        "GetAllEmployeeBankDetails",
                        commandType: CommandType.StoredProcedure
                    );

                    if (!result.Any())
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "No records found.";
                        return response;
                    }

                    response.isSuccess = true;
                    response.ResponseMessage = "Success!";
                    response.Data = result;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
                response.Data = null;
            }
            return response;
        }
    }
}
