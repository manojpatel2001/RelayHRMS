using HRMS_Core.DbContext;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_API.Controllers.EmployeeMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeMasterAPIController : ControllerBase
    {
        private readonly UserManager<HRMSUserIdentity> _userManager;

        public EmployeeMasterAPIController(UserManager<HRMSUserIdentity> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("CreateEmployee")]
        public async Task<APIResponse> CreateEmployee([FromBody] vmEmployeeData employeeData)
        {
            try
            {
                if (employeeData == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Employee details cannot be null" };
                }

                // Check for duplicate EmployeeStringCode and EmployeeNumberCode
                var existingUserByEmployeeCode = await _userManager.Users.FirstOrDefaultAsync(u =>
                    u.EmployeeNumberCode == employeeData.EmployeeNumberCode);

                if (existingUserByEmployeeCode != null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "An employee with the same Employee Code already exists." };
                }

                // Check for duplicate LoginAlias
                var existingUserByLoginAlias = await _userManager.FindByNameAsync(employeeData.LoginAlias);
                if (existingUserByLoginAlias != null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "An employee with the same Login Alias already exists." };
                }

                var user = new HRMSUserIdentity
                {
                    UserName = employeeData.LoginAlias,
                    Email = employeeData.LoginAlias,//$"{employeeData.LoginAlias}@example.com", // Example email
                    Initial = employeeData.Initial,
                    FirstName = employeeData.FirstName,
                    MiddleName = employeeData.MiddleName,
                    LastName = employeeData.LastName,
                    EmployeeStringCode = employeeData.EmployeeStringCode,
                    EmployeeNumberCode = employeeData.EmployeeNumberCode,
                    DateOfJoining = employeeData.DateOfJoining,
                    Branch = employeeData.Branch,
                    Grade = employeeData.Grade,
                    Shift = employeeData.Shift,
                    CTC = employeeData.CTC,
                    Designation = employeeData.Designation,
                    GrossSalary = employeeData.GrossSalary,
                    Category = employeeData.Category,
                    BasicSalary = employeeData.BasicSalary,
                    Department = employeeData.Department,
                    EmployeeType = employeeData.EmployeeType,
                    DateOfBirth = employeeData.DateOfBirth,
                    UserPrivilege = employeeData.UserPrivilege,
                    LoginAlias = employeeData.LoginAlias,
                    Overtime = employeeData.Overtime,
                    Latemark = employeeData.Latemark,
                    Earlymark = employeeData.Earlymark,
                    Fullpf = employeeData.Fullpf,
                    Pt = employeeData.Pt,
                    Fixsalary = employeeData.Fixsalary,
                    Probation = employeeData.Probation,
                    Trainee = employeeData.Trainee
                };

                var result = await _userManager.CreateAsync(user, "DefaultPassword123!"); // Set a default password

                if (result.Succeeded)
                {
                    return new APIResponse { isSuccess = true, Data = user, ResponseMessage = "The record has been saved successfully" };
                }
                else
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = string.Join(", ", result.Errors.Select(e => e.Description)) };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record, Please try again later!" };
            }
        }
    }
}
