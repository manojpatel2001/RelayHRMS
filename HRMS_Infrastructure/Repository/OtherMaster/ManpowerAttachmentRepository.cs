using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.OtherMaster;
using HRMS_Infrastructure.Interface.OtherMaster;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.OtherMaster
{
    public class ManpowerAttachmentRepository: IManpowerAttachmentRepository
    {
        private readonly string _connectionString;
        private readonly HRMSDbContext _db;

        public ManpowerAttachmentRepository(HRMSDbContext db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> CreateManpowerAttachment(ManpowerAttachmentModel model)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@OperationType", "Create");
                    parameters.Add("@ManpowerRequisitionId", model.ManpowerRequisitionId);
                    parameters.Add("@DocumentTypeId", model.DocumentTypeId);
                    parameters.Add("@DocumentUrl", model.DocumentUrl);
                    parameters.Add("@Comment", model.Comment);
                    parameters.Add("@DateOfExpiry", model.DateOfExpiry);
                    parameters.Add("@CreatedBy", model.CreatedBy);
                    parameters.Add("@UpdatedBy", model.UpdatedBy);
                    parameters.Add("@DeletedBy", model.DeletedBy);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync("usp_ManpowerAttachment_CRUD", parameters, commandType: CommandType.StoredProcedure);

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = model.ManpowerAttachmentId;
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

        public async Task<APIResponse> UpdateManpowerAttachment(ManpowerAttachmentModel model)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@OperationType", "Update");
                    parameters.Add("@ManpowerAttachmentId", model.ManpowerAttachmentId);
                    parameters.Add("@ManpowerRequisitionId", model.ManpowerRequisitionId);
                    parameters.Add("@DocumentTypeId", model.DocumentTypeId);
                    parameters.Add("@DocumentUrl", model.DocumentUrl);
                    parameters.Add("@Comment", model.Comment);
                    parameters.Add("@DateOfExpiry", model.DateOfExpiry);
                    parameters.Add("@UpdatedBy", model.UpdatedBy);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync("usp_ManpowerAttachment_CRUD", parameters, commandType: CommandType.StoredProcedure);

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = model.ManpowerAttachmentId;
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

        public async Task<APIResponse> DeleteManpowerAttachment(DeleteRecordVM delete)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@OperationType", "Delete");
                    parameters.Add("@ManpowerAttachmentId", delete.Id);
                    parameters.Add("@DeletedBy", delete.DeletedBy);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync("usp_ManpowerAttachment_CRUD", parameters, commandType: CommandType.StoredProcedure);

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

        public async Task<APIResponse> GetAllManpowerAttachment(int manpowerRequisitionId)
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
                        "usp_ManpowerAttachment_CRUD",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (!parameters.Get<bool>("@Success") || !result.Any())
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
        public async Task<vmManpowerAttachment?> GetByManpowerAttachmentId(int manpowerAttachmentId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@OperationType", "GetById");
                    parameters.Add("@ManpowerAttachmentId", manpowerAttachmentId);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    var result = await connection.QueryAsync<vmManpowerAttachment>(
                        "usp_ManpowerAttachment_CRUD",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null||!result.Any())
                    {
                        return null;
                    }

                    return  result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }


}
