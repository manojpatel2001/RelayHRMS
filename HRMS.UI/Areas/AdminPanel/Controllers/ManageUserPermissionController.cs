using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ManageUserPermissionController : Controller
    {
        public IActionResult UserPermission()
        {
            return View();
        }
    }
}
