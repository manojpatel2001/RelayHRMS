using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePannel.Controllers
{
    [Area("EmployeePannel")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewBag.BaseUrl = _configuration["BaseUrlSettings:baseUrl"];
            return View();
        }
    }

}
