using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ManageRolePermission : Controller
    {
        public IActionResult RolePermission()
        {
            return View();
        }
    }
}
