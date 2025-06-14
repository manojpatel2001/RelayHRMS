using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Controllers
{
    public class PrivilegeMasterController : Controller
    {
        public IActionResult Privileges()
        {
            return View();
        }
    }
}
