using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class PrivilegeMasterController : Controller
    {
        public IActionResult Privileges()
        {
            return View();
        }
    }
}
