using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.Recruitment;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Recruitment
{
    public class CandidateBGVRepository : ICandidateBGVRepository
    {
        private readonly string _connectionString;

        public CandidateBGVRepository(HRMSDbContext db)
        {
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> GetChecksByApplicationId(int candidateApplicationId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<CandidateBGVCheck>("GetCandidateBGVChecksByApplicationId", new { CandidateApplicationId = candidateApplicationId }, commandType: CommandType.StoredProcedure);
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

        public async Task<APIResponse> CreateCheck(CandidateBGVCheck model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "INSERT");
                AddCommonParams(parameters, model);
                parameters.Add("@CreatedBy", model.CreatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_CandidateBGVCheck_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> UpdateCheck(CandidateBGVCheck model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "UPDATE");
                parameters.Add("@CandidateBGVCheckId", model.CandidateBGVCheckId);
                AddCommonParams(parameters, model);
                parameters.Add("@UpdatedBy", model.UpdatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_CandidateBGVCheck_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> DeleteCheck(DeleteRecordVM model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "DELETE");
                parameters.Add("@CandidateBGVCheckId", model.Id);
                parameters.Add("@DeletedBy", string.IsNullOrEmpty(model.DeletedBy) ? (int?)null : int.Parse(model.DeletedBy));

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_CandidateBGVCheck_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        private static void AddCommonParams(DynamicParameters parameters, CandidateBGVCheck model)
        {
            parameters.Add("@CandidateApplicationId", model.CandidateApplicationId);
            parameters.Add("@CheckType", model.CheckType);
            parameters.Add("@VendorName", model.VendorName);
            parameters.Add("@VendorContact", model.VendorContact);
            parameters.Add("@Status", model.Status);
            parameters.Add("@InitiatedDate", model.InitiatedDate);
            parameters.Add("@CompletedDate", model.CompletedDate);
            parameters.Add("@ProofDocumentUrl", model.ProofDocumentUrl);
            parameters.Add("@Remarks", model.Remarks);
            parameters.Add("@VerifiedBy", model.VerifiedBy);
            parameters.Add("@IsEnabled", model.IsEnabled);
        }
    }
}
