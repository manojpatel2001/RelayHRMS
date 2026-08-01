using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeWarningHistoryAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeWarningHistoryAPIController(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

        [HttpPost("CreateEmployeeWarningHistory")]
        public async Task<APIResponse> CreateEmployeeWarningHistory([FromBody] EmployeeWarningHistory model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Warning details cannot be null." };

                if (model.WarningMasterId == null || model.WarningMasterId <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Warning Type is required." };

                var result = await _unitOfWork.EmployeeWarningHistoryRepository.CreateEmployeeWarningHistory(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateEmployeeWarningHistory")]
        public async Task<APIResponse> UpdateEmployeeWarningHistory([FromBody] EmployeeWarningHistory model)
        {
            try
            {
                if (model == null || model.EmployeeWarningHistoryId <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid warning record." };

                var result = await _unitOfWork.EmployeeWarningHistoryRepository.UpdateEmployeeWarningHistory(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteEmployeeWarningHistory")]
        public async Task<APIResponse> DeleteEmployeeWarningHistory([FromBody] DeleteRecordVM request)
        {
            try
            {
                if (request == null || request.Id <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid request." };

                var result = await _unitOfWork.EmployeeWarningHistoryRepository.DeleteEmployeeWarningHistory(request);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

        [HttpGet("GetEmployeeWarningHistoryByEmployeeId/{EmployeeId}")]
        public async Task<APIResponse> GetEmployeeWarningHistoryByEmployeeId(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeWarningHistoryRepository.GetEmployeeWarningHistoryByEmployeeId(EmployeeId);
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records, Please try again later!" };
            }
        }
    }
}
