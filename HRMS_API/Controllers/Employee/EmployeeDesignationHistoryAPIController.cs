using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDesignationHistoryAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeDesignationHistoryAPIController(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

        [HttpPost("CreateEmployeeDesignationHistory")]
        public async Task<APIResponse> CreateEmployeeDesignationHistory([FromBody] EmployeeDesignationHistory model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Designation history details cannot be null." };

                if (model.NewDesignationId == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "New Designation is required." };

                var result = await _unitOfWork.EmployeeDesignationHistoryRepository.CreateEmployeeDesignationHistory(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateEmployeeDesignationHistory")]
        public async Task<APIResponse> UpdateEmployeeDesignationHistory([FromBody] EmployeeDesignationHistory model)
        {
            try
            {
                if (model == null || model.EmployeeDesignationHistoryId <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid designation history record." };

                var result = await _unitOfWork.EmployeeDesignationHistoryRepository.UpdateEmployeeDesignationHistory(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteEmployeeDesignationHistory")]
        public async Task<APIResponse> DeleteEmployeeDesignationHistory([FromBody] DeleteRecordVM request)
        {
            try
            {
                if (request == null || request.Id <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid request." };

                var result = await _unitOfWork.EmployeeDesignationHistoryRepository.DeleteEmployeeDesignationHistory(request);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

        [HttpGet("GetEmployeeDesignationHistoryByEmployeeId/{EmployeeId}")]
        public async Task<APIResponse> GetEmployeeDesignationHistoryByEmployeeId(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeDesignationHistoryRepository.GetEmployeeDesignationHistoryByEmployeeId(EmployeeId);
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records, Please try again later!" };
            }
        }
    }
}
