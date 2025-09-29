using HRMS_Core.Master.OtherMaster;
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
    public interface ISchemeMasterRepository :IRepository<SchemeMasterViewModel>
    {
        Task<SP_Response> AddSchemeMaster(SchemeMasterViewModel model);
        Task<SP_Response> UpdateSchemeMaster(SchemeMasterViewModel model);
        Task<SP_Response> DeleteSchemeMaster(DeleteRecordVM deleteRecord);
        Task<SchemeMasterViewModel?> GetBySchemeId(int Id);
        Task<List<SchemeMasterViewModel>> GetAllSchemeMaster();
    }
}
