using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
	[Area("EmployeePanel")]
	public class LeaveController : Controller
	{
		private readonly IConfiguration _configuration;


		public LeaveController(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult LeaveApplication()
		{
			ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];
            ViewBag.BaseUrlAPI = _configuration["BaseUrlSettings:baseUrl"];


            return View();


		}
        public IActionResult LeaveApproval()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];
            ViewBag.BaseUrlAPI = _configuration["BaseUrlSettings:baseUrl"];


            return View();


        }
        public IActionResult AddLeave()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];
            ViewBag.BaseUrlAPI = _configuration["BaseUrlSettings:baseUrl"];


            return View();
        }

        public IActionResult AdminLeaveApproval()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];
            ViewBag.BaseUrlAPI = _configuration["BaseUrlSettings:baseUrl"];


            return View();
        }
        public IActionResult Attendance()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];
            ViewBag.BaseUrlAPI = _configuration["BaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult AddAttendance()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];
            ViewBag.BaseUrlAPI = _configuration["BaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult AttendanceRegularizationApproval()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];
            ViewBag.BaseUrlAPI = _configuration["BaseUrlSettings:baseUrl"];
            return View();
        }
    }
}
