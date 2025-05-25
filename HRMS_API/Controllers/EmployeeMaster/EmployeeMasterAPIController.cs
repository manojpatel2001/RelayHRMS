using HRMS_Core.DbContext;
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
                    employeeData.Password = "Employee@1";
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
                    Shift = employeeData.Shift,
                    CTC = employeeData.CTC,
                    DesignationId = employeeData.DesignationId,
                    GrossSalary = employeeData.GrossSalary,
                    Category = employeeData.Category,
                    BasicSalary = employeeData.BasicSalary,
                    DepartmentId = employeeData.DepartmentId,
                    EmployeeType = employeeData.EmployeeType,
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
        public async Task<APIResponse> UpdateEmployee(vmEmployeeData employeeData)
        {
            try
            {
                if (employeeData == null || string.IsNullOrEmpty(employeeData.LoginAlias))
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Employee details cannot be null" };
                }

                var existingUser = await _unitOfWork.EmployeeManageRepository.GetAsync(x=>x.Id==employeeData.Id && x.IsDeleted==false && x.IsEnabled==true);
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

                //// Update the user properties
                //existingUser.Id = employeeData.Id;
                //existingUser.Email = employeeData.LoginAlias;
                //existingUser.UserName = employeeData.LoginAlias;
                //if (!string.IsNullOrEmpty(employeeData.Password))
                //{
                //    existingUser.Password = employeeData.Password;
                //}
                //existingUser.Initial = employeeData.Initial;
                //existingUser.FirstName = employeeData.FirstName;
                //existingUser.MiddleName = employeeData.MiddleName;
                //existingUser.LastName = employeeData.LastName;
                //existingUser.FullName = employeeData.FullName;
                //existingUser.EmployeeCode = employeeData.EmployeeCode;
                //existingUser.DateOfJoining = employeeData.DateOfJoining;
                //existingUser.BranchId = employeeData.BranchId;
                //existingUser.GradeId = employeeData.GradeId;
                //existingUser.Shift = employeeData.Shift;
                //existingUser.CTC = employeeData.CTC;
                //existingUser.DesignationId = employeeData.DesignationId;
                //existingUser.GrossSalary = employeeData.GrossSalary;
                //existingUser.Category = employeeData.Category;
                //existingUser.BasicSalary = employeeData.BasicSalary;
                //existingUser.DepartmentId = employeeData.DepartmentId;
                //existingUser.EmployeeType = employeeData.EmployeeType;
                //existingUser.DateOfBirth = employeeData.DateOfBirth;
                //existingUser.UserPrivilege = employeeData.UserPrivilege;
                //existingUser.ReportingManager = employeeData.ReportingManager;
                //existingUser.SubBranch = employeeData.SubBranch;
                //existingUser.EnrollNo = employeeData.EnrollNo;
                //existingUser.CompanyId = employeeData.CompanyId;
                //existingUser.Overtime = employeeData.Overtime;
                //existingUser.Latemark = employeeData.Latemark;
                //existingUser.Earlymark = employeeData.Earlymark;
                //existingUser.Fullpf = employeeData.Fullpf;
                //existingUser.Pt = employeeData.Pt;
                //existingUser.Fixsalary = employeeData.Fixsalary;
                //existingUser.Probation = employeeData.Probation;
                //existingUser.Trainee = employeeData.Trainee;

                
                // Update the user
                var result = await _unitOfWork.EmployeeManageRepository.UpdateEmployee(employeeData);

                if (result.Emp_Id!=null)
                {
                    var updatedEmployee = await _unitOfWork.EmployeeManageRepository.GetEmployeeById(result.Emp_Id);
                    return new APIResponse { isSuccess = true, Data = updatedEmployee, ResponseMessage = "Employee has been updated successfully" };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update employee, Please try again later!" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update employee, Please try again later!" };
            }
        }



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

                if (result.Emp_Id !=null)
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

        [HttpPost("UpdateEmployeeProfileAndSignature")]
        public async Task<APIResponse> UpdateEmployeeProfileAndSignature(vmUpdateEmployeeProfile model)
        {
            try
            {
                if (model == null || model.EmployeeId == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Company details cannot be null." };
                var check = await _unitOfWork.EmployeeManageRepository.GetEmployeeById(model.EmployeeId);
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

                if (result.Emp_Id != null)
                {
                    var updatedProfile = await _unitOfWork.EmployeeManageRepository.GetEmployeeById(result.Emp_Id);
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
    }
}
