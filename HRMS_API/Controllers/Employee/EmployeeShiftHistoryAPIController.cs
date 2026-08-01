using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeShiftHistoryAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeShiftHistoryAPIController(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

        [HttpPost("CreateEmployeeShiftHistory")]
        public async Task<APIResponse> CreateEmployeeShiftHistory([FromBody] EmployeeShiftHistory model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Shift history details cannot be null." };

                if (model.NewShiftMasterId == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "New Shift is required." };

                var result = await _unitOfWork.EmployeeShiftHistoryRepository.CreateEmployeeShiftHistory(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateEmployeeShiftHistory")]
        public async Task<APIResponse> UpdateEmployeeShiftHistory([FromBody] EmployeeShiftHistory model)
        {
            try
            {
                if (model == null || model.EmployeeShiftHistoryId <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid shift history record." };

                var result = await _unitOfWork.EmployeeShiftHistoryRepository.UpdateEmployeeShiftHistory(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteEmployeeShiftHistory")]
        public async Task<APIResponse> DeleteEmployeeShiftHistory([FromBody] DeleteRecordVM request)
        {
            try
            {
                if (request == null || request.Id <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid request." };

                var result = await _unitOfWork.EmployeeShiftHistoryRepository.DeleteEmployeeShiftHistory(request);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

        [HttpGet("GetEmployeeShiftHistoryByEmployeeId/{EmployeeId}")]
        public async Task<APIResponse> GetEmployeeShiftHistoryByEmployeeId(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeShiftHistoryRepository.GetEmployeeShiftHistoryByEmployeeId(EmployeeId);
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records, Please try again later!" };
            }
        }
    }
}
