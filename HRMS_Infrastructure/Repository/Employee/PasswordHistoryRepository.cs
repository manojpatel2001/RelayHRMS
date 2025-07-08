using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Infrastructure.Interface.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class PasswordHistoryRepository : Repository<PasswordHistory> , IPasswordHistory
    {
        private readonly HRMSDbContext _db;

        public PasswordHistoryRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
