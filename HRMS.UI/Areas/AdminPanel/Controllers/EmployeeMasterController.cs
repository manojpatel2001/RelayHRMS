using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class EmployeeMasterController : Controller
    {
        public IActionResult EmployeeMaster()
        {
            return View();
        }

        public IActionResult AdditionalInformation()
        {
            return View();
        }
        public IActionResult EmployeeHistory()
        {
            return View();
        }
    }
}
