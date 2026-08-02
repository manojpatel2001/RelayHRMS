using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
    public class OfferApprovalEssController : Controller
    {
        [Area("EmployeePanel")]
        public IActionResult OfferApprovalEss()
        {
            return View();
        }
    }
}
