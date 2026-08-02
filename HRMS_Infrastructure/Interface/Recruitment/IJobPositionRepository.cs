using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Utility;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface IJobPositionRepository
    {
        Task<APIResponse> GetAllJobPositions(CommonParameter commonParameter);
        Task<APIResponse> GetJobPositionById(int jobPositionId);
        Task<APIResponse> CreateJobPosition(JobPositionMaster model);
        Task<APIResponse> UpdateJobPosition(JobPositionMaster model);
        Task<APIResponse> DeleteJobPosition(DeleteRecordVM model);
        Task<APIResponse> GetJobPositionDropdown(int companyId);
        Task<APIResponse> GetJobPositionsByManpowerRequisitionId(int manpowerRequisitionId);
    }
}
