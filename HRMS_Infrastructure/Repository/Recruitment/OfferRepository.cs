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
    public class OfferRepository : IOfferRepository
    {
        private readonly string _connectionString;

        public OfferRepository(HRMSDbContext db)
        {
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> GetAllOffers(int? companyId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<Offer>("GetAllOffers", new { CompanyId = companyId }, commandType: CommandType.StoredProcedure);
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

        public async Task<APIResponse> GetOfferById(int offerId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryFirstOrDefaultAsync<Offer>("GetOfferById", new { OfferId = offerId }, commandType: CommandType.StoredProcedure);
                response.isSuccess = result != null;
                response.ResponseMessage = result != null ? "Success!" : "No record found.";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> GetOfferByCandidateApplicationId(int candidateApplicationId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryFirstOrDefaultAsync<Offer>("GetOfferByCandidateApplicationId", new { CandidateApplicationId = candidateApplicationId }, commandType: CommandType.StoredProcedure);
                response.isSuccess = result != null;
                response.ResponseMessage = result != null ? "Success!" : "No offer created yet.";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> CreateOffer(Offer model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "INSERT");
                parameters.Add("@CandidateApplicationId", model.CandidateApplicationId);
                parameters.Add("@JobPositionId", model.JobPositionId);
                parameters.Add("@OfferSalaryFitmentId", model.OfferSalaryFitmentId);
                parameters.Add("@OfferedCTC", model.OfferedCTC);
                parameters.Add("@DesignationId", model.DesignationId);
                parameters.Add("@DepartmentId", model.DepartmentId);
                parameters.Add("@BranchId", model.BranchId);
                parameters.Add("@GradeId", model.GradeId);
                parameters.Add("@JoiningDate", model.JoiningDate);
                parameters.Add("@OfferExpiryDate", model.OfferExpiryDate);
                parameters.Add("@OfferStatus", model.OfferStatus);
                parameters.Add("@IsEnabled", model.IsEnabled);
                parameters.Add("@CreatedBy", model.CreatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_Offer_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> UpdateOffer(Offer model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "UPDATE");
                parameters.Add("@OfferId", model.OfferId);
                parameters.Add("@OfferSalaryFitmentId", model.OfferSalaryFitmentId);
                parameters.Add("@OfferedCTC", model.OfferedCTC);
                parameters.Add("@DesignationId", model.DesignationId);
                parameters.Add("@DepartmentId", model.DepartmentId);
                parameters.Add("@BranchId", model.BranchId);
                parameters.Add("@GradeId", model.GradeId);
                parameters.Add("@JoiningDate", model.JoiningDate);
                parameters.Add("@OfferExpiryDate", model.OfferExpiryDate);
                parameters.Add("@OfferStatus", model.OfferStatus);
                parameters.Add("@IsEnabled", model.IsEnabled);
                parameters.Add("@UpdatedBy", model.UpdatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_Offer_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> DeleteOffer(DeleteRecordVM model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "DELETE");
                parameters.Add("@OfferId", model.Id);
                parameters.Add("@DeletedBy", string.IsNullOrEmpty(model.DeletedBy) ? (int?)null : int.Parse(model.DeletedBy));

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_Offer_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> SetOfferStatus(int offerId, string status, int? updatedBy)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "SETSTATUS");
                parameters.Add("@OfferId", offerId);
                parameters.Add("@OfferStatus", status);
                parameters.Add("@UpdatedBy", updatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_Offer_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> SetOfferLetterUrl(int offerId, string offerLetterUrl, int? updatedBy)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "SETLETTERURL");
                parameters.Add("@OfferId", offerId);
                parameters.Add("@OfferLetterUrl", offerLetterUrl);
                parameters.Add("@UpdatedBy", updatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_Offer_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> RecordCandidateResponse(int offerId, string status, string? acceptedByCandidateName, string? declineReason, int? updatedBy)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "RECORDRESPONSE");
                parameters.Add("@OfferId", offerId);
                parameters.Add("@OfferStatus", status);
                parameters.Add("@AcceptedByCandidateName", acceptedByCandidateName);
                parameters.Add("@DeclineReason", declineReason);
                parameters.Add("@UpdatedBy", updatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_Offer_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }
    }
}
