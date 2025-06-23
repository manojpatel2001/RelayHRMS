using HRMS_Core.PrivilegeSetting;
using HRMS_Core.VM;
using HRMS_Core.VM.PrivilegeSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.PrivilegeSetting
{
    public interface IPrivilegeMasterRepository:IRepository<PrivilegeMaster>
    {
        Task<VMCommonResult> CreatePrivilegeMaster(PrivilegeMaster privilegeMaster);
        Task<VMCommonResult> UpdatePrivilegeMaster(PrivilegeMaster privilegeMaster);
        Task<VMCommonResult> DeletePrivilegeMaster(DeleteRecordVM deleteRecord);
        Task<PrivilegeMaster?> GetPrivilegeMasterById(int privilegeMasterId);
        Task<List<vmGetAllPrivilegeMasterByCompanyId>> GetAllPrivilegeMasterByCompanyId(int companyId);
        Task<List<PanelHierarchyVM>> GetAllPageHierarchyByPrivilegeMasterId(PageVM pageVM);
    }
}
