using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class OtherMasterController : Controller
    {
        public IActionResult OrganizationPolicy()
        {
            return View();
        }

        public IActionResult GeoLocationMasterAssign()
        {
            return View();
        }

        public IActionResult TicketType()
        {
            return View();
        }
        public IActionResult PunchInLocation()
        {
            return View();
        }
    }
}
