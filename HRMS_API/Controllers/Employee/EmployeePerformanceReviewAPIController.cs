using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeePerformanceReviewAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeePerformanceReviewAPIController(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

        [HttpPost("CreateEmployeePerformanceReview")]
        public async Task<APIResponse> CreateEmployeePerformanceReview([FromBody] EmployeePerformanceReview model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Performance review details cannot be null." };

                if (model.ReviewDate == default)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Review Date is required." };

                var result = await _unitOfWork.EmployeePerformanceReviewRepository.CreateEmployeePerformanceReview(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateEmployeePerformanceReview")]
        public async Task<APIResponse> UpdateEmployeePerformanceReview([FromBody] EmployeePerformanceReview model)
        {
            try
            {
                if (model == null || model.EmployeePerformanceReviewId <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid performance review record." };

                var result = await _unitOfWork.EmployeePerformanceReviewRepository.UpdateEmployeePerformanceReview(model);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteEmployeePerformanceReview")]
        public async Task<APIResponse> DeleteEmployeePerformanceReview([FromBody] DeleteRecordVM request)
        {
            try
            {
                if (request == null || request.Id <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid request." };

                var result = await _unitOfWork.EmployeePerformanceReviewRepository.DeleteEmployeePerformanceReview(request);

                return result.Success > 0
                    ? new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage }
                    : new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

        [HttpGet("GetEmployeePerformanceReviewByEmployeeId/{EmployeeId}")]
        public async Task<APIResponse> GetEmployeePerformanceReviewByEmployeeId(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeePerformanceReviewRepository.GetEmployeePerformanceReviewByEmployeeId(EmployeeId);
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records, Please try again later!" };
            }
        }
    }
}
