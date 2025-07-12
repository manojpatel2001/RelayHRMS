using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ReportController : Controller
    {
        public IActionResult AttendanceReports()
        {
            return View();
        }
        public IActionResult AttendanceRegularization()
        {
            return View();
        }
        public IActionResult AddAttendanceRegularization()
        {
            return View();
        }
    }
}
