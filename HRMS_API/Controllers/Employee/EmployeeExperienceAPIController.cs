using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeExperienceAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeExperienceAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("CreateEmployeeExperience")]
        public async Task<APIResponse> CreateEmployeeExperience([FromBody] VmExperience model)
        {
            var response = new APIResponse();
            if (model == null)
            {
                response.isSuccess = false;
                response.ResponseMessage = "Invalid Education  data.";
                return response;
            }
            try
            {
                var result = await _unitOfWork.employeeProfileExperienceRepository.CreateEmployeeExperience(model);
                response.isSuccess = true;
                response.ResponseMessage = "Experience created successfully.";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred while creating the experience. Details: {ex.Message}";
            }

            return response;
        }

        [HttpPut("UpdateEmployeeExperience")]
        public async Task<APIResponse> UpdateEmployeeExperience(VmExperience model)
        {
            var response = new APIResponse();

            if (model == null || model.ExperienceId <= 0)
            {
                response.isSuccess = false;
                response.ResponseMessage = "Invalid Experience data.";
                return response;
            }
            try
            {
                var spResult = await _unitOfWork.employeeProfileExperienceRepository.UpdateEmployeeExperience(model);

                if (spResult != null && spResult.Success == 1)
                {
                    response.isSuccess = true;
                    response.ResponseMessage = spResult.ResponseMessage;
                    response.Data = spResult;
                }
                else
                {
                    response.isSuccess = false;
                    response.ResponseMessage = spResult?.ResponseMessage ?? "Failed to update experience.";
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred while updating the experience. Details: {ex.Message}";
                
            }

            return response;
        }

        [HttpDelete("DeleterEmployeeExperience")]
        public async Task<APIResponse> DeleterEmployeeExperience(VmExperience model)
        {
            try
            {
                if (model == null || model.ExperienceId == 0)
                    return new APIResponse { isSuccess = false, 
                   ResponseMessage = "Delete details cannot be null." };
                var result = await _unitOfWork.employeeProfileExperienceRepository.DeleteEmployeeExperience(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, 
                    ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false,
                ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false,
                ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }



        [HttpGet("GetAllEmployeeExperience")]
        public async Task<APIResponse> GetAllEmployeeExperience()
        {
            try
            {
                var data = await _unitOfWork.employeeProfileExperienceRepository.GetAllEmployeeExperience();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }


    }
}
