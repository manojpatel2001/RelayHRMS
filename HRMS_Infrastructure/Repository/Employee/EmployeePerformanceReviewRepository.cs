using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class EmployeePerformanceReviewRepository : Repository<EmployeePerformanceReview>, IEmployeePerformanceReviewRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;

        public EmployeePerformanceReviewRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<SP_Response> CreateEmployeePerformanceReview(EmployeePerformanceReview model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "INSERT");
                    parameters.Add("@EmployeeId", model.EmployeeId);
                    parameters.Add("@ReviewPeriodStart", model.ReviewPeriodStart);
                    parameters.Add("@ReviewPeriodEnd", model.ReviewPeriodEnd);
                    parameters.Add("@ReviewDate", model.ReviewDate);
                    parameters.Add("@ReviewerId", model.ReviewerId);
                    parameters.Add("@Rating", model.Rating);
                    parameters.Add("@Strengths", model.Strengths);
                    parameters.Add("@AreasOfImprovement", model.AreasOfImprovement);
                    parameters.Add("@Status", model.Status);
                    parameters.Add("@IsEnabled", model.IsEnabled);
                    parameters.Add("@CreatedBy", model.CreatedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_EmployeePerformanceReview_CRUD", parameters, commandType: CommandType.StoredProcedure);

                    return result ?? new SP_Response { Success = 0, ResponseMessage = "No response from database" };
                }
            }
            catch (Exception ex)
            {
                return new SP_Response { Success = -1, ResponseMessage = $"Error: {ex.Message}" };
            }
        }

        public async Task<SP_Response> UpdateEmployeePerformanceReview(EmployeePerformanceReview model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "UPDATE");
                    parameters.Add("@EmployeePerformanceReviewId", model.EmployeePerformanceReviewId);
                    parameters.Add("@ReviewPeriodStart", model.ReviewPeriodStart);
                    parameters.Add("@ReviewPeriodEnd", model.ReviewPeriodEnd);
                    parameters.Add("@ReviewDate", model.ReviewDate);
                    parameters.Add("@ReviewerId", model.ReviewerId);
                    parameters.Add("@Rating", model.Rating);
                    parameters.Add("@Strengths", model.Strengths);
                    parameters.Add("@AreasOfImprovement", model.AreasOfImprovement);
                    parameters.Add("@Status", model.Status);
                    parameters.Add("@IsEnabled", model.IsEnabled);
                    parameters.Add("@UpdatedBy", model.UpdatedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_EmployeePerformanceReview_CRUD", parameters, commandType: CommandType.StoredProcedure);

                    return result ?? new SP_Response { Success = 0, ResponseMessage = "No response from database" };
                }
            }
            catch (Exception ex)
            {
                return new SP_Response { Success = -1, ResponseMessage = $"Error: {ex.Message}" };
            }
        }

        public async Task<SP_Response> DeleteEmployeePerformanceReview(DeleteRecordVM model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "DELETE");
                    parameters.Add("@EmployeePerformanceReviewId", model.Id);
                    parameters.Add("@DeletedBy", string.IsNullOrEmpty(model.DeletedBy) ? (int?)null : int.Parse(model.DeletedBy));

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_EmployeePerformanceReview_CRUD", parameters, commandType: CommandType.StoredProcedure);

                    return result ?? new SP_Response { Success = 0, ResponseMessage = "No response from database" };
                }
            }
            catch (Exception ex)
            {
                return new SP_Response { Success = -1, ResponseMessage = $"Error: {ex.Message}" };
            }
        }

        public async Task<List<EmployeePerformanceReview>> GetEmployeePerformanceReviewByEmployeeId(int employeeId)
        {
            try
            {
                var parameters = new[] { new SqlParameter("@EmployeeId", employeeId) };
                var result = await _db.Set<EmployeePerformanceReview>()
                    .FromSqlRaw("EXEC GetEmployeePerformanceReviewByEmployeeId @EmployeeId", parameters)
                    .ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<EmployeePerformanceReview>();
            }
        }
    }
}
