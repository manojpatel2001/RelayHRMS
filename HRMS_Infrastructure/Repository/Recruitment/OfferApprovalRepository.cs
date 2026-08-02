using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Recruitment;
using HRMS_Infrastructure.Interface.Recruitment;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Recruitment
{
    public class OfferApprovalRepository : IOfferApprovalRepository
    {
        private readonly string _connectionString;

        public OfferApprovalRepository(HRMSDbContext db)
        {
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> GetLevelConfig(int? companyId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<OfferApprovalLevelConfig>("GetOfferApprovalLevelConfig", new { CompanyId = companyId }, commandType: CommandType.StoredProcedure);
                response.isSuccess = true;
                response.ResponseMessage = "Success!";
                response.Data = result.AsList();
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> CreateLevelConfig(OfferApprovalLevelConfig model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "INSERT");
                parameters.Add("@CompanyId", model.CompanyId);
                parameters.Add("@LevelNo", model.LevelNo);
                parameters.Add("@ApproverRole", model.ApproverRole);
                parameters.Add("@FixedApproverEmployeeId", model.FixedApproverEmployeeId);
                parameters.Add("@EscalationDays", model.EscalationDays);
                parameters.Add("@IsActive", model.IsActive);
                parameters.Add("@CreatedBy", model.CreatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_OfferApprovalLevelConfig_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> UpdateLevelConfig(OfferApprovalLevelConfig model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "UPDATE");
                parameters.Add("@OfferApprovalLevelConfigId", model.OfferApprovalLevelConfigId);
                parameters.Add("@LevelNo", model.LevelNo);
                parameters.Add("@ApproverRole", model.ApproverRole);
                parameters.Add("@FixedApproverEmployeeId", model.FixedApproverEmployeeId);
                parameters.Add("@EscalationDays", model.EscalationDays);
                parameters.Add("@IsActive", model.IsActive);
                parameters.Add("@UpdatedBy", model.UpdatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_OfferApprovalLevelConfig_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> DeleteLevelConfig(int offerApprovalLevelConfigId, int? deletedBy)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "DELETE");
                parameters.Add("@OfferApprovalLevelConfigId", offerApprovalLevelConfigId);
                parameters.Add("@DeletedBy", deletedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_OfferApprovalLevelConfig_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> CreateApprovalRequest(int offerId, int? createdBy)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@OfferId", offerId);
                parameters.Add("@CreatedBy", createdBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_CreateOfferApprovalRequest", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> AddRequestLevel(int offerApprovalRequestId, int levelNo, int? approverEmployeeId, DateTime? escalationDueOn, int? createdBy)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "INSERT");
                parameters.Add("@OfferApprovalRequestId", offerApprovalRequestId);
                parameters.Add("@LevelNo", levelNo);
                parameters.Add("@ApproverEmployeeId", approverEmployeeId);
                parameters.Add("@EscalationDueOn", escalationDueOn);
                parameters.Add("@CreatedBy", createdBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_OfferApprovalRequestLevel_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> ActionOnRequestLevel(int offerApprovalRequestLevelId, string action, int actionBy, string? remarks)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@OfferApprovalRequestLevelId", offerApprovalRequestLevelId);
                parameters.Add("@Action", action);
                parameters.Add("@ActionBy", actionBy);
                parameters.Add("@Remarks", remarks);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_ActionOnOfferApprovalRequestLevel", parameters, commandType: CommandType.StoredProcedure);
                response.isSuccess = row != null && (int)row.Success > 0;
                response.ResponseMessage = row != null ? (string)row.ResponseMessage : "No response from database";
                response.Data = row != null ? new OfferApprovalActionResult { IsAllLevelsCompleted = (bool)row.IsAllLevelsCompleted, OfferApprovalRequestId = (int?)row.OfferApprovalRequestId } : null;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> GetRequestByOfferId(int offerId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryFirstOrDefaultAsync<OfferApprovalRequest>("GetOfferApprovalRequestByOfferId", new { OfferId = offerId }, commandType: CommandType.StoredProcedure);
                response.isSuccess = result != null;
                response.ResponseMessage = result != null ? "Success!" : "No approval request found.";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> GetRequestLevelsByRequestId(int offerApprovalRequestId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<OfferApprovalRequestLevel>("GetOfferApprovalRequestLevelsByRequestId", new { OfferApprovalRequestId = offerApprovalRequestId }, commandType: CommandType.StoredProcedure);
                response.isSuccess = true;
                response.ResponseMessage = "Success!";
                response.Data = result.AsList();
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> GetPendingApprovalsByApproverEmployeeId(int approverEmployeeId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<OfferApprovalRequestLevel>("GetPendingOfferApprovalsByApproverEmployeeId", new { ApproverEmployeeId = approverEmployeeId }, commandType: CommandType.StoredProcedure);
                response.isSuccess = true;
                response.ResponseMessage = "Success!";
                response.Data = result.AsList();
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> GetRequestHistoryByRequestId(int offerApprovalRequestId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<OfferApprovalRequestHistory>("GetOfferApprovalRequestHistoryByRequestId", new { OfferApprovalRequestId = offerApprovalRequestId }, commandType: CommandType.StoredProcedure);
                response.isSuccess = true;
                response.ResponseMessage = "Success!";
                response.Data = result.AsList();
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> GetAndMarkOverdueLevels()
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<OfferApprovalRequestLevel>("GetAndMarkOverdueOfferApprovalLevels", commandType: CommandType.StoredProcedure);
                response.isSuccess = true;
                response.ResponseMessage = "Success!";
                response.Data = result.AsList();
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
