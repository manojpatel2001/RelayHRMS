using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.Recruitment;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Recruitment
{
    public class OfferSalaryFitmentRepository : IOfferSalaryFitmentRepository
    {
        private readonly string _connectionString;

        public OfferSalaryFitmentRepository(HRMSDbContext db)
        {
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> CreateFitment(OfferSalaryFitment model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "INSERT");
                AddCommonParams(parameters, model);
                parameters.Add("@CreatedBy", model.CreatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_OfferSalaryFitment_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> UpdateFitment(OfferSalaryFitment model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "UPDATE");
                parameters.Add("@OfferSalaryFitmentId", model.OfferSalaryFitmentId);
                AddCommonParams(parameters, model);
                parameters.Add("@UpdatedBy", model.UpdatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_OfferSalaryFitment_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> DeleteFitment(DeleteRecordVM model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "DELETE");
                parameters.Add("@OfferSalaryFitmentId", model.Id);
                parameters.Add("@DeletedBy", string.IsNullOrEmpty(model.DeletedBy) ? (int?)null : int.Parse(model.DeletedBy));

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_OfferSalaryFitment_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> GetFitmentByCandidateApplicationId(int candidateApplicationId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryFirstOrDefaultAsync<OfferSalaryFitment>("GetOfferSalaryFitmentByCandidateApplicationId", new { CandidateApplicationId = candidateApplicationId }, commandType: CommandType.StoredProcedure);
                response.isSuccess = result != null;
                response.ResponseMessage = result != null ? "Success!" : "No fitment recorded yet.";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> GetFitmentById(int offerSalaryFitmentId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryFirstOrDefaultAsync<OfferSalaryFitment>("GetOfferSalaryFitmentById", new { OfferSalaryFitmentId = offerSalaryFitmentId }, commandType: CommandType.StoredProcedure);
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

        public async Task<APIResponse> ReplaceBreakupComponents(int offerSalaryFitmentId, List<OfferSalaryBreakupComponent> components, int? createdBy)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                await connection.ExecuteAsync("DeleteOfferSalaryBreakupComponents", new { OfferSalaryFitmentId = offerSalaryFitmentId, DeletedBy = createdBy }, commandType: CommandType.StoredProcedure);

                foreach (var component in components)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "INSERT");
                    parameters.Add("@OfferSalaryFitmentId", offerSalaryFitmentId);
                    parameters.Add("@ComponentName", component.ComponentName);
                    parameters.Add("@ComponentCategory", component.ComponentCategory);
                    parameters.Add("@Frequency", component.Frequency);
                    parameters.Add("@Amount", component.Amount);
                    parameters.Add("@CreatedBy", createdBy);

                    await connection.ExecuteAsync("sp_OfferSalaryBreakupComponent_CRUD", parameters, commandType: CommandType.StoredProcedure);
                }

                return new APIResponse { isSuccess = true, ResponseMessage = "Salary breakup saved successfully!" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> GetBreakupByFitmentId(int offerSalaryFitmentId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<OfferSalaryBreakupComponent>("GetOfferSalaryBreakupByFitmentId", new { OfferSalaryFitmentId = offerSalaryFitmentId }, commandType: CommandType.StoredProcedure);
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

        private static void AddCommonParams(DynamicParameters parameters, OfferSalaryFitment model)
        {
            parameters.Add("@CandidateApplicationId", model.CandidateApplicationId);
            parameters.Add("@SalaryStructureTemplateId", model.SalaryStructureTemplateId);
            parameters.Add("@CurrentCTC", model.CurrentCTC);
            parameters.Add("@ExpectedCTC", model.ExpectedCTC);
            parameters.Add("@RecommendedCTC", model.RecommendedCTC);
            parameters.Add("@FinalCTC", model.FinalCTC);
            parameters.Add("@BudgetValidationStatus", model.BudgetValidationStatus);
            parameters.Add("@Remarks", model.Remarks);
            parameters.Add("@IsEnabled", model.IsEnabled);
        }
    }
}
