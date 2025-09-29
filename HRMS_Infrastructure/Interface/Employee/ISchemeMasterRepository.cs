using HRMS_Core.Migrations;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.ManagePermision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface ISchemeMasterRepository :IRepository<SchemeMaster>
    {
        Task<SP_Response> AddSchemeMaster(SchemeMaster model);
        Task<SP_Response> UpdateSchemeMaster(SchemeMaster model);
        Task<SP_Response> DeleteSchemeMaster(DeleteRecordVM deleteRecord);
        Task<SchemeMaster?> GetBySchemeId(vmCommonGetById filter);
    }
}
