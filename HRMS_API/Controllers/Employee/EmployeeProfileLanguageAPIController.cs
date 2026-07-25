using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeProfileLanguageAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeProfileLanguageAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("AddEmployeeProfileLanguage")]
        public async Task<APIResponse> AddEmployeeProfileLanguage([FromBody] VmLanguage model)
        {
            var response = new APIResponse();

            if (model == null )
            {
                response.isSuccess = false;
                response.ResponseMessage = "Invalid skill data.";
                return response;
            }
            try
            {
                var result = await _unitOfWork.employeeProfileLanguageRepository.AddEmpProfileLanguge(model);

                response.isSuccess = true;
                response.ResponseMessage = "Skill created successfully.";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred while creating the skill. Details: {ex.Message}";
            }
            return response;
        }

        [HttpPut("UpdateEmployeeProfileLanguage")]
        public async Task<APIResponse> UpdateEmployeeProfileLanguage(VmLanguage model)
        {
            try
            {
                if (model == null || model.LanguageId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Language details cannot be null." };


                var result = await _unitOfWork.employeeProfileLanguageRepository.UpdateEmpProfileLanguge(model);

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

        [HttpDelete("DeleteEmployeeProfileLanguage")]
        public async Task<APIResponse> DeleteEmployeeProfileLanguage(VmLanguage model)
        {
            try
            {
                if (model == null || model.LanguageId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };


                var result = await _unitOfWork.employeeProfileLanguageRepository.DeleteEmpProfileLanguge(model);

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

        [HttpGet("GetEmployeeProfileLanguage")]
        public async Task<APIResponse> GetEmployeeProfileLanguage()
        {
            try
            {
                var data = await _unitOfWork.employeeProfileLanguageRepository.GetAllEmpProfileLanguage();
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
