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
    public class CommonReasonsRepository : Repository<LeaveCancellationReasonvm>, ICommonReasonsRepository
    {
        private HRMSDbContext _db;
        private readonly string _connectionString;

        public CommonReasonsRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;

        }

        public Task AddAsync(LeaveCancellationReasonvm entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LeaveCancellationReasonvm>> GetAllAsync(Expression<Func<LeaveCancellationReasonvm, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public Task<LeaveCancellationReasonvm> GetAsync(Expression<Func<LeaveCancellationReasonvm, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(LeaveCancellationReasonvm entity)
        {
            throw new NotImplementedException();
        }
        public async Task<List<LeaveCancellationReasonvm>> GetLeavecancellationReasons()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                  
                    var result = await connection.QueryAsync<LeaveCancellationReasonvm>(
                        "GetLeaveApplicationReasons",
                        commandType: CommandType.StoredProcedure
                    );
                    return result.AsList();
                }
            }
            catch
            {
                // Return an empty list or handle the exception as needed
                return new List<LeaveCancellationReasonvm>();
            }
        }

    }
}
