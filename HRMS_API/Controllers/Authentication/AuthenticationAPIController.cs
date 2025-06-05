using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRMS_API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationAPIController : ControllerBase
    {
        private readonly UserManager<HRMSUserIdentity> _userManager;
        private readonly SignInManager<HRMSUserIdentity> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthenticationAPIController(UserManager<HRMSUserIdentity> userManager, SignInManager<HRMSUserIdentity> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] vmLogin model)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Check if the password is correct
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            // Return the token
            return Ok(new TokenResponse
            {
                Token = token,
                Id = user.Id,
                Email = user.Email
            });
        }

        private string GenerateJwtToken(HRMSUserIdentity user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            // Add additional claims as needed
        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token expiration time
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
