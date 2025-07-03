  using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class EmployeeIncrementController : Controller
    {
        public IActionResult EmployeeIncrementApplication()
        {
            return View();
        }

        public IActionResult AddEmployeeIncrement()
        {
            return View();
        }
    }
}
