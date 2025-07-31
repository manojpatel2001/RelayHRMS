using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
    public class PartialViewController : Controller
    {
        [Area("EmployeePanel")]
        public IActionResult PersonalInfo()
        {
            return PartialView("_PersonalInfo");
        }
    }
}
