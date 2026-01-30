using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Report;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class EmployeeTransferRepository : Repository<EmployeeTransfer>, IEmployeeTransferRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;

        public EmployeeTransferRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;

        }
        public async Task<SP_Response> CreateEmployeeTransfer(EmployeeTransfer model)
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
                    parameters.Add("@CurrentBranchId", model.CurrentBranchId);
                    parameters.Add("@TransferBranchId", model.TransferBranchId);
                    parameters.Add("@EffectiveDate", model.EffectiveDate);
                    parameters.Add("@Reason", model.Reason);
                    parameters.Add("@CreatedBy", model.CreatedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_EmployeeTransfer_CRUD",
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

        // Update Employee Transfer
        public async Task<SP_Response> UpdateEmployeeTransfer(EmployeeTransfer model)
        {
            var response = new SP_Response();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "UPDATE");
                    parameters.Add("@TransferId", model.TransferId);
                    parameters.Add("@EmployeeId", model.EmployeeId);
                    parameters.Add("@CurrentBranchId", model.CurrentBranchId);
                    parameters.Add("@TransferBranchId", model.TransferBranchId);
                    parameters.Add("@EffectiveDate", model.EffectiveDate);
                    parameters.Add("@Reason", model.Reason);
                    parameters.Add("@UpdatedBy", model.UpdatedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_EmployeeTransfer_CRUD",
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

        // Delete Employee Transfer
        public async Task<SP_Response> DeleteEmployeeTransfer(DeleteRecordVM deleteRecord)
        {
            var response = new SP_Response();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "DELETE");
                    parameters.Add("@TransferId", deleteRecord.Id);
                    parameters.Add("@DeletedBy", deleteRecord.DeletedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_EmployeeTransfer_CRUD",
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

        public async Task<List<GetEmployeeTransfervm>> GetEmployeeTransfers(int? CompanyId)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@CompanyId", CompanyId),

                };

                var result = await _db.Set<GetEmployeeTransfervm>()
                    .FromSqlRaw("EXEC sp_GetEmployeeTransfers @CompanyId ", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<GetEmployeeTransfervm>();
            }
        }
    }

}

