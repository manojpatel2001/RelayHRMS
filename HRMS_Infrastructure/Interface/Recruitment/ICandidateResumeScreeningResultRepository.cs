using HRMS_Core.Recruitment;
using HRMS_Utility;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface ICandidateResumeScreeningResultRepository
    {
        Task<APIResponse> CreateOrUpdateScreeningResult(CandidateResumeScreeningResult model);
        Task<APIResponse> GetScreeningResultByApplicationId(int candidateApplicationId);
        Task<APIResponse> GetScreeningResultsByJobPositionId(int jobPositionId);
    }
}
