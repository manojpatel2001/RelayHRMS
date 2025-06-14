using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
    [Area("EmployeePanel")]
    public class CompOffApplicationController : Controller
    {
        private readonly IConfiguration _configuration;


        public CompOffApplicationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewBag.BaseUrl = _configuration["BaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult AddCompOffApplication()
        {
            ViewBag.BaseUrl = _configuration["BaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult CompOffApproval()
        {
            ViewBag.BaseUrl = _configuration["BaseUrlSettings:baseUrl"];

            return View();
        }
    }
}
