using HRMS_Core.DbContext;
using HRMS_Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HRMSDbContext _dbContext;

        public UnitOfWork(HRMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
