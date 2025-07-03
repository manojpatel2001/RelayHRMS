using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class GradeAllowanceController : Controller
    {
        public IActionResult GradeAllowance()
        {
            return View();
        }

    }
}
