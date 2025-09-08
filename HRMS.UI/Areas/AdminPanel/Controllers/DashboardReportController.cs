using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class DashboardReportController : Controller
    {
        public IActionResult CompOffApprovalApplicationAdmin()
        {
            return View();
        }
    }
}
