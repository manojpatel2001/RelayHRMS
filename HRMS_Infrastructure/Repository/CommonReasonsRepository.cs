using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.VM.Leave;
using HRMS_Infrastructure.Interface.CommanReason;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository
{
    public class CommonReasonsRepository : Repository<CancellationReasonvm>, ICommonReasonsRepository
    {
        private HRMSDbContext _db;
        private readonly string _connectionString;

        public CommonReasonsRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;

        }

        public Task AddAsync(CancellationReasonvm entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CancellationReasonvm>> GetAllAsync(Expression<Func<CancellationReasonvm, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public Task<CancellationReasonvm> GetAsync(Expression<Func<CancellationReasonvm, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(CancellationReasonvm entity)
        {
            throw new NotImplementedException();
        }
        public async Task<List<CancellationReasonvm>> GetLeavecancellationReasons()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                  
                    var result = await connection.QueryAsync<CancellationReasonvm>(
                        "GetLeaveApplicationReasons",
                        commandType: CommandType.StoredProcedure
                    );
                    return result.AsList();
                }
            }
            catch
            {
                // Return an empty list or handle the exception as needed
                return new List<CancellationReasonvm>();
            }
        }

        public async Task<List<CancellationReasonvm>> GetcancellationReasons()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {

                    var result = await connection.QueryAsync<CancellationReasonvm>(
                        "GetAttendanceReasons",
                        commandType: CommandType.StoredProcedure
                    );
                    return result.AsList();
                }
            }
            catch
            {
                return new List<CancellationReasonvm>();
            }
        }
    }
}
