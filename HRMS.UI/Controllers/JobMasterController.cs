using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Controllers
{
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

    }
}
