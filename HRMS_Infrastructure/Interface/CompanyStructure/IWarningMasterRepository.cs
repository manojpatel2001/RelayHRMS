using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.CompanyStructure
{
    public interface IWarningMasterRepository : IRepository<WarningMaster>
    {
        Task<List<WarningMaster>> GetAllWarningMaster();
        Task<WarningMaster?> GetByWarningMasterId(int warningMasterId);
        Task<VMCommonResult> CreateWarningMaster(WarningMaster warningMaster);
        Task<VMCommonResult> UpdateWarningMaster(WarningMaster warningMaster);
        Task<VMCommonResult> DeleteWarningMaster(DeleteRecordVM deleteRecordVM);
    }
}
