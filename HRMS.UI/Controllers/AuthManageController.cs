using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Controllers
{
    public class AuthManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
