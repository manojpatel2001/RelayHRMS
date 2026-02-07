using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.OtherMaster;
using HRMS_Infrastructure.Interface.OtherMaster;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HRMS_Infrastructure.Repository.OtherMaster
{
    public class ManpowerRelationRepository: IManpowerRelationRepository
    {
        private readonly string _connectionString;
        private readonly HRMSDbContext _db;

        public ManpowerRelationRepository(HRMSDbContext db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> CreateManpowerRelation(ManpowerRelationModel model)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@OperationType", "Create");
                    parameters.Add("@Name", model.Name);
                    parameters.Add("@ManpowerRequisitionId", model.ManpowerRequisitionId);
                    parameters.Add("@RelationShipId", model.RelationShipId);
                    parameters.Add("@MobileNo", model.MobileNo);
                    parameters.Add("@CreatedBy", model.CreatedBy);
                    parameters.Add("@UpdatedBy", model.UpdatedBy);
                    parameters.Add("@DeletedBy", model.DeletedBy);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync("usp_ManpowerRelation_CRUD", parameters, commandType: CommandType.StoredProcedure);

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = model.ManpowerRelationId;
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

        public async Task<APIResponse> UpdateManpowerRelation(ManpowerRelationModel model)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@OperationType", "Update");
                    parameters.Add("@ManpowerRelationId", model.ManpowerRelationId);
                    parameters.Add("@Name", model.Name);
                    parameters.Add("@ManpowerRequisitionId", model.ManpowerRequisitionId);
                    parameters.Add("@RelationShipId", model.RelationShipId);
                    parameters.Add("@MobileNo", model.MobileNo);
                    parameters.Add("@UpdatedBy", model.UpdatedBy);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync("usp_ManpowerRelation_CRUD", parameters, commandType: CommandType.StoredProcedure);

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = model.ManpowerRelationId;
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

        public async Task<APIResponse> DeleteManpowerRelation(DeleteRecordVM delete)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@OperationType", "Delete");
                    parameters.Add("@ManpowerRelationId", delete.Id);
                    parameters.Add("@DeletedBy", delete.DeletedBy);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync("usp_ManpowerRelation_CRUD", parameters, commandType: CommandType.StoredProcedure);

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

        public async Task<APIResponse> GetAllManpowerRelation(int manpowerRequisitionId)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@OperationType", "GetAll");
                    parameters.Add("@ManpowerRequisitionId", manpowerRequisitionId);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    var result = await connection.QueryAsync<dynamic>(
                        "usp_ManpowerRelation_CRUD",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (!parameters.Get<bool>("@Success") ||!result.Any())
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
