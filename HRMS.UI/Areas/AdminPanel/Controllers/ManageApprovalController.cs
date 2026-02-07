using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ManageApprovalController : Controller
    {
        public IActionResult ApprovalMaster()
        {
            return View();
        }
        public IActionResult ApprovalManagement()
        {
            return View();
        }
        public IActionResult AddApprovalLevel()
        {
            return View();
        }
    }
}
