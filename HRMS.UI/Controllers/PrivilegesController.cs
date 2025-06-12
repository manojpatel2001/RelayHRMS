using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Controllers
{
    public class PrivilegesController : Controller
    {
        public IActionResult EmployeePrivileges()
        {
            return View();
        }
    }
}
