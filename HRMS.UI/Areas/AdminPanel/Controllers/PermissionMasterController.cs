using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class PermissionMasterController : Controller
    {
        public IActionResult PermissionMaster()
        {
            return View();
        }
    }
}
