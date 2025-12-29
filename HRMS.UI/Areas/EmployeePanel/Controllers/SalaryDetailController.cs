using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
    [Area("EmployeePanel")]
    public class SalaryDetailController : Controller
    {
        private readonly IConfiguration _configuration;

        public SalaryDetailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult SalarySlip()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }
        public IActionResult EmpLoanApplication()
        {
            ViewBag.BaseUrl = _configuration["UIBaseUrlSettings:baseUrl"];

            return View();
        }

       
    }
}
