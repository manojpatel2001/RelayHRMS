using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
    [Area("EmployeePanel")]
    public class ManageProbationController : Controller
    {
        public IActionResult Probation()
        {
            return View();
        }
        public IActionResult ProbationPerformance()
        {
            return View();
        }
    }
}
