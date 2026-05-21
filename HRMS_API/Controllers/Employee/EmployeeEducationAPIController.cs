using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeEducationAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeEducationAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        [HttpPost("CreateEmployeeEducation")]
        public async Task<APIResponse> CreateEducation([FromBody] VmEducation model)
        {
            var response = new APIResponse();
            if (model == null )
            {
                response.isSuccess = false;
                response.ResponseMessage = "Invalid Education  data.";
                return response;
            }
            try
            {
                var result = await _unitOfWork.employeeProfileEducationRepository.CreateEmployeeEducation(model);
                response.isSuccess = true;
                response.ResponseMessage = "Education  created successfully.";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred while creating the education. Details: {ex.Message}";
            }

            return response;
        }

        [HttpPut("UpdateEmployeeEducation")]
        public async Task<APIResponse> UpdateEmployeeEducation(VmEducation model)
        {
            try
            {
                if (model == null || model.EducationId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Skill details cannot be null." };


                var result = await _unitOfWork.employeeProfileEducationRepository.UpdateEmployeeEducation(model);

                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };


            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteEmployeeEducation")]
        public async Task<APIResponse> DeleteEmployeeEducation(VmEducation model)
        {
            try
            {
                if (model == null || model.EducationId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };


                var result = await _unitOfWork.employeeProfileEducationRepository.DeleteEmployeeEducation(model);

                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)

            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }


        [HttpGet("GetAllEducation")]
        public async Task<APIResponse> GetAllEducation()
        {
            try
            {
                var data = await _unitOfWork.employeeProfileEducationRepository.GetAllEmployeeEducation();
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
