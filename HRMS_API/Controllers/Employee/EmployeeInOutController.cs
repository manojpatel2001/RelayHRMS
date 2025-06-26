using HRMS_Core.Employee;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeInOutController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public EmployeeInOutController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("CreateEmpINOut")]
        public async Task<APIResponse> CreateEmpINOut([FromBody] EmployeeInOutRecord model)
        {
            try
            {
                var data = await _unitOfWork.EmployeeInOut.CreateEmpInOut(model);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Records have been added successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to add records. Please try again later."
                };
            }
        }

        [HttpPost("UpdateEmpOutTime")]
        public async Task<APIResponse> UpdateEmpOutTime([FromBody] EmployeeInOutRecord request)
        {
            try
            {
                if (request.Emp_Id == null || request.For_Date == null || request.Out_Time == null || string.IsNullOrWhiteSpace(request.UpdatedBy))
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Emp_Id, For_Date, Out_Time, and UpdatedBy are required."
                    };
                }
                bool isUpdated = await _unitOfWork.EmployeeInOut.UpdateEmployeeOutTimeAsync(
                    request.Emp_Id.Value,
                    request.For_Date.Value,
                    request.Out_Time.Value,
                    request.UpdatedBy
                );

                if (!isUpdated)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No matching IN record found or update failed."
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    ResponseMessage = "Out time updated successfully."
                };
            }
            catch (Exception ex)
            {
                // Optional: Log ex
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "An error occurred while updating out time."
                };
            }
        }


    }
}
