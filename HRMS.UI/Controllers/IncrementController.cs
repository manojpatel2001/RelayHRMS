using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Controllers
{
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
