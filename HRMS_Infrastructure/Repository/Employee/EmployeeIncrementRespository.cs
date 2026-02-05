using Azure;
using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Infrastructure.Interface.Employee;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class EmployeeIncrementRespository: IEmployeeIncrementRespository
    {
        private HRMSDbContext _db;
        private readonly string _connectionString;

        public EmployeeIncrementRespository(HRMSDbContext db) 
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }
        public async Task<List<IncrementReason>> GetAllIncrementReason()
        {
            try
            {
                var result = await _db.Set<IncrementReason>()
                                      .FromSqlInterpolated($"EXEC GetAllIncrementReason")
                                      .ToListAsync();

                return result ?? new List<IncrementReason>();
            }
            catch (Exception ex)
            {
                return new List<IncrementReason>();
            }
        }

        public async Task<APIResponse> GetAllIncrementEmployees(int companyId)
        {
            var response = new APIResponse();

            try
            {
                using var connection = new SqlConnection(_connectionString);

                var result = await connection.QueryAsync<vmGetAllIncrementEmployees>(
                    "GetAllIncrementEmployees",
                    new { CompanyId = companyId },
                    commandType: CommandType.StoredProcedure
                );

                var list = result?.ToList() ?? new List<vmGetAllIncrementEmployees>();

                if (!list.Any())
                {
                    response.isSuccess = false;
                    response.ResponseMessage = "No increment records found";
                    response.Data = list;
                }
                else
                {
                    response.isSuccess = true;
                    response.ResponseMessage = $"{list.Count} records fetched successfully";
                    response.Data = list;
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


        public async Task<APIResponse> DeleteIncrement(int employeeId)
        {
            var response = new APIResponse();

            try
            {
                using var connection = new SqlConnection(_connectionString);

                var parameters = new DynamicParameters();
                parameters.Add("@Action", "DELETE");
                parameters.Add("@EmployeeId", employeeId);

                var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                    "SP_EmployeeIncrementSalary",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (result != null && result.Success > 0)
                {
                    response.isSuccess = true;
                    response.ResponseMessage = result.ResponseMessage;
                }
                else
                {
                    response.isSuccess = false;
                    response.ResponseMessage = result?.ResponseMessage ?? "Delete failed";
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }

            return response;
        }

        public async Task<APIResponse> GetEmployeeSalaryInfo(int employeeId)
        {
            var response = new APIResponse();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var result = await connection.QueryFirstOrDefaultAsync<EmployeeSalaryInfoVM>(
                        "GetEmployeeSalaryInfo",
                        new { EmployeeId = employeeId },
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null)
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "Employee salary info not found.";
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
            }

            return response;
        }
        public async Task<APIResponse> GetEmployeeSalaryInfoByCompnayId(int companyId)
        {
            var response = new APIResponse();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var result = await connection.QueryAsync<dynamic>(
                        "GetEmployeeSalaryInfoByCompnayId",
                        new { CompanyId = companyId },
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null)
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "Employee salary info not found.";
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
            }

            return response;
        }

        public async Task<APIResponse> InsertEmployeeSalaryHistory(InsertEmployeeSalaryHistoryVM model)
        {
            var response = new APIResponse();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@EmployeeId", model.EmployeeId);
                    parameters.Add("@OldGrossSalary", model.OldGrossSalary);
                    parameters.Add("@NewGrossSalary", model.NewGrossSalary);
                    parameters.Add("@OldBasicSalary", model.OldBasicSalary);
                    parameters.Add("@NewBasicSalary", model.NewBasicSalary);
                    parameters.Add("@EffectiveFromDate", model.EffectiveFromDate);
                    parameters.Add("@ReasonId", model.ReasonId);
                    parameters.Add("@IsActive", model.IsActive);
                    parameters.Add("@CreatedBy", model.CreatedBy);

                    // 🔹 OUTPUT parameters
                    parameters.Add("@ResponseMessage", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync(
                        "USP_InsertEmployeeSalaryHistory",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }

            return response;
        }

    }
}

