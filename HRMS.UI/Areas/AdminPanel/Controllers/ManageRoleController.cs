using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ManageRoleController : Controller
    {
        public IActionResult ManageRole()
        {
            return View();
        }
    }
}
