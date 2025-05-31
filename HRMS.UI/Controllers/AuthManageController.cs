using HRMS_Core.VM;
using HRMS_Core.VM.Authentication;
using HRMS_Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

public class AuthManageController : Controller
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;

    public AuthManageController(IHttpClientFactory clientFactory, IConfiguration configuration)
    {
        _clientFactory = clientFactory;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<APIResponse> Login([FromBody]vmLogin model)
    {
        if (!ModelState.IsValid)
        {
            return new APIResponse {isSuccess=false,ResponseMessage="Login details cannot be null" };
        }
        if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
        {
            return new APIResponse { isSuccess = false, ResponseMessage = "Email or password cannot be null" };

        }
        if (model.Email.ToLower()=="admin@demo.com" && model.Password== "admin")
        {
            var tokenResponse = new TokenResponse { Email=model.Email.ToLower() };
            return new APIResponse { isSuccess = true, Data = tokenResponse, ResponseMessage = "Login successfully!" };
        }
        var client = _clientFactory.CreateClient();
        var apiBaseUrl = _configuration["BaseUrlSettings:baseUrl"]; // Access the base URL from configuration
        var response = await client.PostAsJsonAsync($"{apiBaseUrl}/AuthenticationAPI/Login", model);

        if (response.IsSuccessStatusCode)
        {
            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, tokenResponse.Email),
                new Claim(ClaimTypes.Email, tokenResponse.Email),
                new Claim("Token", tokenResponse.Token)
               
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return new APIResponse { isSuccess = true,Data= tokenResponse, ResponseMessage = "Login successfully!" };
        }
        else
        {
            return new APIResponse { isSuccess = false, ResponseMessage = "Invalid login attempt!" };

        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}

