using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Utility;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface ICandidateApplicationRepository
    {
        Task<APIResponse> GetAllCandidateApplications(int? jobPositionId);
        Task<APIResponse> GetCandidateApplicationById(int candidateApplicationId);
        Task<APIResponse> GetCandidateApplicationsByCandidateId(int candidateId);
        Task<APIResponse> CreateCandidateApplication(CandidateApplication model);
        Task<APIResponse> UpdateCandidateApplication(CandidateApplication model);
        Task<APIResponse> DeleteCandidateApplication(DeleteRecordVM model);
        Task<APIResponse> GetKanbanBoardData(int jobPositionId);
    }
}
