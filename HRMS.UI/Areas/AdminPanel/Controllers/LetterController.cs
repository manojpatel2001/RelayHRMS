using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class LetterController : Controller
    {
        public IActionResult ExperienceLetter()
        {
            return View();
        }
    }
}
