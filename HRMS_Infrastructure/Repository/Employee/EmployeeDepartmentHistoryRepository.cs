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
    public class EmployeeDepartmentHistoryRepository : Repository<EmployeeDepartmentHistory>, IEmployeeDepartmentHistoryRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;

        public EmployeeDepartmentHistoryRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<SP_Response> CreateEmployeeDepartmentHistory(EmployeeDepartmentHistory model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "INSERT");
                    parameters.Add("@EmployeeId", model.EmployeeId);
                    parameters.Add("@OldDepartmentId", model.OldDepartmentId);
                    parameters.Add("@NewDepartmentId", model.NewDepartmentId);
                    parameters.Add("@EffectiveDate", model.EffectiveDate);
                    parameters.Add("@Reason", model.Reason);
                    parameters.Add("@IsEnabled", model.IsEnabled);
                    parameters.Add("@CreatedBy", model.CreatedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_EmployeeDepartmentHistory_CRUD", parameters, commandType: CommandType.StoredProcedure);

                    return result ?? new SP_Response { Success = 0, ResponseMessage = "No response from database" };
                }
            }
            catch (Exception ex)
            {
                return new SP_Response { Success = -1, ResponseMessage = $"Error: {ex.Message}" };
            }
        }

        public async Task<SP_Response> UpdateEmployeeDepartmentHistory(EmployeeDepartmentHistory model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "UPDATE");
                    parameters.Add("@EmployeeDepartmentHistoryId", model.EmployeeDepartmentHistoryId);
                    parameters.Add("@OldDepartmentId", model.OldDepartmentId);
                    parameters.Add("@NewDepartmentId", model.NewDepartmentId);
                    parameters.Add("@EffectiveDate", model.EffectiveDate);
                    parameters.Add("@Reason", model.Reason);
                    parameters.Add("@IsEnabled", model.IsEnabled);
                    parameters.Add("@UpdatedBy", model.UpdatedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_EmployeeDepartmentHistory_CRUD", parameters, commandType: CommandType.StoredProcedure);

                    return result ?? new SP_Response { Success = 0, ResponseMessage = "No response from database" };
                }
            }
            catch (Exception ex)
            {
                return new SP_Response { Success = -1, ResponseMessage = $"Error: {ex.Message}" };
            }
        }

        public async Task<SP_Response> DeleteEmployeeDepartmentHistory(DeleteRecordVM model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "DELETE");
                    parameters.Add("@EmployeeDepartmentHistoryId", model.Id);
                    parameters.Add("@DeletedBy", string.IsNullOrEmpty(model.DeletedBy) ? (int?)null : int.Parse(model.DeletedBy));

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_EmployeeDepartmentHistory_CRUD", parameters, commandType: CommandType.StoredProcedure);

                    return result ?? new SP_Response { Success = 0, ResponseMessage = "No response from database" };
                }
            }
            catch (Exception ex)
            {
                return new SP_Response { Success = -1, ResponseMessage = $"Error: {ex.Message}" };
            }
        }

        public async Task<List<EmployeeDepartmentHistory>> GetEmployeeDepartmentHistoryByEmployeeId(int employeeId)
        {
            try
            {
                var parameters = new[] { new SqlParameter("@EmployeeId", employeeId) };
                var result = await _db.Set<EmployeeDepartmentHistory>()
                    .FromSqlRaw("EXEC GetEmployeeDepartmentHistoryByEmployeeId @EmployeeId", parameters)
                    .ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<EmployeeDepartmentHistory>();
            }
        }
    }
}
