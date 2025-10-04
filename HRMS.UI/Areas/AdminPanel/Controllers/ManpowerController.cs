using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ManpowerController : Controller
    {
        public IActionResult ManpowerRequisition()
        {
            return View();
        }
    }
}
