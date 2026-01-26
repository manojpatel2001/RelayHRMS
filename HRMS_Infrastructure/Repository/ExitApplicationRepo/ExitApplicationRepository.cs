using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.ExitApplication;
using HRMS_Core.VM.Leave;
using HRMS_Core.VM.ManagePermision;
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


        public async Task<SP_Response> DeleteExitApplication(DeleteRecordVModel deleteRecord)
        {
            var response = new SP_Response();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    foreach (int id in deleteRecord.Id)
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@Operation", "DELETE");
                        parameters.Add("@ExitApplicationID", id);
                        parameters.Add("@DeletedBy", deleteRecord.DeletedBy);

                        // Query the stored procedure and get the first row of the result
                        var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                            "sp_ExitApplication_CRUD",
                            parameters,
                            commandType: CommandType.StoredProcedure
                        );

                        // If the stored procedure returns null or indicates failure, stop processing
                        if (result == null || result.Success != 1)
                        {
                            response.Success = result?.Success ?? -1;
                            response.ResponseMessage = result?.ResponseMessage ?? "Deletion failed for ID: " + id;
                            break;
                        }

                        // If successful, continue to the next ID
                        response.Success = result.Success;
                        response.ResponseMessage = result.ResponseMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = -1;
                response.ResponseMessage = $"Error: {ex.Message}";
            }
            return response;
        }

        public async Task<ExitApplicationReportVm?> GetExitApplicationById(int Employeeid)
        {
            try
            {
                var result = await _db.Set<ExitApplicationReportVm?>().FromSqlInterpolated($@"
                    EXEC sp_GetExitApplicationByEmployeeId
                        @Employeeid = {Employeeid}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<GetExitApproval?>> GetExitApproval(ExitApprovalParam model)
        {
            try
            {
                var parameters = new[]
                {
               
                          new SqlParameter("@EmployeeId", (object?)model.EmployeeID ?? DBNull.Value),
                          new SqlParameter("@SearchBy", (object?)model.SearchBy ?? DBNull.Value),
                          new SqlParameter("@SearchFor", (object?)model.SearchFor ?? DBNull.Value),
                          new SqlParameter("@Status", (object?)model.Status ?? DBNull.Value),

                };

                var result = await _db.Set<GetExitApproval>()
                    .FromSqlRaw("EXEC SP_GetExitApproval @EmployeeId, @SearchBy,@SearchFor ,@Status", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SP_GetExitApproval Error: " + ex.Message);
                return new List<GetExitApproval>();
            }
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

        public async Task<SP_Response> UpdateExitApproval(ExitApplicationUpdateparam model)
        {
            try
            {
                string idsString = string.Join(",", model.ExitApplicationID);

                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                       EXEC UpdateExitApplicationStatus
                        @ExitApplicationID = {idsString},
                        @Status = {model.Status},
                        @UpdatedBy = {model.UpdatedBy}
                       ")
                    .ToListAsync();


                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Some thing went wrong" };
            }
            catch (Exception ex)
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }
    }
}
