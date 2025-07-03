using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class PageMasterController : Controller
    {
        public IActionResult PageMaster()
        {
            return View();
        }
    }
}
