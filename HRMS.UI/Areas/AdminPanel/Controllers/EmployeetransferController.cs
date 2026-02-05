using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class EmployeetransferController : Controller
    {
        public IActionResult Employeetransfer()
        {
            return View();
        }
    }
}
