using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.ExitApplication;
using HRMS_Infrastructure.Interface.ExitApplication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.ExitApplicationRepo
{
    public class ExitApplicationRepository:Repository<ExitApplicationVm>, IExitApplicationRepository

    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;

        public ExitApplicationRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;

        }

        public async Task<SP_Response> CreateExitApplication(ExitApplicationVm model)
        {
            var response = new SP_Response();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "INSERT");
                    parameters.Add("@EmployeeId", model.EmployeeId);
                    parameters.Add("@ResignationDate", model.ResignationDate);
                    parameters.Add("@NoticePeriodDays", model.NoticePeriodDays);
                    parameters.Add("@LastWorkingDate", model.LastWorkingDate);
                    parameters.Add("@ShortFallDays", model.ShortFallDays);
                    parameters.Add("@ReasonForResignation", model.ReasonForResignation);
                    parameters.Add("@Comments", model.Comments);
                    parameters.Add("@DocumentName", model.DocumentName);
                    parameters.Add("@DocumentData", model.DocumentData);
                    parameters.Add("@IsAgreementAccepted", model.IsAgreementAccepted);
                    parameters.Add("@IsPending", model.IsPending);
                    parameters.Add("@IsApproved", model.IsApproved);
                    parameters.Add("@IsRejected", model.IsRejected);
                    parameters.Add("@CreatedBy", model.CreatedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_ExitApplication_CRUD",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    response.Success = result.Success;
                    response.ResponseMessage = result.ResponseMessage;
                }
            }
            catch (Exception ex)
            {
                response.Success = -1;
                response.ResponseMessage = $"Error: {ex.Message}";
            }
            return response;
        }


        public async Task<SP_Response> DeleteExitApplication(DeleteRecordVM deleteRecord)
        {
            var response = new SP_Response();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "DELETE");
                    parameters.Add("@ExitApplicationID", deleteRecord.Id);
                    parameters.Add("@DeletedBy", deleteRecord.DeletedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_ExitApplication_CRUD",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    response.Success = result.Success;
                    response.ResponseMessage = result.ResponseMessage;
                }
            }
            catch (Exception ex)
            {
                response.Success = -1;
                response.ResponseMessage = $"Error: {ex.Message}";
            }
            return response;
        }


        public async Task<SP_Response> UpdateExitApplication(ExitApplicationVm model)
        {
            var response = new SP_Response();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "UPDATE");
                    parameters.Add("@ExitApplicationID", model.ExitApplicationID);
                    parameters.Add("@EmployeeId", model.EmployeeId);
                    parameters.Add("@ResignationDate", model.ResignationDate);
                    parameters.Add("@NoticePeriodDays", model.NoticePeriodDays);
                    parameters.Add("@LastWorkingDate", model.LastWorkingDate);
                    parameters.Add("@ShortFallDays", model.ShortFallDays);
                    parameters.Add("@ReasonForResignation", model.ReasonForResignation);
                    parameters.Add("@Comments", model.Comments);
                    parameters.Add("@DocumentName", model.DocumentName);
                    parameters.Add("@DocumentData", model.DocumentData);
                    parameters.Add("@IsAgreementAccepted", model.IsAgreementAccepted);
                    parameters.Add("@IsPending", model.IsPending);
                    parameters.Add("@IsApproved", model.IsApproved);
                    parameters.Add("@IsRejected", model.IsRejected);
                    parameters.Add("@UpdatedBy", model.UpdatedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_ExitApplication_CRUD",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    response.Success = result.Success;
                    response.ResponseMessage = result.ResponseMessage;
                }
            }
            catch (Exception ex)
            {
                response.Success = -1;
                response.ResponseMessage = $"Error: {ex.Message}";
            }
            return response;
        }

    }
}
