using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
    [Area("EmployeePanel")]
    public class MyTeamController : Controller
    {
        private readonly IConfiguration _configuration;

        public MyTeamController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult ShiftChanges()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }

        public IActionResult AddShiftChanges()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult ShiftImport()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult AddShiftImport()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult MemberInOutRecord()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult MemberRoster()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult MemberDetails()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];
            return View();
        }


    }
}
