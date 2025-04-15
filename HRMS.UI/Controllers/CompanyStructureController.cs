using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Controllers
{
    public class CompanyStructureController : Controller
    {
        public IActionResult BankMaster()
        {
            return View();
        }

        public IActionResult WeekOffMaster()
        {
            return View();
        }
    }
}
