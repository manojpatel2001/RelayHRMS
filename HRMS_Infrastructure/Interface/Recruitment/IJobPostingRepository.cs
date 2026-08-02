using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Utility;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface IJobPostingRepository
    {
        Task<APIResponse> GetAllJobPostings(CommonParameter commonParameter);
        Task<APIResponse> GetJobPostingById(int jobPostingId);
        Task<APIResponse> GetJobPostingBySlug(string slug);
        Task<APIResponse> CreateJobPosting(JobPosting model);
        Task<APIResponse> UpdateJobPosting(JobPosting model);
        Task<APIResponse> DeleteJobPosting(DeleteRecordVM model);
        Task<APIResponse> PublishJobPosting(int jobPostingId, int? updatedBy);
        Task<APIResponse> PostExternally(int jobPostingId, string channel, string? postingUrl, int? updatedBy);
    }
}
