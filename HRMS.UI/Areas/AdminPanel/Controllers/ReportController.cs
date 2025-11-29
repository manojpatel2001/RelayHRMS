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
        public IActionResult  ActiveInActiveUsers()
        {
            return View(); 
        }
        public IActionResult  ActiveInActiveMobileUsers()
        {
            return View(); 
        }
        public IActionResult  WhosOffReport()
        {
            return View(); 
        }
        public IActionResult  LeaveODReport()
        {
            return View(); 
        }
        public IActionResult  ProbationReport()
        {
            return View(); 
        }
        public IActionResult ContinousAbsentReport()
        {
            return View(); 
        }
        public IActionResult AttendanceRequestAdminReport()
        {
            return View(); 
        }
        public IActionResult LeaveYearlySummary()
        {
            return View(); 
        }
      
    }
}
