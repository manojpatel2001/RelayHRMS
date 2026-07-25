using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.SuperAdmin;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.SuperAdmin;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.SuperAdmin
{
    internal class SuperAdminDetailsRepository : Repository<SuperAdminDetails>, ISuperAdminDetailsRepository
    {

        private readonly HRMSDbContext _db;
        private readonly string _connectionString;
        public SuperAdminDetailsRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<SuperAdminDetails?> GetSuperAdminByCredentials(vmLogin vmLogin)
        {
            try
            {
                var result = await _db.Set<SuperAdminDetails>().FromSqlInterpolated($"EXEC GetSuperAdminByCredentials @Email={vmLogin.Email},@Password={vmLogin.Password}").ToListAsync();
                return result.FirstOrDefault() ?? null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> InsertLoginHistory(LoginHistory model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@EmpID", model.EmpID);
                    param.Add("@IPAddress", model.IPAddress);
                    param.Add("@BrowserInfo", model.BrowserInfo);
                    param.Add("@DeviceType", model.DeviceType);
                    param.Add("@LoginStatus", model.LoginStatus);
                    param.Add("@FailureReason", model.FailureReason);
                    param.Add("@SessionID", model.SessionID);

                    var result = await connection.ExecuteScalarAsync<int>(
                        "SP_InsertLoginHistory", param, commandType: CommandType.StoredProcedure
                    );
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"LoginHistory Insert Error: {ex.Message}");
                    return 0;
                }
            }
        }
        public async Task UpdateLogoutTime(int loginHistoryId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@LoginHistoryID", loginHistoryId);

                    await connection.ExecuteAsync(
                        "SP_UpdateLogoutTime", param, commandType: CommandType.StoredProcedure
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"UpdateLogoutTime Error: {ex.Message}");
                }
            }
        }
    }
}
