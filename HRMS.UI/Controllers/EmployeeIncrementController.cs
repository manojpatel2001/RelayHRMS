  using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Controllers
{
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
