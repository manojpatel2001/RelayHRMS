using HRMS_Core.DbContext;
using HRMS_Core.ManagePermission;
using HRMS_Core.VM;
using HRMS_Core.VM.Authentication;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Repository;
using HRMS_Utility;
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

        public AuthenticationAPIController(IUnitOfWork unitOfWork,UserManager<HRMSUserIdentity> userManager, SignInManager<HRMSUserIdentity> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;

        }

        [HttpPost("Login")]
        public async Task<APIResponse> Login(vmLogin model)
        {
            // Check if the model state is valid
            if (model==null)
            {
                return new APIResponse { isSuccess = false ,ResponseMessage="Login details cannot be null"};
            }

            var SuperAdmin = await _unitOfWork.SuperAdminDetailsRepository.GetSuperAdminByCredentials(model);
            var login = await _unitOfWork.EmployeeManageRepository.GetAsync(asp => asp.Email == model.Email && asp.IsEnabled == true);

            if (SuperAdmin != null)
            {
                var company= await _unitOfWork.CompanyDetailsRepository.GetAllCompanyDetailsList();

                var userDetails = new UserDetailsDto
                {
                    Id = SuperAdmin.Id,
                    Email = SuperAdmin.Email,
                    FullName = SuperAdmin?.FullName,
                    Designation = "Super Admin",
                    BranchId=0,
                    ProfileUrl = SuperAdmin?.ProfileImageUrl,
                    Password = SuperAdmin?.Password,
                    RoleName = "Super Admin",
                    RoleSlug = "super-admin",
                    //Permissions = new List<string> { "all-admin" },
                    Company = JsonSerializer.Serialize(company),
                    IsPasswordChange=true
                };

                // Generate JWT token
                var token = GenerateJwtToken(userDetails);


                var lastlogin = await _unitOfWork.EmployeeManageRepository.UpdateLastLogin(SuperAdmin.Id, 0);


                // Return the token
                return new APIResponse { isSuccess = true,Data=new {Token=token }, ResponseMessage = "Login Successfully!" };

            }
            else
            {
                //// Find the user by email
                //var user = await _userManager.FindByEmailAsync(model.Email);
                //if (user == null)
                //{
                //    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid email or password." };
                //}

                // Check if the password is correct
                //var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                var user = await _unitOfWork.EmployeeManageRepository.UserLogin(model);
                if (user == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid email or password." };
                }
                var emp_company = await _unitOfWork.UserCompanyPermissionsRepository.GetCompanyPermissionsListByEmployeeId((int)user.Id);
                //var DesignationDetails = await _unitOfWork.DesignationRepository.GetAsync(x=>x.DesignationId== user.DesignationId) ;
               
                ////var employeeDetails= await _unitOfWork.EmployeeManageRepository.GetEmployeeById((int)user.Id);

                var userDetails = new UserDetailsDto
                {
                    Id = (int)user.Id,
                    Email = user.Email,
                    FullName=user?.FullName,
                    Designation= user?.DesignationName,
                    BranchId = user?.BranchId,
                    ProfileUrl =user?.EmployeeProfileUrl,
                    Password = user?.Password,
                    RoleName = user?.RoleName,
                    RoleSlug = user?.RoleSlug,
                    //Permissions = permssions,
                    Company = JsonSerializer.Serialize(emp_company),
                    IsPasswordChange= user?.IsPasswordChange
                };

                // Generate JWT token
                var token = GenerateJwtToken(userDetails);

                var lastlogin = await _unitOfWork.EmployeeManageRepository.UpdateLastLogin(login.Id, login.CompanyId.Value);

                // Return the token
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
                new Claim("RoleSlug", user.RoleSlug.ToString()),
                new Claim(ClaimTypes.Role, user.RoleName ?? ""),
                new Claim("Company", user.Company ?? ""),
                new Claim("FullName", user.FullName ?? ""),
                new Claim("Designation", user.Designation ?? ""),
                new Claim("ProfileUrl", user.ProfileUrl ?? ""),
                new Claim("IsPasswordChange", user.IsPasswordChange.ToString() ?? ""),
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
    }
}
