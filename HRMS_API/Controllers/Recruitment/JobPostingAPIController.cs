using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPostingAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobPostingAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("GetAllJobPostings")]
        public async Task<APIResponse> GetAllJobPostings(CommonParameter commonParameter)
        {
            try
            {
                return await _unitOfWork.JobPostingRepository.GetAllJobPostings(commonParameter);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve job postings. Please try again later." };
            }
        }

        [HttpGet("GetJobPostingById/{jobPostingId}")]
        public async Task<APIResponse> GetJobPostingById(int jobPostingId)
        {
            try
            {
                return await _unitOfWork.JobPostingRepository.GetJobPostingById(jobPostingId);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve job posting. Please try again later." };
            }
        }

        [HttpGet("GetJobPostingBySlug/{slug}")]
        public async Task<APIResponse> GetJobPostingBySlug(string slug)
        {
            try
            {
                return await _unitOfWork.JobPostingRepository.GetJobPostingBySlug(slug);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve job posting. Please try again later." };
            }
        }

        [HttpPost("CreateJobPosting")]
        public async Task<APIResponse> CreateJobPosting([FromBody] JobPosting model)
        {
            try
            {
                if (model == null || model.JobPositionId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Job posting must reference a job position." };

                if (string.IsNullOrWhiteSpace(model.Slug) && !string.IsNullOrWhiteSpace(model.PostingTitle))
                    model.Slug = Slugify(model.PostingTitle) + "-" + DateTime.UtcNow.Ticks.ToString().Substring(10);

                model.CreatedDate = DateTime.UtcNow;
                return await _unitOfWork.JobPostingRepository.CreateJobPosting(model);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create job posting. Please try again later." };
            }
        }

        [HttpPost("UpdateJobPosting")]
        public async Task<APIResponse> UpdateJobPosting([FromBody] JobPosting model)
        {
            try
            {
                if (model == null || model.JobPostingId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid job posting." };

                return await _unitOfWork.JobPostingRepository.UpdateJobPosting(model);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update job posting. Please try again later." };
            }
        }

        [HttpDelete("DeleteJobPosting")]
        public async Task<APIResponse> DeleteJobPosting(DeleteRecordVM model)
        {
            try
            {
                return await _unitOfWork.JobPostingRepository.DeleteJobPosting(model);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete job posting. Please try again later." };
            }
        }

        [HttpPut("PublishJobPosting")]
        public async Task<APIResponse> PublishJobPosting([FromBody] PublishJobPostingRequest model)
        {
            try
            {
                return await _unitOfWork.JobPostingRepository.PublishJobPosting(model.JobPostingId, model.UpdatedBy);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to publish job posting. Please try again later." };
            }
        }

        // Placeholder — records intent to post to an external channel. No live LinkedIn/
        // Naukri/Indeed/Referral API call is made; wiring a real integration only requires
        // replacing JobPostingRepository.PostExternally's body.
        [HttpPut("PostExternally")]
        public async Task<APIResponse> PostExternally([FromBody] PostExternallyRequest model)
        {
            try
            {
                return await _unitOfWork.JobPostingRepository.PostExternally(model.JobPostingId, model.Channel, model.PostingUrl, model.UpdatedBy);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to post job externally. Please try again later." };
            }
        }

        private static string Slugify(string title) =>
            string.Join("-", title.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Replace("/", "-").Replace("--", "-");
    }

    public class PublishJobPostingRequest
    {
        public int JobPostingId { get; set; }
        public int? UpdatedBy { get; set; }
    }

    public class PostExternallyRequest
    {
        public int JobPostingId { get; set; }
        public string Channel { get; set; } = string.Empty;
        public string? PostingUrl { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
