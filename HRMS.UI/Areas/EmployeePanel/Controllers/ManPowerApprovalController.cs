using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
    public class ManPowerApprovalController : Controller
    {
        [Area("EmployeePanel")]
        public IActionResult ManPowerApproval()
        {
            return View();
        }
    }
}
