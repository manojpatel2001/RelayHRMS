using HRMS_Core.ManagePermission;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeProfileSkillRepository
    {
        Task<SP_Response> CreateEmployeeProfileSkill(EmployeeProfile_Skill model);
        Task<SP_Response> UpdateEmployeeProfileSkill(EmployeeProfile_Skill model);
        Task<SP_Response> DeleteEmployeeProfileSkill(DeleteRecordVM  model);

        Task<List<EmployeeProfile_Skill>> GetAllEmployeeProfile_Skills(int EmployeeId);
    }
}
