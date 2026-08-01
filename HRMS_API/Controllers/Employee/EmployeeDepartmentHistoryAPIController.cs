using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDepartmentHistoryAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeDepartmentHistoryAPIController(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

        [HttpPost("CreateEmployeeDepartmentHistory")]
        public async Task<APIResponse> CreateEmployeeDepartmentHistory([FromBody] EmployeeDepartmentHistory model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Department history details cannot be null." };

                if (model.NewDepartmentId == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "New Department is required." };

                var result = await _unitOfWork.EmployeeDepartmentHistoryRepository.CreateEmployeeDepartmentHistory(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateEmployeeDepartmentHistory")]
        public async Task<APIResponse> UpdateEmployeeDepartmentHistory([FromBody] EmployeeDepartmentHistory model)
        {
            try
            {
                if (model == null || model.EmployeeDepartmentHistoryId <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid department history record." };

                var result = await _unitOfWork.EmployeeDepartmentHistoryRepository.UpdateEmployeeDepartmentHistory(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteEmployeeDepartmentHistory")]
        public async Task<APIResponse> DeleteEmployeeDepartmentHistory([FromBody] DeleteRecordVM request)
        {
            try
            {
                if (request == null || request.Id <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid request." };

                var result = await _unitOfWork.EmployeeDepartmentHistoryRepository.DeleteEmployeeDepartmentHistory(request);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

        [HttpGet("GetEmployeeDepartmentHistoryByEmployeeId/{EmployeeId}")]
        public async Task<APIResponse> GetEmployeeDepartmentHistoryByEmployeeId(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeDepartmentHistoryRepository.GetEmployeeDepartmentHistoryByEmployeeId(EmployeeId);
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records, Please try again later!" };
            }
        }
    }
}
