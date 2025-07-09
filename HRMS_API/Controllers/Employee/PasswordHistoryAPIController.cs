using HRMS_Core.Employee;
using HRMS_Core.Salary;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordHistoryAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PasswordHistoryAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("GetAllPasswordHistory")]
        public async Task<APIResponse> GetAllPasswordHistory()
        {
            try
            {
                var data = await _unitOfWork.PasswordHistory.GetAllAsync();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpPost("CreatePasswordHistory")]
        public async Task<APIResponse> CreatePasswordHistory(PasswordHistory password)
        {
            try
            {
                if (password == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Details cannot be null" };
                }

                var existingSamePasswords = await _unitOfWork.PasswordHistory
                    .GetAllAsync(x => x.EMPID == password.EMPID && x.CurrentPassword == password.CurrentPassword);

                if (existingSamePasswords != null && existingSamePasswords.Count() >= 3)
                {
                    return new APIResponse()
                    {
                        isSuccess = false,
                        ResponseMessage = "You have already used this password 3 times. Please use a new password."
                    };
                }

                // ✅ Step 2: Save new password history
                password.CreatedDate = DateTime.UtcNow;
                await _unitOfWork.PasswordHistory.AddAsync(password);
                await _unitOfWork.CommitAsync();

                return new APIResponse()
                {
                    isSuccess = true,
                    Data = password,
                    ResponseMessage = "The record has been saved successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to add records, please try again later!"
                };
            }
        }

    }
}
