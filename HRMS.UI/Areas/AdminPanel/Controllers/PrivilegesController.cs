using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class PrivilegesController : Controller
    {
        public IActionResult EmployeePrivileges()
        {
            return View();
        }
    }
}
