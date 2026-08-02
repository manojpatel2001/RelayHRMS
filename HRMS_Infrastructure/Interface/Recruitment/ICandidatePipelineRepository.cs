using HRMS_Core.Recruitment;
using HRMS_Utility;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface ICandidatePipelineRepository
    {
        Task<APIResponse> GetAllPipelineStages(int? companyId);
        Task<APIResponse> CreatePipelineStage(CandidatePipelineStage model);
        Task<APIResponse> UpdatePipelineStage(CandidatePipelineStage model);
        Task<APIResponse> MoveCandidateToStage(int candidateApplicationId, int toStageId, string? remarks, int? actionBy);
        Task<APIResponse> GetPipelineHistoryByApplicationId(int candidateApplicationId);
    }
}
