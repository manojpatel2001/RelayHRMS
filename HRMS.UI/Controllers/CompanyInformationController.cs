using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Controllers
{
    public class CompanyInformationController : Controller
    {
        public IActionResult CompanyDetails()
        {
            return View();
        }
    }
}
