using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class LeftEmployeeController : Controller
    {
        public IActionResult LeftEmployeeDetails()
        {
            return View();
        }
    }
}
