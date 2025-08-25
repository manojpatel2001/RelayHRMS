using HRMS_Core.ManagePermission;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeProfileSkillRepository
    {
        Task<SP_Response> CreateEmpskill(VmSkill model);
        Task<SP_Response> UpdateEmpProfileSkill(VmSkill model);
        Task<SP_Response> DeleteEmpProfileSkill(VmSkill model);

        Task<List<VmSkillMaster>> GetAllSkill();
    }
}
