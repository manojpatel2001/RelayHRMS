using HRMS_Core.DbContext;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class LeftEmployeeRepository : Repository<LeftEmployee>, ILeftEmployeeRepository
    {

        private readonly HRMSDbContext _db;
        public LeftEmployeeRepository(HRMSDbContext hRMSDbContext) : base(hRMSDbContext)
        {
            _db = hRMSDbContext;
        }
    }
}
