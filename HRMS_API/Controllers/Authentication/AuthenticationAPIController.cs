using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.ManagePermission;
using HRMS_Core.VM;
using HRMS_Core.VM.Authentication;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Repository;
using HRMS_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace HRMS_API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly UserManager<HRMSUserIdentity> _userManager;
        private readonly SignInManager<HRMSUserIdentity> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthenticationAPIController(IUnitOfWork unitOfWork, UserManager<HRMSUserIdentity> userManager, SignInManager<HRMSUserIdentity> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;

        }

        [HttpPost("Login")]
        public async Task<APIResponse> Login(vmLogin model)
        {
            if (model == null)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Login details cannot be null" };
            }

            // ✅ STEP 5 — ye 4 lines add karo (model null check ke baad)
            string ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                                ?? HttpContext.Connection.RemoteIpAddress?.ToString();
            string userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            string deviceType = GetDeviceType(userAgent);
            //string sessionId = HttpContext.Session.Id;
            int sessionId = 10;

            var SuperAdmin = await _unitOfWork.SuperAdminDetailsRepository.GetSuperAdminByCredentials(model);

            if (SuperAdmin != null)
            {
                var company = await _unitOfWork.CompanyDetailsRepository.GetAllCompanyDetailsList();

                var userDetails = new UserDetailsDto
                {
                    Id = SuperAdmin.Id,
                    Email = SuperAdmin.Email,
                    FullName = SuperAdmin?.FullName,
                    EmployeeCode = "0",
                    Designation = "Super Admin",
                    BranchId = 0,
                    ProfileUrl = SuperAdmin?.ProfileImageUrl,
                    Password = SuperAdmin?.Password,
                    RoleName = "Super Admin",
                    RoleSlug = "super-admin",
                    Company = JsonSerializer.Serialize(company),
                    IsPasswordChange = true
                };

                // ✅ STEP 5 — ye 6 lines add karo (GenerateJwtToken se pehle)
                int loginHistoryId = await _unitOfWork.SuperAdminDetailsRepository.InsertLoginHistory(new LoginHistory
                {
                    EmpID = SuperAdmin.Id,
                    IPAddress = ip,
                    BrowserInfo = userAgent,
                    DeviceType = deviceType,
                    LoginStatus = true,
                    SessionID = sessionId.ToString()
                });
                userDetails.LoginHistoryID = loginHistoryId;

                var token = GenerateJwtToken(userDetails); // existing line

                var lastlogin = await _unitOfWork.EmployeeManageRepository.UpdateLastLogin(SuperAdmin.Id, 0); // existing line

                return new APIResponse { isSuccess = true, Data = new { Token = token }, ResponseMessage = "Login Successfully!" };
            }
            else
            {
                var user = await _unitOfWork.EmployeeManageRepository.UserLogin(model);

                if (user == null)
                {
                    // ✅ STEP 5 — ye block add karo (return se pehle)
                    await _unitOfWork.SuperAdminDetailsRepository.InsertLoginHistory(new LoginHistory
                    {
                        IPAddress = ip,
                        BrowserInfo = userAgent,
                        DeviceType = deviceType,
                        LoginStatus = false,
                        FailureReason = "Invalid email or password",
                        SessionID = sessionId.ToString()
                    });

                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid email or password." }; // existing line
                }

                var emp_company = await _unitOfWork.UserCompanyPermissionsRepository.GetCompanyPermissionsListByEmployeeId((int)user.Id); // existing line

                var userDetails = new UserDetailsDto
                {
                    Id = (int)user.Id,
                    Email = user.Email,
                    FullName = user?.FullName,
                    EmployeeCode = user?.EmployeeCode,
                    Designation = user?.DesignationName,
                    BranchId = user?.BranchId,
                    ProfileUrl = user?.EmployeeProfileUrl,
                    Password = user?.Password,
                    RoleName = user?.RoleName,
                    RoleSlug = user?.RoleSlug,
                    Company = JsonSerializer.Serialize(emp_company),
                    IsPasswordChange = user?.IsPasswordChange
                };

                // ✅ STEP 5 — ye 6 lines add karo (GenerateJwtToken se pehle)
                int loginHistoryId = await _unitOfWork.SuperAdminDetailsRepository.InsertLoginHistory(new LoginHistory
                {
                    EmpID = (int)user.Id,
                    IPAddress = ip,
                    BrowserInfo = userAgent,
                    DeviceType = deviceType,
                    LoginStatus = true,
                    SessionID = sessionId.ToString()
                });
                userDetails.LoginHistoryID = loginHistoryId;

                var token = GenerateJwtToken(userDetails); // existing line

                var lastlogin = await _unitOfWork.EmployeeManageRepository.UpdateLastLogin((int)user.Id, user.CompanyId.Value); // existing line

                return new APIResponse { isSuccess = true, Data = new { Token = token }, ResponseMessage = "Login Successfully!" };
            }
        }

        private string GenerateJwtToken(UserDetailsDto user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Id", user.Id.ToString()),
                new Claim("Password", user.Password.ToString()),
                new Claim("RoleSlug", user?.RoleSlug.ToString()),
                new Claim(ClaimTypes.Role, user?.RoleName ?? ""),
                new Claim("Company", user?.Company ?? ""),
                new Claim("FullName", user?.FullName ?? ""),
                new Claim("EmployeeCode", user?.EmployeeCode ?? ""),
                new Claim("Designation", user?.Designation ?? ""),
                new Claim("ProfileUrl", user?.ProfileUrl ?? ""),
                new Claim("IsPasswordChange", user?.IsPasswordChange.ToString() ?? ""),
                new Claim("LoginHistoryID", user.LoginHistoryID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //// ✅ Add multiple Permission claims (array-style)
            //if (user.Permissions.Any()||user.Permissions!=null)
            //{
            //    claims.AddRange(user.Permissions.Select(p => new Claim("Permission", p)));
            //}

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<APIResponse> Logout()
        {
            var claim = User.FindFirst("LoginHistoryID")?.Value;

            if (!string.IsNullOrEmpty(claim) && int.TryParse(claim, out int loginHistoryId) && loginHistoryId > 0)
            {
                await _unitOfWork.SuperAdminDetailsRepository.UpdateLogoutTime(loginHistoryId);
            }

            return new APIResponse { isSuccess = true, ResponseMessage = "Logged out successfully." };
        }
        private string GetDeviceType(string userAgent)
        {
            userAgent = userAgent.ToLower();
            if (userAgent.Contains("mobile")) return "Mobile";
            if (userAgent.Contains("tablet") || userAgent.Contains("ipad")) return "Tablet";
            return "Desktop";
        }
    }
}
