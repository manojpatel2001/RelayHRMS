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
        public IActionResult AttendanceRegisterAdmin()
        {
            return View();
        }
        public IActionResult  SalaryRegisterAdmin()
        {
            return View(); 
        }
        public IActionResult  LeaveBalanceAdmin()
        {
            return View(); 
        }
        public IActionResult  TodaysAttendanceForAdmin()
        {
            return View(); 
        }
      
    }
}
