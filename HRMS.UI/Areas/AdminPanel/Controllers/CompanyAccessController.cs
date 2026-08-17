using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class CompanyAccessController : Controller
    {
        public IActionResult CompanyAccess()
        {
            return View();
        }
    }
}
