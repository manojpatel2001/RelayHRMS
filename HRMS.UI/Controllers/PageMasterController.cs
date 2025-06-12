using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Controllers
{
    public class PageMasterController : Controller
    {
        public IActionResult PageMaster()
        {
            return View();
        }
    }
}
