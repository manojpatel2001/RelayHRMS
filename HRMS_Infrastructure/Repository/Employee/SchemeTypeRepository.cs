using HRMS_Core.DbContext;
using HRMS_Core.Master.OtherMaster;
using HRMS_Infrastructure.Interface.JobMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class SchemeTypeRepository : Repository<SchemeTypeRepository>, ISchemeTypeRepository
    {
        private readonly HRMSDbContext _db;

        public SchemeTypeRepository(HRMSDbContext hRMSDbContext) : base(hRMSDbContext)
        {
            _db = hRMSDbContext;
        }

        public Task AddAsync(SchemeTypeModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SchemeTypeModel>> GetAllAsync(Expression<Func<SchemeTypeModel, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SchemeTypeModel>> GetAllSchemeType()
        {
            try
            {
                var result = await _db.Database
                    .SqlQueryRaw<SchemeTypeModel>($@"
                EXEC GetAllSchemeTypes 
            ")
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                return new List<SchemeTypeModel>();
            }
        }

        public Task<SchemeTypeModel> GetAsync(Expression<Func<SchemeTypeModel, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(SchemeTypeModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
