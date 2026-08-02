using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Utility;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface ICandidateRepository
    {
        Task<APIResponse> GetAllCandidates();
        Task<APIResponse> GetCandidateById(int candidateId);
        Task<APIResponse> GetCandidateFullProfile(int candidateId);
        Task<APIResponse> CreateCandidate(Candidate model);
        Task<APIResponse> UpdateCandidate(Candidate model);
        Task<APIResponse> DeleteCandidate(DeleteRecordVM model);
        Task<APIResponse> CheckDuplicateCandidate(string? email, string? phone, string? resumeFileHash);
    }
}
