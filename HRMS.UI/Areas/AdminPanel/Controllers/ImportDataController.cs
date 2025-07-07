using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ImportDataController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ImportData()
        {
            return View();
        }
        public IActionResult AddAllowance()
        {
            return View();
        }
    }
}
