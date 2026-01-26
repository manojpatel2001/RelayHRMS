using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
    [Area("EmployeePanel")]
    public class ExitApplicationEssController : Controller
    {
        public IActionResult ExitApplication()
        {
            return View();
         
        }
        public IActionResult ExitApplicationReport()
        {
            return View();
         
        }
        public IActionResult ExitApproval()
        {
            return View();
         
        }
    }
}
