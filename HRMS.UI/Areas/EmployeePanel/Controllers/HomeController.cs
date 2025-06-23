using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
    [Area("EmployeePanel")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];
            return View();
        }
        public IActionResult EmployeeandTeam()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];
            return View();
        }
        public IActionResult Attendanceandleave()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];
            return View();
        }
        public IActionResult Salary()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];
            return View();
        }
        public IActionResult Reports()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];
            return View();
        }
    }

}
