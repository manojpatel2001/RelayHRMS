using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAwardAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeAwardAPIController(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

        [HttpPost("CreateEmployeeAward")]
        public async Task<APIResponse> CreateEmployeeAward([FromBody] EmployeeAward model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Award details cannot be null." };

                if (string.IsNullOrWhiteSpace(model.AwardName))
                    return new APIResponse { isSuccess = false, ResponseMessage = "Award Name is required." };

                var result = await _unitOfWork.EmployeeAwardRepository.CreateEmployeeAward(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateEmployeeAward")]
        public async Task<APIResponse> UpdateEmployeeAward([FromBody] EmployeeAward model)
        {
            try
            {
                if (model == null || model.EmployeeAwardId <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid award record." };

                var result = await _unitOfWork.EmployeeAwardRepository.UpdateEmployeeAward(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteEmployeeAward")]
        public async Task<APIResponse> DeleteEmployeeAward([FromBody] DeleteRecordVM request)
        {
            try
            {
                if (request == null || request.Id <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid request." };

                var result = await _unitOfWork.EmployeeAwardRepository.DeleteEmployeeAward(request);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

        [HttpGet("GetEmployeeAwardByEmployeeId/{EmployeeId}")]
        public async Task<APIResponse> GetEmployeeAwardByEmployeeId(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeAwardRepository.GetEmployeeAwardByEmployeeId(EmployeeId);
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records, Please try again later!" };
            }
        }
    }
}
