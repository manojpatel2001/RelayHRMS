using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
    [Area("EmployeePanel")]

    public class ChangeRequestController : Controller
    {
        private readonly IConfiguration _configuration;

        public ChangeRequestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult ChangeRequestApplication()
        {
            ViewBag.BaseUrl = _configuration["BaseUrlSettings:baseUrl"];

            return View();
        }

        public IActionResult ChangeRequestApproval()
        {
            ViewBag.BaseUrl = _configuration["BaseUrlSettings:baseUrl"];

            return View();
        }
    }


}
