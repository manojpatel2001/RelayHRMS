using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
    public class MyInterviewsController : Controller
    {
        [Area("EmployeePanel")]
        public IActionResult InterviewFeedback()
        {
            return View();
        }
    }
}
