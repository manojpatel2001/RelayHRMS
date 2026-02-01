using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SalaryDetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MonthlySalary()
        {
            return View();
        }
        public IActionResult AddLoanApplication()
        {
            return View();
        }
    }
}
