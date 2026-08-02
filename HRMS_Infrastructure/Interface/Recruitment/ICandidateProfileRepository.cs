using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Utility;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    // Combined repository for the 5 identical-shape Candidate profile child tables
    // (Education/Experience/Skill/Certification/Project) — grouped to avoid 10 near-duplicate files.
    public interface ICandidateProfileRepository
    {
        Task<APIResponse> CreateEducation(CandidateEducation model);
        Task<APIResponse> UpdateEducation(CandidateEducation model);
        Task<APIResponse> DeleteEducation(DeleteRecordVM model);
        Task<APIResponse> GetEducationByCandidateId(int candidateId);

        Task<APIResponse> CreateExperience(CandidateExperience model);
        Task<APIResponse> UpdateExperience(CandidateExperience model);
        Task<APIResponse> DeleteExperience(DeleteRecordVM model);
        Task<APIResponse> GetExperienceByCandidateId(int candidateId);

        Task<APIResponse> CreateSkill(CandidateSkill model);
        Task<APIResponse> UpdateSkill(CandidateSkill model);
        Task<APIResponse> DeleteSkill(DeleteRecordVM model);
        Task<APIResponse> GetSkillByCandidateId(int candidateId);

        Task<APIResponse> CreateCertification(CandidateCertification model);
        Task<APIResponse> UpdateCertification(CandidateCertification model);
        Task<APIResponse> DeleteCertification(DeleteRecordVM model);
        Task<APIResponse> GetCertificationByCandidateId(int candidateId);

        Task<APIResponse> CreateProject(CandidateProject model);
        Task<APIResponse> UpdateProject(CandidateProject model);
        Task<APIResponse> DeleteProject(DeleteRecordVM model);
        Task<APIResponse> GetProjectByCandidateId(int candidateId);
    }
}
