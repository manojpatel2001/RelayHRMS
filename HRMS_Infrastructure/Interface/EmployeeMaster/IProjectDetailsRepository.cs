using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.EmployeeMaster
{
    public interface IProjectDetailsRepository
    {
        Task<VMCommonResult> CreateProjectDetail(ProjectDetails model);
        Task<VMCommonResult> UpdateProjectDetail(ProjectDetails model);
        Task<VMCommonResult> DeleteProjectDetail(DeleteRecordVM deleteRecord);
        Task<ProjectDetails?> GetProjectDetailById(vmCommonGetById vmCommonGetById);
        Task<List<ProjectDetails>> GetAllProjectDetails(vmCommonGetById vmCommonGetById);
    }
}
