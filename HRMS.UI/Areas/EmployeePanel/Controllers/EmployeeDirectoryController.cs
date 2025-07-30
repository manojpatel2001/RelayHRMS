using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
    [Area("EmployeePanel")]
    public class EmployeeDirectoryController : Controller
    {
        private readonly IConfiguration _configuration;

        public EmployeeDirectoryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];
            ViewBag.BaseUrlAPI = _configuration["BaseUrlSettings:baseUrl"];

            return View();
        }



    }
}
