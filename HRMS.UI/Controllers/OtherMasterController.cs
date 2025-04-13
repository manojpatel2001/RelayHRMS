using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Controllers
{
    public class OtherMasterController : Controller
    {
        public IActionResult OrganizationPolicy()
        {
            return View();
        }

        public IActionResult CEOLocationMasterAssign()
        {
            return View();
        }

        public IActionResult TicketType()
        {
            return View();
        }
    }
}
