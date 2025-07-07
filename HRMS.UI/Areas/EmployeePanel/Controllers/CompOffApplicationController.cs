using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
    [Area("EmployeePanel")]
    public class CompOffApplicationController : Controller
    {
        private readonly IConfiguration _configuration;


        public CompOffApplicationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];
            ViewBag.BaseUrlAPI = _configuration["BaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult AddCompOffApplication()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];
            ViewBag.BaseUrlAPI = _configuration["BaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult CompOffApproval()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];
            ViewBag.BaseUrlAPI = _configuration["BaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult PreCompOffApplication()
        {
            ViewBag.BaseUrl = _configuration["BaseUrlSettings:baseUrl"];
            ViewBag.BaseUrlAPI = _configuration["BaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult AddPreCompOffApplication()
        {
            ViewBag.BaseUrl = _configuration["BaseUrlSettings:baseUrl"];
            ViewBag.BaseUrlAPI = _configuration["BaseUrlSettings:baseUrl"];

            return View();
        }

        public IActionResult PreCompOffApproval()
        {
            ViewBag.BaseUrl = _configuration["BaseUrlSettings:baseUrl"];

            return View();
        }
    }
}
