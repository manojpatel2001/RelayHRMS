using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class CompanyInformationController : Controller
    {
        public IActionResult CompanyDetails()
        {
            return View();
        }
    }
}
