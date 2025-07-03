using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class IncrementController : Controller
    {
        public IActionResult EmployeeIncrement()
        {
            return View();
        }

        public IActionResult AddIncrement()
        {
            return View();
        }
    }
}
