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
    public class NOCRepository: Repository<NOSForm>, INOCRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;

        public NOCRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;

        }
        public async Task<SP_Response> CreateNOC(NOSForm model)
        {
            var response = new SP_Response();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "INSERT");
                    parameters.Add("@ExitApplicationID", model.ExitApplicationID);
                    parameters.Add("@ItemName", model.ItemName);
                    parameters.Add("@IsHandedOver", model.IsHandedOver);
                    parameters.Add("@HandoverTo", model.HandoverTo);
                    parameters.Add("@Remarks", model.Remarks);

                    // ⭐ Fix for byte[] - pass null directly, not DBNull.Value
                    parameters.Add("@DocumentProof", model.DocumentProof, DbType.Binary);

                    parameters.Add("@CreatedBy", model.CreatedBy);

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_NOSForm_CRUD",
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

        public async Task<List<NOSForm>> GetNOCByExitApplicationId(int exitApplicationId)
        {

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "SELECT");
                    parameters.Add("@ExitApplicationID", exitApplicationId);

                    var result = await connection.QueryAsync<NOSForm>(
                        "sp_NOSForm_CRUD",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching NOC data: {ex.Message}");
            }
        }

        public async Task<SP_Response> UpdateNOC(NOSForm model)
        {
            var response = new SP_Response();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Operation", "UPDATE");
                    parameters.Add("@NOSID", model.NOSID);
                    parameters.Add("@ExitApplicationID", model.ExitApplicationID);
                    parameters.Add("@ItemName", model.ItemName);
                    parameters.Add("@IsHandedOver", model.IsHandedOver);
                    parameters.Add("@HandoverTo", model.HandoverTo);
                    parameters.Add("@Remarks", model.Remarks);

                    // ⭐ Fix for byte[]
                    parameters.Add("@DocumentProof", model.DocumentProof, DbType.Binary);

             

                    var result = await connection.QueryFirstOrDefaultAsync<SP_Response>(
                        "sp_NOSForm_CRUD",
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
