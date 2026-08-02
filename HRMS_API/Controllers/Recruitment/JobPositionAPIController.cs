using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPositionAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobPositionAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("GetAllJobPositions")]
        public async Task<APIResponse> GetAllJobPositions(CommonParameter commonParameter)
        {
            try
            {
                return await _unitOfWork.JobPositionRepository.GetAllJobPositions(commonParameter);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve job positions. Please try again later." };
            }
        }

        [HttpGet("GetJobPositionById/{jobPositionId}")]
        public async Task<APIResponse> GetJobPositionById(int jobPositionId)
        {
            try
            {
                return await _unitOfWork.JobPositionRepository.GetJobPositionById(jobPositionId);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve job position. Please try again later." };
            }
        }

        [HttpPost("CreateJobPosition")]
        public async Task<APIResponse> CreateJobPosition([FromBody] JobPositionMaster model)
        {
            try
            {
                if (model == null || model.ManpowerRequisitionId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Job position must reference an approved Manpower Requisition." };

                model.CreatedDate = DateTime.UtcNow;
                return await _unitOfWork.JobPositionRepository.CreateJobPosition(model);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create job position. Please try again later." };
            }
        }

        [HttpPost("UpdateJobPosition")]
        public async Task<APIResponse> UpdateJobPosition([FromBody] JobPositionMaster model)
        {
            try
            {
                if (model == null || model.JobPositionId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid job position." };

                return await _unitOfWork.JobPositionRepository.UpdateJobPosition(model);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update job position. Please try again later." };
            }
        }

        [HttpDelete("DeleteJobPosition")]
        public async Task<APIResponse> DeleteJobPosition(DeleteRecordVM model)
        {
            try
            {
                return await _unitOfWork.JobPositionRepository.DeleteJobPosition(model);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete job position. Please try again later." };
            }
        }

        [HttpGet("GetJobPositionDropdown/{companyId}")]
        public async Task<APIResponse> GetJobPositionDropdown(int companyId)
        {
            try
            {
                return await _unitOfWork.JobPositionRepository.GetJobPositionDropdown(companyId);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve dropdown data. Please try again later." };
            }
        }

        [HttpGet("GetJobPositionsByRequisition/{manpowerRequisitionId}")]
        public async Task<APIResponse> GetJobPositionsByRequisition(int manpowerRequisitionId)
        {
            try
            {
                return await _unitOfWork.JobPositionRepository.GetJobPositionsByManpowerRequisitionId(manpowerRequisitionId);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve job positions. Please try again later." };
            }
        }
    }
}
