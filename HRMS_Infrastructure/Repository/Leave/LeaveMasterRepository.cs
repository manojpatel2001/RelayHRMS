using HRMS_Core.DbContext;
using HRMS_Core.Leave;
using HRMS_Infrastructure.Interface.Leave;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Leave
{
    public class LeaveMasterRepository:Repository<LeaveMaster>,ILeaveMasterRepository
    {

        private HRMSDbContext _db;

        public LeaveMasterRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }




    }
}
