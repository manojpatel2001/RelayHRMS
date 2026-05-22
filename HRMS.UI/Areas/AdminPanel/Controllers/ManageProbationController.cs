using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ManageProbationController : Controller
    {
        public IActionResult ProbationConfirmation()
        {
            return View();
        }
    }
}
