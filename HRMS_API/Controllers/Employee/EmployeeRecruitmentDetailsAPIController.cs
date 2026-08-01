using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeRecruitmentDetailsAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeRecruitmentDetailsAPIController(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

        [HttpPost("CreateEmployeeRecruitmentDetails")]
        public async Task<APIResponse> CreateEmployeeRecruitmentDetails([FromBody] EmployeeRecruitmentDetails model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Recruitment details cannot be null." };

                var result = await _unitOfWork.EmployeeRecruitmentDetailsRepository.CreateEmployeeRecruitmentDetails(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateEmployeeRecruitmentDetails")]
        public async Task<APIResponse> UpdateEmployeeRecruitmentDetails([FromBody] EmployeeRecruitmentDetails model)
        {
            try
            {
                if (model == null || model.EmployeeRecruitmentDetailsId <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid recruitment details record." };

                var result = await _unitOfWork.EmployeeRecruitmentDetailsRepository.UpdateEmployeeRecruitmentDetails(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteEmployeeRecruitmentDetails")]
        public async Task<APIResponse> DeleteEmployeeRecruitmentDetails([FromBody] DeleteRecordVM request)
        {
            try
            {
                if (request == null || request.Id <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid request." };

                var result = await _unitOfWork.EmployeeRecruitmentDetailsRepository.DeleteEmployeeRecruitmentDetails(request);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

        [HttpGet("GetEmployeeRecruitmentDetailsByEmployeeId/{EmployeeId}")]
        public async Task<APIResponse> GetEmployeeRecruitmentDetailsByEmployeeId(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeRecruitmentDetailsRepository.GetEmployeeRecruitmentDetailsByEmployeeId(EmployeeId);
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records, Please try again later!" };
            }
        }
    }
}
