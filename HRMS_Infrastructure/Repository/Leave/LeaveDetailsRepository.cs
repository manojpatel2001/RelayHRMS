using HRMS_Core.DbContext;
using HRMS_Core.Leave;
using HRMS_Infrastructure.Interface.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Leave
{
    public class LeaveDetailsRepository:Repository<LeaveDetails>,ILeaveDetailsRepository
    {
        private HRMSDbContext _db;

        public LeaveDetailsRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
