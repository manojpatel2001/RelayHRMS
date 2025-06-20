using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
    [Area("EmployeePanel")]
    public class MyReportsController : Controller
    {
        private readonly IConfiguration _configuration;

        public MyReportsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }

        public IActionResult EmployeeInOutPresent()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult LateEarlySummary()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }

        public IActionResult MobileInOutSummary()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult AttendanceRegister()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult InOutSummary()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }

        public IActionResult EmployeeInOutPresentAttendance()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }

        public IActionResult LateEarlySummaryAttendance()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult MobileInOutSummaryAttendance()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }
    }
}
