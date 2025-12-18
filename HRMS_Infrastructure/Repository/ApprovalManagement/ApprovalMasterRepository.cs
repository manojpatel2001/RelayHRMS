using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.VM.ApprovalManagement;
using HRMS_Infrastructure.Interface.ApprovalManagement;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.ApprovalManagement
{
    public class ApprovalMasterRepository: IApprovalMasterRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;


        public ApprovalMasterRepository(HRMSDbContext db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;

        }
        public async Task<APIResponse> ManageApprovalMaster(ApprovalMasterViewModel model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();

                    // Input parameters
                    parameters.Add("@Action", model.Action);
                    parameters.Add("@ApprovalMasterId", model.ApprovalMasterId);
                    parameters.Add("@ApprovalName", model.ApprovalName);
                    parameters.Add("@ApprovalTypeId", model.ApprovalTypeId);
                    parameters.Add("@CreatedBy", model.CreatedBy);
                    parameters.Add("@UpdatedBy", model.UpdatedBy);
                    parameters.Add("@DeletedBy", model.DeletedBy);

                    // OUTPUT PARAMETERS
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync(
                        "usp_ManageApprovalMaster",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return new APIResponse
                    {
                        isSuccess = parameters.Get<bool>("@Success"),
                        ResponseMessage = parameters.Get<string>("@ResponseMessage")
                    };
                }
            }
            catch (SqlException sqlEx)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "SQL Error: " + sqlEx.Message
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Error: " + ex.Message
                };
            }
        }

        public async Task<APIResponse> GetAllApprovalMasters()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var result = await connection.QueryAsync<dynamic>(
                        sql: "GetAllApprovalMasters",
                        commandType: CommandType.StoredProcedure);

                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = "ApprovalMasters retrieved successfully.",
                        Data = result
                    };
                }
            }
            catch (SqlException sqlEx)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Database error: " + sqlEx.Message,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Error retrieving ApprovalMasters: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<APIResponse> GetAllApprovalMasterList(int? approvalTypeId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ApprovalTypeId", approvalTypeId, DbType.Int32, ParameterDirection.Input);

                    var result = await connection.QueryAsync<dynamic>(
                        sql: "GetAllApprovalMasterList",
                        param: dynamicParameters,
                        commandType: CommandType.StoredProcedure);

                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = "ApprovalMaster list retrieved successfully.",
                        Data = result
                    };
                }
            }
            catch (SqlException sqlEx)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Database error: " + sqlEx.Message,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Error retrieving ApprovalMaster list: " + ex.Message,
                    Data = null
                };
            }
        }


        public async Task<APIResponse> GetAllApprovalMasterType()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var result = await connection.QueryAsync<dynamic>(
                        sql: "GetAllApprovalMasterType",
                        commandType: CommandType.StoredProcedure);

                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = "Approval master  types retrieved successfully.",
                        Data = result
                    };
                }
            }
            catch (SqlException sqlEx)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Database error: " + sqlEx.Message,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Error retrieving Approval Master types: " + ex.Message,
                    Data = null
                };
            }
        }

    }
}
