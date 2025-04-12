using HRMS.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HRMS.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() {
            return View();
        }
    }
}
