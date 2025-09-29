using HRMS_Core.DbContext;
using HRMS_Core.Migrations;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface.Employee;
using HRMS_Infrastructure.Interface.JobMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class SchemeMasterRepository: Repository<SchemeMaster> ,ISchemeMasterRepository
    {
        private readonly HRMSDbContext _db;
        public SchemeMasterRepository(HRMSDbContext hRMSDbContext) : base(hRMSDbContext)
        {
            _db = hRMSDbContext;
        }

        public Task<SP_Response> AddSchemeMaster(SchemeMaster model)
        {
            throw new NotImplementedException();
        }

        public Task<SP_Response> DeleteSchemeMaster(DeleteRecordVM deleteRecord)
        {
            throw new NotImplementedException();
        }

        public Task<SchemeMaster?> GetBySchemeId(vmCommonGetById filter)
        {
            throw new NotImplementedException();
        }

        public Task<SP_Response> UpdateSchemeMaster(SchemeMaster model)
        {
            throw new NotImplementedException();
        }
    }
}
