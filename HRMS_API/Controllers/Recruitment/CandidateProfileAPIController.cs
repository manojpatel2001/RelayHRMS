using HRMS_API.Services;
using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Recruitment
{
    // Combined controller for the 5 identical-shape Candidate profile child resources
    // (Education/Experience/Skill/Certification/Project) — grouped to avoid 5 near-duplicate
    // controllers; routes are namespaced per sub-resource (api/CandidateProfile/Education/...).
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateProfileAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileUploadService _fileUploadService;

        public CandidateProfileAPIController(IUnitOfWork unitOfWork, FileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
        }

        // ===== Education =====
        [HttpPost("Education/Create")]
        public async Task<APIResponse> CreateEducation([FromBody] CandidateEducation model) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.CreateEducation(model));

        [HttpPost("Education/Update")]
        public async Task<APIResponse> UpdateEducation([FromBody] CandidateEducation model) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.UpdateEducation(model));

        [HttpDelete("Education/Delete")]
        public async Task<APIResponse> DeleteEducation(DeleteRecordVM model) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.DeleteEducation(model));

        [HttpGet("Education/GetAll/{candidateId}")]
        public async Task<APIResponse> GetEducation(int candidateId) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.GetEducationByCandidateId(candidateId));

        // ===== Experience =====
        [HttpPost("Experience/Create")]
        public async Task<APIResponse> CreateExperience([FromBody] CandidateExperience model) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.CreateExperience(model));

        [HttpPost("Experience/Update")]
        public async Task<APIResponse> UpdateExperience([FromBody] CandidateExperience model) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.UpdateExperience(model));

        [HttpDelete("Experience/Delete")]
        public async Task<APIResponse> DeleteExperience(DeleteRecordVM model) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.DeleteExperience(model));

        [HttpGet("Experience/GetAll/{candidateId}")]
        public async Task<APIResponse> GetExperience(int candidateId) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.GetExperienceByCandidateId(candidateId));

        // ===== Skill =====
        [HttpPost("Skill/Create")]
        public async Task<APIResponse> CreateSkill([FromBody] CandidateSkill model) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.CreateSkill(model));

        [HttpPost("Skill/Update")]
        public async Task<APIResponse> UpdateSkill([FromBody] CandidateSkill model) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.UpdateSkill(model));

        [HttpDelete("Skill/Delete")]
        public async Task<APIResponse> DeleteSkill(DeleteRecordVM model) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.DeleteSkill(model));

        [HttpGet("Skill/GetAll/{candidateId}")]
        public async Task<APIResponse> GetSkill(int candidateId) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.GetSkillByCandidateId(candidateId));

        // ===== Certification =====
        [HttpPost("Certification/Create")]
        public async Task<APIResponse> CreateCertification([FromForm] CandidateCertification model, IFormFile? certificateFile)
        {
            try
            {
                if (certificateFile != null && certificateFile.Length > 0)
                {
                    var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync(certificateFile, "uploads/candidate-certification", model.CertificateUrl);
                    if (string.IsNullOrEmpty(fileUrl))
                        return new APIResponse { isSuccess = false, ResponseMessage = "Certificate upload failed. Please try again later." };
                    model.CertificateUrl = fileUrl;
                }
                return await _unitOfWork.CandidateProfileRepository.CreateCertification(model);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create certification. Please try again later." };
            }
        }

        [HttpPost("Certification/Update")]
        public async Task<APIResponse> UpdateCertification([FromForm] CandidateCertification model, IFormFile? certificateFile)
        {
            try
            {
                if (certificateFile != null && certificateFile.Length > 0)
                {
                    var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync(certificateFile, "uploads/candidate-certification", model.CertificateUrl);
                    if (string.IsNullOrEmpty(fileUrl))
                        return new APIResponse { isSuccess = false, ResponseMessage = "Certificate upload failed. Please try again later." };
                    model.CertificateUrl = fileUrl;
                }
                return await _unitOfWork.CandidateProfileRepository.UpdateCertification(model);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update certification. Please try again later." };
            }
        }

        [HttpDelete("Certification/Delete")]
        public async Task<APIResponse> DeleteCertification(DeleteRecordVM model) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.DeleteCertification(model));

        [HttpGet("Certification/GetAll/{candidateId}")]
        public async Task<APIResponse> GetCertification(int candidateId) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.GetCertificationByCandidateId(candidateId));

        // ===== Project =====
        [HttpPost("Project/Create")]
        public async Task<APIResponse> CreateProject([FromBody] CandidateProject model) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.CreateProject(model));

        [HttpPost("Project/Update")]
        public async Task<APIResponse> UpdateProject([FromBody] CandidateProject model) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.UpdateProject(model));

        [HttpDelete("Project/Delete")]
        public async Task<APIResponse> DeleteProject(DeleteRecordVM model) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.DeleteProject(model));

        [HttpGet("Project/GetAll/{candidateId}")]
        public async Task<APIResponse> GetProject(int candidateId) =>
            await Safe(() => _unitOfWork.CandidateProfileRepository.GetProjectByCandidateId(candidateId));

        private static async Task<APIResponse> Safe(Func<Task<APIResponse>> action)
        {
            try
            {
                return await action();
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to process request. Please try again later." };
            }
        }
    }
}
