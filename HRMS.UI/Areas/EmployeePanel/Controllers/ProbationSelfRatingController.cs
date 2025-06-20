using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{

    [Area("EmployeePanel")]
    public class ProbationSelfRatingController : Controller
    {
        private readonly IConfiguration _configuration;


        public ProbationSelfRatingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }
    }
}
