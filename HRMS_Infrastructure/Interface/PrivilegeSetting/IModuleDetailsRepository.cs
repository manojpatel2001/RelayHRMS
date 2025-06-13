using HRMS_Core.PrivilegeSetting;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.PrivilegeSetting
{
    public interface IModuleDetailsRepository:IRepository<ModuleDetails>
    {
        Task<VMCommonResult> CreateModuleDetails(ModuleDetails moduleDetails);
        Task<VMCommonResult> UpdateModuleDetails(ModuleDetails moduleDetails);
        Task<VMCommonResult> DeleteModuleDetails(DeleteRecordVM moduleDetails);
        Task<ModuleDetails?> GetModuleDetailsById(int moduleDetailsId);
        Task<List<ModuleDetails>> GetAllModuleDetailsByCompanyId(int companyId);
    }
}
