using HRMS_Core.DbContext;
using HRMS_Core.VM;
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

                // Update the user properties
                existingUser.Initial = employeeData.Initial;
                existingUser.FirstName = employeeData.FirstName;
                existingUser.MiddleName = employeeData.MiddleName;
                existingUser.LastName = employeeData.LastName;
                existingUser.FullName = employeeData.FullName;
                existingUser.EmployeeCode = employeeData.EmployeeCode;
                existingUser.DateOfJoining = employeeData.DateOfJoining;
                existingUser.BranchId = employeeData.BranchId;
                existingUser.GradeId = employeeData.GradeId;
                existingUser.Shift = employeeData.Shift;
                existingUser.CTC = employeeData.CTC;
                existingUser.DesignationId = employeeData.DesignationId;
                existingUser.GrossSalary = employeeData.GrossSalary;
                existingUser.Category = employeeData.Category;
                existingUser.BasicSalary = employeeData.BasicSalary;
                existingUser.DepartmentId = employeeData.DepartmentId;
                existingUser.EmployeeType = employeeData.EmployeeType;
                existingUser.DateOfBirth = employeeData.DateOfBirth;
                existingUser.UserPrivilege = employeeData.UserPrivilege;
                existingUser.ReportingManager = employeeData.ReportingManager;
                existingUser.SubBranch = employeeData.SubBranch;
                existingUser.EnrollNo = employeeData.EnrollNo;
                existingUser.CompanyId = employeeData.CompanyId;
                existingUser.Overtime = employeeData.Overtime;
                existingUser.Latemark = employeeData.Latemark;
                existingUser.Earlymark = employeeData.Earlymark;
                existingUser.Fullpf = employeeData.Fullpf;
                existingUser.Pt = employeeData.Pt;
                existingUser.Fixsalary = employeeData.Fixsalary;
                existingUser.Probation = employeeData.Probation;
                existingUser.Trainee = employeeData.Trainee;

                // Update email if it has changed
                if (existingUser.Email != employeeData.LoginAlias)
                {
                    var emailToken = await _userManager.GenerateChangeEmailTokenAsync(existingUser, employeeData.LoginAlias);
                    var emailResult = await _userManager.ChangeEmailAsync(existingUser, employeeData.LoginAlias, emailToken);
                    if (!emailResult.Succeeded)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update email, Please try again later!" };
                    }
                }

                // Update password if it has changed and is not empty, otherwise set a default password
                if (!string.IsNullOrEmpty(employeeData.Password))
                {
                    var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                    var passwordResult = await _userManager.ResetPasswordAsync(existingUser, passwordToken, employeeData.Password);
                    if (!passwordResult.Succeeded)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update password, Please try again later!" };
                    }
                }
                else
                {
                    // Set a default password if none is provided
                    var defaultPassword = "Employee@1";
                    var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                    var passwordResult = await _userManager.ResetPasswordAsync(existingUser, passwordToken, defaultPassword);
                    if (!passwordResult.Succeeded)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = "Unable to set default password, Please try again later!" };
                    }
                }

                // Update the user
                var result = await _userManager.UpdateAsync(existingUser);

                if (result.Succeeded)
                {
                    return new APIResponse { isSuccess = true, Data = existingUser, ResponseMessage = "Employee has been updated successfully" };
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
