using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SchemeController : Controller
    {
        public IActionResult SchemeMaster()
        {
            return View();
        }
        public IActionResult SchemeDetails()
        {
            return View();
        }
    }
}
