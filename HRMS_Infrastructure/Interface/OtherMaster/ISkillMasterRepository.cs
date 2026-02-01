using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.OtherMaster
{
    public interface ISkillMasterRepository
    {
        Task<SP_Response> CreateSkillMaster(SkillMaster model);
        Task<SP_Response> UpdateSkillMaster(SkillMaster model);
        Task<SP_Response> DeleteSkillMaster(DeleteRecordVM model);
        Task<List<SkillMaster>> GetAllSkillMasters();
    }
}
