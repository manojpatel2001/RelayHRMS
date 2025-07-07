using HRMS.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class HomeController : Controller
    {
        public IActionResult Index() {
            return View();
        }
    }
}
