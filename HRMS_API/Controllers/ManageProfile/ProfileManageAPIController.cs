using HRMS_API.Services;
using HRMS_Core.DbContext;
using HRMS_Core.ProfileManage;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.ManageProfile
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileManageAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileUploadService _fileUploadService;


        public ProfileManageAPIController(IUnitOfWork unitOfWork, FileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
           
            _fileUploadService = fileUploadService;
        }

        // GET endpoint to retrieve personal info
        [HttpGet("GetPersonalInfo/{employeeId}")]
        public async Task<APIResponse> GetPersonalInfo(int employeeId)
        {
            try
            {
                var data = await _unitOfWork.ManageProfileRepository.GetPersonalInfo(employeeId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        // GET endpoint to retrieve contact details
        [HttpGet("GetContactDetails/{employeeId}")]
        public async Task<APIResponse> GetContactDetails(int employeeId)
        {
            try
            {
                var data = await _unitOfWork.ManageProfileRepository.GetContactDetails(employeeId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        // GET endpoint to retrieve salary details
        [HttpGet("GetSalaryDetails/{employeeId}")]
        public async Task<APIResponse> GetSalaryDetails(int employeeId)
        {
            try
            {
                var data = await _unitOfWork.ManageProfileRepository.GetSalaryDetails(employeeId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        // PUT endpoint to update personal info
        [HttpPost("UpdatePersonalInfo")]
        public async Task<APIResponse> UpdatePersonalInfo(vmPersonalInfo vmPersonalInfo)
        {
            try
            {
                if (vmPersonalInfo == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Personal info details cannot be null." };
                var result = await _unitOfWork.ManageProfileRepository.UpdatePersonalInfo(vmPersonalInfo);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        // PUT endpoint to update contact details
        [HttpPost("UpdateContactDetails")]
        public async Task<APIResponse> UpdateContactDetails(vmContactDetails vmContactDetails)
        {
            try
            {
                if (vmContactDetails == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Contact details cannot be null." };
                var result = await _unitOfWork.ManageProfileRepository.UpdateContactDetails(vmContactDetails);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        // PUT endpoint to update salary details
        [HttpPost("UpdateSalaryDetails")]
        public async Task<APIResponse> UpdateSalaryDetails(vmSalaryDetails vmSalaryDetails)
        {
            try
            {
                if (vmSalaryDetails == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Salary details cannot be null." };
                var result = await _unitOfWork.ManageProfileRepository.UpdateSalaryDetails(vmSalaryDetails);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        // GET endpoint to retrieve salary allowance
        [HttpGet("GetEmployeeSalaryAllowance/{employeeId}")]
        public async Task<APIResponse> GetEmployeeSalaryAllowance(int employeeId)
        {
            try
            {
                var data = await _unitOfWork.ManageProfileRepository.GetEmployeeSalaryAllowance(employeeId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("UpdateProfilePic")]
        public async Task<APIResponse> UpdateProfilePic(vmUpdateEmployeeProfile model)
        {
            try
            {
                if (model == null || model.EmployeeId == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Company details cannot be null." };
                var check = await _unitOfWork.EmployeeManageRepository.GetEmployeeById((int)model.EmployeeId);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid  record." };


                if (model.EmployeeProfileFile != null)
                {
                    if (model.EmployeeProfileFile.Length > 0)
                    {

                        var folder = $"uploads/employeeprofile";
                        var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync(model.EmployeeProfileFile, folder, check.EmployeeProfileUrl);
                        if (string.IsNullOrEmpty(fileUrl))
                        {
                            return new APIResponse { isSuccess = false, ResponseMessage = "Some thing went wrong. Please try again later." };

                        }
                        model.EmployeeProfileUrl = fileUrl;
                    }
                    
                }
                var result = await _unitOfWork.ManageProfileRepository.UpdateProfilePic(model);

                if (result.Success > 0)
                {
                    var updatedProfile = await _unitOfWork.EmployeeManageRepository.GetEmployeeById((int)model.EmployeeId);
                    return new APIResponse { isSuccess = true, Data = updatedProfile, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to  update employee profile. Please try again later." };
            }
        }

    }
}