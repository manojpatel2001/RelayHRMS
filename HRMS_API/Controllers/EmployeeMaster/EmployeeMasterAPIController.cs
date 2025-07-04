﻿using HRMS_Core.DbContext;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.Helper;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyInformation;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRMS_API.Controllers.EmployeeMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeMasterAPIController : ControllerBase
    {
        private readonly UserManager<HRMSUserIdentity> _userManager;
        
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeMasterAPIController(IUnitOfWork unitOfWork, UserManager<HRMSUserIdentity> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }


        [HttpGet("GetAllEmployee")]
        public async Task<APIResponse> GetAllEmployee(int companyId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetAllEmployee(companyId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetNextEmployeeCode/{companyId}")]
        public async Task<APIResponse> GetNextEmployeeCode(int companyId)
        {
            try
            {
                var nextEmployeeCode = await _unitOfWork.EmployeeManageRepository.GetNextEmployeeCode(companyId);
                var companyDetails = await _unitOfWork.CompanyDetailsRepository.GetByCompanyId(companyId);
                if (companyDetails == null )
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                var newData = new
                {
                    NextEmployeeCode = nextEmployeeCode.NextEmployeeCode,
                    CompanyCode = companyDetails.CompanyCode,
                    SampleCode = companyDetails.SampleCode,
                    DigitsForEmployeeCode = companyDetails.DigitsForEmployeeCode,

                };

                return new APIResponse { isSuccess = true, Data = newData, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }


        [HttpPost("CreateEmployee")]
        public async Task<APIResponse> CreateEmployee(vmEmployeeData employeeData)
        {
            
            try
            {
                if (employeeData == null||employeeData.LoginAlias==null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Employee details cannot be null" };
                }

                // Check if the user already exists
                var existingUser = await _userManager.FindByEmailAsync(employeeData.LoginAlias);
                if (existingUser != null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "An employee with the same Login Alias already exists." };
                }

                var existingUserByEmployeeCode = await _unitOfWork.EmployeeManageRepository.GetAllAsync(u => u.EmployeeCode == employeeData.EmployeeCode);

                if (existingUserByEmployeeCode.Any())
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "An employee with the same Employee Code already exists." };
                }
                // Map vmEmployeeData to HRMSUserIdentity
                if (string.IsNullOrEmpty(employeeData.Password))
                {
                    employeeData.Password = "Hrms@123";
                }
                var employee = new HRMSUserIdentity
                {
                    UserName = employeeData.LoginAlias,
                    Email = employeeData.LoginAlias,
                    Password=employeeData.Password,
                    Initial = employeeData.Initial,
                    FirstName = employeeData.FirstName,
                    MiddleName = employeeData.MiddleName,
                    LastName = employeeData.LastName,
                    FullName = employeeData.FullName,
                    EmployeeCode = employeeData.EmployeeCode,
                    DateOfJoining = employeeData.DateOfJoining,
                    BranchId = employeeData.BranchId,
                    GradeId = employeeData.GradeId,
                    ShiftMasterId = employeeData.ShiftMasterId,
                    CTC = employeeData.CTC,
                    DesignationId = employeeData.DesignationId,
                    GrossSalary = employeeData.GrossSalary,
                    CategoryId = employeeData.CategoryId,
                    BasicSalary = employeeData.BasicSalary,
                    DepartmentId = employeeData.DepartmentId,
                    EmployeeTypeId = employeeData.EmployeeTypeId,
                    DateOfBirth = employeeData.DateOfBirth,
                    UserPrivilege = employeeData.UserPrivilege,
                    LoginAlias = employeeData.LoginAlias,
                    ReportingManager = employeeData.ReportingManager,
                    SubBranch = employeeData.SubBranch,
                    EnrollNo = employeeData.EnrollNo,
                    CompanyId = employeeData.CompanyId,
                    Overtime = employeeData.Overtime,
                    Latemark = employeeData.Latemark,
                    Earlymark = employeeData.Earlymark,
                    Fullpf = employeeData.Fullpf,
                    Pt = employeeData.Pt,
                    Fixsalary = employeeData.Fixsalary,
                    Probation = employeeData.Probation,
                    Trainee = employeeData.Trainee
                };

                // Create the user
                var result = await _userManager.CreateAsync(employee, employeeData.Password);

                if (result.Succeeded)
                {
                    var userRole = new HRMSUserRole
                    {
                        EmployeeId = employee.Id,
                        CompanyId= employee.CompanyId,
                        RoleId = (int)employeeData.RoleId,
                        CreatedBy= employeeData.CreatedBy,
                        CreatedDate=DateTime.UtcNow,
                    };
                      var resultUserRole=await _unitOfWork.HRMSUserRoleRepository.CreateUserRole(userRole);
                    return new APIResponse { isSuccess = true, Data = employee, ResponseMessage = "Employee has been created successfully" };

                }
                return new APIResponse { isSuccess = false,ResponseMessage = "Unable to create employee, Please try again later!" };

            }
            catch (Exception ex)
            {
              return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create employee, Please try again later!" };

            }

        }


        [HttpPost("UpdateEmployee")]
        public async Task<APIResponse> UpdateEmployee(vmUpdateEmployee updateEmployee)
        {
            try
            {
                var employeeData = updateEmployee.vmEmployeeData;
                if (employeeData == null || string.IsNullOrEmpty(employeeData.LoginAlias))
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Employee details cannot be null" };
                }

                // Fetch the existing user using your repository
                var existingUser = await _unitOfWork.EmployeeManageRepository.GetAsync(x => x.Id == employeeData.Id && x.IsDeleted == false && x.IsEnabled == true);
                if (existingUser == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Employee not found." };
                }

                // Check if another user with the same EmployeeCode exists (excluding the current user)
                var existingUserByEmployeeCode = await _unitOfWork.EmployeeManageRepository.GetAllAsync(u => u.EmployeeCode == employeeData.EmployeeCode && u.Id != existingUser.Id);
                if (existingUserByEmployeeCode.Any())
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "An employee with the same Employee Code already exists." };
                }
                var oldUser = await _userManager.FindByIdAsync(employeeData.Id.ToString());
                if (oldUser == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Employee not found." };
                }

                // Update email if a new email is provided
                if (!string.IsNullOrEmpty(oldUser.LoginAlias))
                {
                    var setEmailResult = await _userManager.SetEmailAsync(oldUser, employeeData.LoginAlias);
                    if (!setEmailResult.Succeeded)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = "Failed to update email." };
                    }

                    var setUserNameResult = await _userManager.SetUserNameAsync(oldUser, employeeData.LoginAlias);
                    if (!setUserNameResult.Succeeded)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = "Failed to update username." };
                    }
                }

                // Reset password if a new password is provided
                if (!string.IsNullOrEmpty(employeeData.Password))
                {

                    var token = await _userManager.GeneratePasswordResetTokenAsync(oldUser);
                    var resetPasswordResult = await _userManager.ResetPasswordAsync(oldUser, token, employeeData.Password);
                    if (!resetPasswordResult.Succeeded)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = "Failed to reset password." };
                    }
                }
                else
                {
                    employeeData.Password = "Hrms@123";
                }
                // Update other user properties
                var result = await _unitOfWork.EmployeeManageRepository.UpdateEmployee(employeeData);

                if (result.Id >0)
                {
                    var userRole = new HRMSUserRole
                    {
                        EmployeeId = employeeData.Id,
                        CompanyId = employeeData.CompanyId,
                        RoleId = (int)employeeData.RoleId,
                        CreatedBy = employeeData.CreatedBy,
                        CreatedDate = DateTime.UtcNow,
                    };
                    var checkExist = await _unitOfWork.HRMSUserRoleRepository.GetAsync(x => x.EmployeeId == employeeData.Id && x.IsEnabled == true && x.IsDeleted == false);
                    if (checkExist == null)
                    {
                        var resultUserRole = await _unitOfWork.HRMSUserRoleRepository.CreateUserRole(userRole);
                    }
                    else
                    {
                        var resultUserRole = await _unitOfWork.HRMSUserRoleRepository.UpdateUserRole(userRole);
                    }
                    // Additional information
                    if (updateEmployee.EmployeePersonalInfo != null)
                    {
                        var modelEmployeePersonalInfo = updateEmployee.EmployeePersonalInfo;
                        // Add or update employee personal info
                        if (modelEmployeePersonalInfo.EmployeePersonalInfoId == 0|| modelEmployeePersonalInfo.EmployeePersonalInfoId==null)
                        {
                            var resultEmployeePersonalInfo = await _unitOfWork.EmployeePersonalInfoRepository.CreateEmployeePersonalInfo(modelEmployeePersonalInfo);
                        }
                        else
                        {
                            var check = await _unitOfWork.EmployeePersonalInfoRepository.GetEmployeePersonalInfoById(modelEmployeePersonalInfo.EmployeePersonalInfoId);
                            if (check != null)
                            {
                                var resultUpdatedEmployeePersonalInfo = await _unitOfWork.EmployeePersonalInfoRepository.UpdateEmployeePersonalInfo(modelEmployeePersonalInfo);
                            }
                        }
                    }
                    //Add employee contact
                    if (updateEmployee.EmployeeContact != null)
                    {
                        var modelEmployeeContact = updateEmployee.EmployeeContact;
                        if (modelEmployeeContact.EmployeeContactId == 0 || modelEmployeeContact.EmployeeContactId == null)
                        {
                            var resultEmployeeContact = await _unitOfWork.EmployeeContactRepository.CreateEmployeeContact(modelEmployeeContact);
                        }
                        else
                        {
                            var check = await _unitOfWork.EmployeeContactRepository.GetEmployeeContactById(modelEmployeeContact.EmployeeContactId);
                            if (check != null)
                            {
                                var resultUpdatedEmployeePersonalInfo = await _unitOfWork.EmployeeContactRepository.UpdateEmployeeContact(modelEmployeeContact);
                            }
                        }
                    }


                    var updatedEmployee = await _unitOfWork.EmployeeManageRepository.GetEmployeeById((int)result.Id);
                    return new APIResponse { isSuccess = true, Data = updatedEmployee, ResponseMessage = "Employee has been updated successfully" };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update employee, Please try again later!" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update employee, Please try again later!" };
            }
        }

        //public async Task<APIResponse> UpdateEmployee(vmUpdateEmployee updateEmployee)
        //{
        //    try
        //    {
        //        var employeeData = updateEmployee.vmEmployeeData;
        //        if (employeeData == null || string.IsNullOrEmpty(employeeData.LoginAlias))
        //        {
        //            return new APIResponse { isSuccess = false, ResponseMessage = "Employee details cannot be null" };
        //        }

        //        var existingUser = await _unitOfWork.EmployeeManageRepository.GetAsync(x=>x.Id==employeeData.Id && x.IsDeleted==false && x.IsEnabled==true);
        //        if (existingUser == null)
        //        {
        //            return new APIResponse { isSuccess = false, ResponseMessage = "Employee not found." };
        //        }


        //        // Check if another user with the same EmployeeCode exists (excluding the current user)
        //        var existingUserByEmployeeCode = await _unitOfWork.EmployeeManageRepository.GetAllAsync(u => u.EmployeeCode == employeeData.EmployeeCode && u.Id != existingUser.Id);
        //        if (existingUserByEmployeeCode.Any())
        //        {
        //            return new APIResponse { isSuccess = false, ResponseMessage = "An employee with the same Employee Code already exists." };
        //        }

        //        // Update the user
        //        var result = await _unitOfWork.EmployeeManageRepository.UpdateEmployee(employeeData);

        //        if (result.Emp_Id!=null)
        //        {
        //            // Additional information
        //            if (updateEmployee.EmployeePersonalInfo != null)
        //            {
        //                var modelEmployeePersonalInfo = updateEmployee.EmployeePersonalInfo;
        //                //Add employee info
        //                if (modelEmployeePersonalInfo.EmployeePersonalInfoId == 0)
        //                {
        //                    var resultEmployeePersonalInfo = await _unitOfWork.EmployeePersonalInfoRepository.CreateEmployeePersonalInfo(modelEmployeePersonalInfo);
        //                }
        //                else
        //                {
        //                    var check = await _unitOfWork.EmployeePersonalInfoRepository.GetEmployeePersonalInfoById(modelEmployeePersonalInfo.EmployeePersonalInfoId);
        //                    if (check != null)
        //                    {
        //                        var resultUpdatedEmployeePersonalInfo = await _unitOfWork.EmployeePersonalInfoRepository.UpdateEmployeePersonalInfo(modelEmployeePersonalInfo);
        //                    }
        //                }
        //            }


        //            var updatedEmployee = await _unitOfWork.EmployeeManageRepository.GetEmployeeById(result.Emp_Id);
        //            return new APIResponse { isSuccess = true, Data = updatedEmployee, ResponseMessage = "Employee has been updated successfully" };
        //        }

        //        return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update employee, Please try again later!" };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update employee, Please try again later!" };
        //    }
        //}



        [HttpDelete("DeleteEmployee")]
        public async Task<APIResponse> DeleteEmployee(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.emp_id == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };
                var existingUser = await _unitOfWork.EmployeeManageRepository.GetAsync(x => x.Id == model.emp_id && x.IsDeleted == false && x.IsEnabled == true);
                if (existingUser == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Employee not found." };
                }
                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.EmployeeManageRepository.DeleteEmployee(model);

                if (result.Id !=null)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete city. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

        [HttpGet("GetAllEmployeeByIsBlocked/{IsBlocked}")]
        public async Task<APIResponse> GetAllEmployeeByIsBlocked( bool IsBlocked,int companyId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetAllEmployeeByIsBlocked(IsBlocked, companyId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }
        [HttpGet("GetEmployeeById/{employeeId}")]
        public async Task<APIResponse> GetEmployeeById( int employeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetEmployeeById(employeeId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpPost("UpdateEmployeeProfileAndSignature")]
        public async Task<APIResponse> UpdateEmployeeProfileAndSignature(vmUpdateEmployeeProfile model)
        {
            try
            {
                if (model == null || model.EmployeeId == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Company details cannot be null." };
                var check = await _unitOfWork.EmployeeManageRepository.GetEmployeeById((int)model.EmployeeId);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid company record." };

                if (model.EmployeeProfileFile != null )
                {
                    if (model.EmployeeProfileFile.Length > 0)
                    {
                        var folder = $"uploads/employeeprofile";
                        var fileUrl = await UploadDocument.UploadAndReplaceDocumentAsync(Request, model.EmployeeProfileFile, folder, model.EmployeeProfileUrl);
                        model.EmployeeProfileUrl = fileUrl;
                    }
                }
                if (model.EmployeeSignatureFile != null)
                {
                    if (model.EmployeeSignatureFile.Length > 0)
                    {
                        var folder = $"uploads/employeesignature";
                        var fileUrl = await UploadDocument.UploadAndReplaceDocumentAsync(Request, model.EmployeeSignatureFile, folder, model.EmployeeSignatureUrl);
                        model.EmployeeSignatureUrl = fileUrl;
                    }
                }
                //if ((string.IsNullOrEmpty(model.EmployeeSignatureUrl)|| model.EmployeeSignatureUrl==null)&& model.EmployeeSignatureFile==null)
                //{
                //    model.EmployeeSignatureUrl = null;
                //}
                //if ((string.IsNullOrEmpty(model.EmployeeProfileUrl)|| model.EmployeeProfileUrl=="null")&& model.EmployeeSignatureFile == null)
                //{
                //    model.EmployeeProfileUrl = null;
                //}

                if ((!string.IsNullOrEmpty(model.EmployeeSignatureUrl) || model.EmployeeSignatureUrl != "null") && model.EmployeeSignatureFile == null)
                {
                    var folder = $"uploads/employeesignature";
                    bool isDeleted =  UploadDocument.DeleteUploadedFile(Request,  folder, model.EmployeeSignatureUrl);

                    model.EmployeeSignatureUrl = null;
                }
                if ((!string.IsNullOrEmpty(model.EmployeeProfileUrl) || model.EmployeeProfileUrl != "null") && model.EmployeeSignatureFile == null)
                {
                    var folder = $"uploads/employeeprofile";
                    bool isDeleted = UploadDocument.DeleteUploadedFile(Request, folder, model.EmployeeSignatureUrl);

                    model.EmployeeProfileUrl = null;
                }


                var result = await _unitOfWork.EmployeeManageRepository.UpdateEmployeeProfileAndSignature(model);

                if (result.Id>0)
                {
                    var updatedProfile = await _unitOfWork.EmployeeManageRepository.GetEmployeeById((int)result.Id);
                    return new APIResponse { isSuccess = true, Data= updatedProfile,ResponseMessage = "Employee profile has been updated successfully." };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update employee profile. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to  update employee profile. Please try again later." };
            }
        }








        //[HttpPost("CreateEmployee")]
        //public async Task<APIResponse> CreateEmployee(vmEmployeeData employeeData)
        //{
        //    try
        //    {
        //        if (employeeData == null)
        //        {
        //            return new APIResponse { isSuccess = false, ResponseMessage = "Employee details cannot be null" };
        //        }

        //        // Check for duplicate EmployeeStringCode and EmployeeNumberCode
        //        var existingUserByEmployeeCode = await _unitOfWork.EmployeeManageRepository.GetAllAsync(u =>u.EmployeeCode == employeeData.EmployeeCode);

        //        if (existingUserByEmployeeCode.Any())
        //        {
        //            return new APIResponse { isSuccess = false, ResponseMessage = "An employee with the same Employee Code already exists." };
        //        }

        //        // Check for duplicate LoginAlias
        //        var existingUserByLoginAlias = await _unitOfWork.EmployeeManageRepository.GetAllAsync(u => u.LoginAlias == employeeData.LoginAlias && u.IsDeleted==false &&u.IsEnabled==true);
        //        if (existingUserByLoginAlias.Any())
        //        {
        //            return new APIResponse { isSuccess = false, ResponseMessage = "An employee with the same Login Alias already exists." };
        //        }


        //        var result = await _unitOfWork.EmployeeManageRepository.CreateEmployee(employeeData); 

        //        if (result.Emp_Id!=null)
        //        {
        //            var AddedData=await _unitOfWork.EmployeeManageRepository.GetEmployeeById(result.Emp_Id);
        //            return new APIResponse { isSuccess = true, Data = AddedData, ResponseMessage = "The record has been saved successfully" };
        //        }
        //        else
        //        {
        //            return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add employee.Please try again!" };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record, Please try again later!" };
        //    }
        //}

        [HttpGet("GetAllAdditionalInformation/{employeeId}")]
        public async Task<APIResponse> GetAllAdditionalInformation(string employeeId)
        {
            try
            {
                var personalData = await _unitOfWork.EmployeePersonalInfoRepository.GetEmployeePersonalInfoByEmployeeId(employeeId);
                var employeeContactData = await _unitOfWork.EmployeeContactRepository.GetEmployeeContactByEmployeeId(employeeId);

                var newData = new
                {
                    EmployeePersonalInfo = personalData,
                    EmployeeContact = employeeContactData
                };
                return new APIResponse { isSuccess = true, Data = newData, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

    }
}
