using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ExitApplicationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
