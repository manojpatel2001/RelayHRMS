using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
    [Area("EmployeePanel")]
    public class ExitApplicationEssController : Controller
    {
        public IActionResult ExitApplication()
        {
            return View();
         
        }
    }
}
