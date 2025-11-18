using HRMS_API.NotificationService.HubService;
using HRMS_API.NotificationService.ManageService;
using HRMS_API.Services;
using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.Leave;
using HRMS_Core.Notifications;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyInformation;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Repository;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRMS_API.Controllers.EmployeeMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeMasterAPIController : ControllerBase
    {
        private readonly UserManager<HRMSUserIdentity> _userManager;
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly FileUploadService _fileUploadService;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;


        public EmployeeMasterAPIController(IUnitOfWork unitOfWork, UserManager<HRMSUserIdentity> userManager, IConfiguration configuration, FileUploadService fileUploadService, IHubContext<NotificationRemainderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _configuration = configuration;
            _fileUploadService = fileUploadService;
            _hubContext = hubContext;
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
        [HttpGet("GetAllEmployeeByBranch/{companyId}")]
        public async Task<APIResponse> GetAllEmployeeByBranch(int companyId, [FromQuery] string BranchId)
        {
            try
            {
                // Split comma-separated BranchIds if multiple branches are sent
                var branchIds = string.IsNullOrEmpty(BranchId) ? new List<string>()
                               : BranchId.Split(',').ToList();

                var data = await _unitOfWork.EmployeeManageRepository.GetAllEmployeeByBranch(companyId, BranchId);

                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }
        [HttpGet("GetAllEmployee_DropDown/{companyId}")]
        public async Task<APIResponse> GetAllEmployee_DropDown(int companyId, [FromQuery] string BranchId, int Month, int Year)
        {
            try
            {
                // Split comma-separated BranchIds if multiple branches are sent
                var branchIds = string.IsNullOrEmpty(BranchId) ? new List<string>()
                               : BranchId.Split(',').ToList();

                var data = await _unitOfWork.EmployeeManageRepository.GetAllEmployee_DropDown(companyId, BranchId, Month, Year);

                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        //[HttpGet("GetAllEmployees")]
        //public async Task<APIResponse> GetAllEmployees()
        //{
        //    try
        //    {
        //        var data = await _unitOfWork.EmployeeManageRepository.GetAllAsync();
        //        if (data == null || !data.Any())
        //            return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

        //        return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
        //    }
        //}

        [HttpGet("GetAllEmployeeForUpdate")]
        public async Task<APIResponse> GetAllEmployeeForUpdate(int companyId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetAllEmployeeForUpdate(companyId);
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
                
                return new APIResponse { isSuccess = true, Data = nextEmployeeCode, ResponseMessage = "Records fetched successfully." };
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

                var existingUserByEmployeeCode = await _unitOfWork.EmployeeManageRepository.GetAllAsync(u => u.CompanyId == employeeData.CompanyId && u.EmployeeCode == employeeData.EmployeeCode);

                if (existingUserByEmployeeCode.Any())
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "An employee with the same Employee Code already exists." };
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
                    AlfaCode=employeeData.AlfaCode,
                    AlfaEmployeeCode=employeeData.AlfaEmployeeCode,
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
                    ReportingManagerId = employeeData.ReportingManagerId,
                    SubBranch = employeeData.SubBranch,
                    EnrollNo = employeeData.EnrollNo,
                    CompanyId = employeeData.CompanyId,
                    Pt = employeeData.Pt,
                    WeekOffDetailsId=(int)employeeData.WeekOffDetailsId,
                    IsPermissionPunchInOut =employeeData.IsPermissionPunchInOut,
                    IsLeft=employeeData.IsLeft,
                    IsPFApplicable=employeeData.IsPFApplicable,
                    Probation = employeeData.Probation
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

                    var salary = await _unitOfWork.EmployeeSalaryAllowanceRepository.CreateEmployeeSalaryAllowance(new vmEmployeeSalary { EmployeeId= employee.Id,CompanyId= employee.CompanyId, GrossSalary=employee.GrossSalary, BasicSalary = employeeData.BasicSalary, IsPFApplicable = employeeData.IsPFApplicable });

                    var getRole = await _unitOfWork.RoleRepository.GetAsync(x => x.Id == (int)employeeData.RoleId && x.IsDeleted == false && x.IsEnabled == true);
                    var companyPermission = new VMUserCompanyPermission
                    {
                        EmployeeId = employee.Id,
                        CompanyId = employee.CompanyId,
                        IsAdmin = getRole.Slug.ToString().ToLower() == "admin" ? true :false
                    };
                    var assignCompany=await _unitOfWork.UserCompanyPermissionsRepository.CreateUserCompanyPermissions(companyPermission);

                    var history = new PasswordHistory
                    {
                        EMPID = employee.Id,
                        NewPassword = employee.Password,
                        CreatedBy = employee.Id.ToString()
                    };
                    var CreateHistory = await _unitOfWork.PasswordHistory.CreateHistoryPassword(history);

                    var reporting = await _unitOfWork.ReportingManagerDetailsRepository.CreateReportingManagerDetail(new ReportingManagerDetails { EffectedDate=DateTime.UtcNow,EmployeeId= employee .Id,ReportingManagerId=(int) employee.ReportingManagerId,MethodName= "In Person" });
                 
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
        public async Task<APIResponse> UpdateEmployee(vmUpdateEmployee employeeData)
        {
            try
            {
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
                var existingUserByEmployeeCode = await _unitOfWork.EmployeeManageRepository.GetAllAsync(u =>u.CompanyId==employeeData.CompanyId&& u.EmployeeCode == employeeData.EmployeeCode && u.Id != existingUser.Id);
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

                //// Reset password if a new password is provided
                //if (!string.IsNullOrEmpty(employeeData.Password))
                //{

                //    var token = await _userManager.GeneratePasswordResetTokenAsync(oldUser);
                //    var resetPasswordResult = await _userManager.ResetPasswordAsync(oldUser, token, employeeData.Password);
                //    if (!resetPasswordResult.Succeeded)
                //    {
                //        return new APIResponse { isSuccess = false, ResponseMessage = "Failed to reset password." };
                //    }
                //}
                
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
                    var checkExistSalary = await _unitOfWork.EmployeeSalaryAllowanceRepository.GetEmployeeSalaryAllowanceByEmployeeId((int)employeeData.Id);
                    if (checkExistSalary == null)
                    {
                        var newsalary = await _unitOfWork.EmployeeSalaryAllowanceRepository.CreateEmployeeSalaryAllowance(new vmEmployeeSalary { EmployeeId = employeeData.Id, CompanyId = employeeData.CompanyId, GrossSalary = employeeData.GrossSalary, BasicSalary=employeeData.BasicSalary, IsPFApplicable= employeeData.IsPFApplicable });

                    }
                    else
                    {
                        var updateSalary = await _unitOfWork.EmployeeSalaryAllowanceRepository.UpdateEmployeeSalaryAllowance(new vmEmployeeSalary { EmployeeId = employeeData.Id, CompanyId = employeeData.CompanyId, GrossSalary = employeeData.GrossSalary, BasicSalary = employeeData.BasicSalary, IsPFApplicable = employeeData.IsPFApplicable });

                    }

                    if ((existingUser.ProbationCompletionPeriod == 0 || existingUser.ProbationCompletionPeriod == null) && employeeData.ProbationCompletionPeriod != 0)
                    {
                        //Notification send to reporting persion
                        if (employeeData.ManagerProbationId != null)
                        {
                            var notification = new NotificationRemainders()
                            {
                                NotificationMessage = $"Employee {employeeData.FullName} is in probation from Now.",
                                NotificationTime = DateTime.UtcNow,
                                SenderId = employeeData.Id.ToString(),
                                ReceiverIds = employeeData.ManagerProbationId.ToString(),
                                NotificationType = NotificationType.ProbationOver,
                                NotificationAffectedId = employeeData.Id
                            };
                            var savedNotification = await _unitOfWork.NotificationRemainderRepository.CreateNotificationRemainder(notification);
                            if (savedNotification.Success > 0)
                            {
                                notification.NotificationRemainderId = savedNotification.Success;
                                var reprtingConnection = NotificationRemainderConnectionManager.GetConnections(employeeData.ManagerProbationId.ToString());
                                if (reprtingConnection.Any())
                                {
                                    await _hubContext.Clients.Clients(reprtingConnection).SendAsync("ReceiveNotificationRemainder", notification);
                                }
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

        [HttpPost("UpdateBasicInfo")]
        public async Task<APIResponse> UpdateBasicInfo(vmUpdateEmployee employeeData)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.UpdateBasicInfo(employeeData);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update data. Please try again later." };
            }
        }
        [HttpPost("UpdatePersonalInfo")]
        public async Task<APIResponse> UpdatePersonalInfo(vmUpdateEmployee employeeData)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.UpdatePersonalInfo(employeeData);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update data. Please try again later." };
            }
        }
        [HttpPost("UpdateContactInfo")]
        public async Task<APIResponse> UpdateContactInfo(vmUpdateEmployee employeeData)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.UpdateContactInfo(employeeData);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update data. Please try again later." };
            }
        }

        [HttpPost("UpdateSalaryInfo")]
        public async Task<APIResponse> UpdateSalaryInfo(vmUpdateEmployee employeeData)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.UpdateSalaryInfo(employeeData);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update data. Please try again later." };
            }
        }

        [HttpDelete("DeleteEmployee")]
        public async Task<APIResponse> DeleteEmployee(DeleteRecordVM deleteRecord)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.DeleteEmployee(deleteRecord);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update data. Please try again later." };
            }
        }

        [HttpGet("GetBasicInfoById/{EmployeeId}")]
        public async Task<APIResponse> GetBasicInfoById(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetUpdateEmployeeById(EmployeeId, "BASIC");

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update data. Please try again later." };
            }
        }
        [HttpGet("GetPersonalInfoById/{EmployeeId}")]
        public async Task<APIResponse> GetPersonalInfoById(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetUpdateEmployeeById(EmployeeId, "PERSONAL");

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update data. Please try again later." };
            }
        }
        [HttpGet("GetContactInfoById/{EmployeeId}")]
        public async Task<APIResponse> GetContactInfoById(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetUpdateEmployeeById(EmployeeId, "CONTACT");

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update data. Please try again later." };
            }
        }
        [HttpGet("GetSalaryInfoById/{EmployeeId}")]
        public async Task<APIResponse> GetSalaryInfoById(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetUpdateEmployeeById(EmployeeId, "SALARY");

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update data. Please try again later." };
            }
        }



        //[HttpDelete("DeleteEmployee")]
        //public async Task<APIResponse> DeleteEmployee(DeleteRecordVM model)
        //{
        //    try
        //    {
        //        if (model == null || model.emp_id == null)
        //            return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };
        //        var existingUser = await _unitOfWork.EmployeeManageRepository.GetAsync(x => x.Id == model.emp_id && x.IsDeleted == false && x.IsEnabled == true);
        //        if (existingUser == null)
        //        {
        //            return new APIResponse { isSuccess = false, ResponseMessage = "Employee not found." };
        //        }
        //        model.DeletedDate = DateTime.UtcNow;
        //        var result = await _unitOfWork.EmployeeManageRepository.DeleteEmployee(model);

        //        if (result.Id !=null)
        //            return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

        //        return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete city. Please try again later." };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
        //    }
        //}

        [HttpGet("GetAllEmployeeActiveOrLeft/{IsLeft}")]
        public async Task<APIResponse> GetAllEmployeeActiveOrLeft( bool IsLeft, int companyId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetAllEmployeeActiveOrLeft(IsLeft, companyId);
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
        public async Task<APIResponse> GetEmployeeById(int employeeId)
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

        [HttpGet("GetEmployeeByBranchId/{BranchId}")]
        public async Task<APIResponse> GetEmployeeByBranchId(int BranchId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetAllAsync(x => x.BranchId == BranchId && x.IsDeleted == false && x.IsEnabled == true);

                if (data == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Record not found"
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Record fetched successfully"
                };
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
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid  record." };

               
                if (model.EmployeeProfileFile != null )
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
                    else
                    {
                        model.EmployeeProfileUrl = null;
                    }
                }
                else
                {
                    model.EmployeeProfileUrl = null;
                }

                if (model.EmployeeSignatureFile != null)
                {
                    if (model.EmployeeSignatureFile.Length > 0)
                    {
                        var folder = $"uploads/employeesignature";
                        var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync( model.EmployeeSignatureFile, folder, check.EmployeeSignatureUrl);
                        if (string.IsNullOrEmpty(fileUrl))
                        {
                            return new APIResponse { isSuccess = false, ResponseMessage = "Some thing went wrong. Please try again later." };

                        }
                        model.EmployeeSignatureUrl = fileUrl;
                    }
                    else
                    {
                        model.EmployeeSignatureUrl = null;
                    }
                }
                else
                {
                    model.EmployeeSignatureUrl = null;
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
        [HttpPost("GetExistEmployeeCode")]
        public async Task<APIResponse> GetExistEmployeeCode(vmCommonParameters vmCommonParameters)
        {
            try
            {
                var existEmployeeCode = await _unitOfWork.EmployeeManageRepository.GetExistEmployeeCode(vmCommonParameters);

                return new APIResponse { isSuccess = true, Data = existEmployeeCode, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }




        [HttpGet("GetAllEmployeeByBranchId")]
        public async Task<APIResponse> GetAllEmployeeByBranchId([FromQuery] string? BranchIds,int CompanyId)
        {
            try
            {
                var data = await _unitOfWork.MonthlySalaryDetailsRepository.GetEmployeesByBranchId(BranchIds,CompanyId);
                return new APIResponse() { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrieve records, Please try again later!"
                };
            }
        }



        [HttpGet("EmployeePersonalInformation")]
        public async Task<APIResponse> EmployeePersonalInformation([FromQuery]int empid,[FromQuery] int Compid)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.EmployeePersonalInformation(empid,Compid);
                return new APIResponse() { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrieve records, Please try again later!"
                };
            }
        }

        [HttpGet("GetAllReportingPersons")]
        public async Task<APIResponse> GetAllReportingPersons()
        {
            try
            {
                var data = await _unitOfWork.EmployeePersonalInfoRepository.GetAllReportingPersons();
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpPost("GetRecordsForUpdate")]
        public async Task<APIResponse> GetRecordsForUpdate(CommonParameter param)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetRecordsForUpdate( param);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }
        [HttpGet("GetRecordsForAdd/{CompanyId}")]
        public async Task<APIResponse> GetRecordsForAdd(int CompanyId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetRecordsForAdd(CompanyId);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }
        [HttpGet("GetReportingList")]
        public async Task<APIResponse> GetReportingList()
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetReportingList();

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }

        [HttpGet("GetEmployeesListForSalary")]
        public async Task<APIResponse> GetEmployeesListForSalary([FromQuery] int month, [FromQuery] int year , [FromQuery] int compId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetEmployeesListForSalary(month, year,compId);
                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }


        [HttpPost("GetEmployeeListByBranchId")]
        public async Task<APIResponse> GetEmployeeListByBranchId(CommonParameter parameter)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetEmployeeListByBranchId(parameter);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }
        [HttpPost("GetEmployeeListByBranchIdForLeft")]
        public async Task<APIResponse> GetEmployeeListByBranchIdForLeft(CommonParameter parameter)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetEmployeeListByBranchIdForLeft(parameter);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }

        [HttpPost("GetGradeBySalaryRange")]
        public async Task<APIResponse> GetGradeBySalaryRange(vmEmployeeSalary param)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetGradeBySalaryRange(param);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }

        [HttpGet("GetEmplyeeDetailsById/{EmployeeId}")]
        public async Task<APIResponse> GetEmplyeeDetailsById(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetEmplyeeDetailsById(EmployeeId);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }

        [HttpGet("GetAllEmployeeList/{CompanyId}")]
        public async Task<APIResponse> GetAllEmployeeList(int CompanyId)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetAllEmployeeList(CompanyId);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }
    }
}
