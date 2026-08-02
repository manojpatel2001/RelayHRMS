using HRMS_Core.Recruitment;
using HRMS_Utility;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface ICandidateEmployeeConversionRepository
    {
        Task<APIResponse> GetByCandidateApplicationId(int candidateApplicationId);
        Task<APIResponse> CreateConversion(CandidateEmployeeConversion model);
        Task<APIResponse> UpdateConversion(CandidateEmployeeConversion model);
    }
}
