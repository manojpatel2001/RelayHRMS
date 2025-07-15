using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.Salary;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Core.VM.PasswordHistory;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordHistoryAPIController : ControllerBase
    {
        private readonly UserManager<HRMSUserIdentity> _userManager;

        private readonly IUnitOfWork _unitOfWork;

        public PasswordHistoryAPIController(IUnitOfWork unitOfWork, UserManager<HRMSUserIdentity> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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
        public async Task<APIResponse> CreatePasswordHistory(VmPasswordHistory password)
        {
            try
            {
                if (password == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Details cannot be null" };
                }
                var existingOldPasswords = await _unitOfWork.EmployeeManageRepository.GetAsync(x => x.Id == password.EMPID && x.Password == password.CurrentPassword);

                if (existingOldPasswords == null)
                {
                    return new APIResponse()
                    {
                        isSuccess = false,
                        ResponseMessage = "Incorrect old password."
                    };
                }
                var oldUser = await _userManager.FindByIdAsync(password.EMPID.ToString());
                if (oldUser == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Employee not found." };
                }

                var history = new PasswordHistory
                {
                    EMPID=password.EMPID,
                    NewPassword = password.NewPassword,
                    CreatedBy=password.EMPID.ToString()
                };
                var CreateHistory = await _unitOfWork.PasswordHistory.CreateHistoryPassword(history);
                if (CreateHistory.Id <1)
                {
                    if (CreateHistory.Id == -1)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = "New Password must be different then last three password!" };

                    }
                    else
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = "Some thing went wrong. Please try again!" };

                    }
                }
                if (!string.IsNullOrEmpty(password.NewPassword))
                {

                    var token = await _userManager.GeneratePasswordResetTokenAsync(oldUser);
                    var resetPasswordResult = await _userManager.ResetPasswordAsync(oldUser, token, password.NewPassword);
                    if (!resetPasswordResult.Succeeded)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = "Failed to change password." };
                    }
                }
                var updatedPassword = await _unitOfWork.PasswordHistory.ChangePassword(history);
                
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
