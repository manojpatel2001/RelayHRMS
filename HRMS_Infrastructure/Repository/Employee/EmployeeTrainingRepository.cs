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
    public class EmployeeTrainingRepository : Repository<EmployeeTraining>, IEmployeeTrainingRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;

        public EmployeeTrainingRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<SP_Response> CreateEmployeeTraining(EmployeeTraining model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "INSERT");
                    parameters.Add("@EmployeeId", model.EmployeeId);
                    parameters.Add("@TrainingName", model.TrainingName);
                    parameters.Add("@TrainingType", model.TrainingType);
                    parameters.Add("@StartDate", model.StartDate);
                    parameters.Add("@EndDate", model.EndDate);
                    parameters.Add("@Provider", model.Provider);
                    parameters.Add("@Status", model.Status);
                    parameters.Add("@Score", model.Score);
                    parameters.Add("@IsEnabled", model.IsEnabled);
                    parameters.Add("@CreatedBy", model.CreatedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_EmployeeTraining_CRUD", parameters, commandType: CommandType.StoredProcedure);

                    return result ?? new SP_Response { Success = 0, ResponseMessage = "No response from database" };
                }
            }
            catch (Exception ex)
            {
                return new SP_Response { Success = -1, ResponseMessage = $"Error: {ex.Message}" };
            }
        }

        public async Task<SP_Response> UpdateEmployeeTraining(EmployeeTraining model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "UPDATE");
                    parameters.Add("@EmployeeTrainingId", model.EmployeeTrainingId);
                    parameters.Add("@TrainingName", model.TrainingName);
                    parameters.Add("@TrainingType", model.TrainingType);
                    parameters.Add("@StartDate", model.StartDate);
                    parameters.Add("@EndDate", model.EndDate);
                    parameters.Add("@Provider", model.Provider);
                    parameters.Add("@Status", model.Status);
                    parameters.Add("@Score", model.Score);
                    parameters.Add("@IsEnabled", model.IsEnabled);
                    parameters.Add("@UpdatedBy", model.UpdatedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_EmployeeTraining_CRUD", parameters, commandType: CommandType.StoredProcedure);

                    return result ?? new SP_Response { Success = 0, ResponseMessage = "No response from database" };
                }
            }
            catch (Exception ex)
            {
                return new SP_Response { Success = -1, ResponseMessage = $"Error: {ex.Message}" };
            }
        }

        public async Task<SP_Response> DeleteEmployeeTraining(DeleteRecordVM model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "DELETE");
                    parameters.Add("@EmployeeTrainingId", model.Id);
                    parameters.Add("@DeletedBy", string.IsNullOrEmpty(model.DeletedBy) ? (int?)null : int.Parse(model.DeletedBy));

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_EmployeeTraining_CRUD", parameters, commandType: CommandType.StoredProcedure);

                    return result ?? new SP_Response { Success = 0, ResponseMessage = "No response from database" };
                }
            }
            catch (Exception ex)
            {
                return new SP_Response { Success = -1, ResponseMessage = $"Error: {ex.Message}" };
            }
        }

        public async Task<List<EmployeeTraining>> GetEmployeeTrainingByEmployeeId(int employeeId)
        {
            try
            {
                var parameters = new[] { new SqlParameter("@EmployeeId", employeeId) };
                var result = await _db.Set<EmployeeTraining>()
                    .FromSqlRaw("EXEC GetEmployeeTrainingByEmployeeId @EmployeeId", parameters)
                    .ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<EmployeeTraining>();
            }
        }
    }
}
