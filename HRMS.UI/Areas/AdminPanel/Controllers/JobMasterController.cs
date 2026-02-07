using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class JobMasterController : Controller
    {
        public IActionResult State()
        {
            return View();
        }
        public IActionResult Branch()
        {
            return View();
        }

        public IActionResult ManageBranch()
        {
            return View();  
        }
        public IActionResult ManageDepartment()
        {
            return View();  
        }
        public IActionResult ManageDesignation()
        {
            return View();  
        }
        public IActionResult ManageGrade()
        {
            return View();  
        }

        public IActionResult ManageShifts() {
            return View();
        }

        public IActionResult Reason()
        {
            return View();
        }
        public IActionResult NewsAnnouncement()
        {
            return View();
        }

    }
}
