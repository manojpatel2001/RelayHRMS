using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class BulkIncrementController : Controller
    {
        public IActionResult BulkIncrement()
        {
            return View();
        }
    }
}
