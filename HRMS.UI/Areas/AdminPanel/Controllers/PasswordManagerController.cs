using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    public class PasswordManagerController : Controller
    {
        [Area("AdminPanel")]
        public IActionResult ResetPassword()
        {
            return View();
        }
    }
}
