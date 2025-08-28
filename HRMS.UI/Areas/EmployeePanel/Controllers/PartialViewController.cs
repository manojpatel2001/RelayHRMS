using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.EmployeePanel.Controllers
{
    [Area("EmployeePanel")]
    public class PartialViewController : Controller
    {
        
        public IActionResult PersonalInfo()
        {
            return PartialView("_PersonalInfo");
        }

        public IActionResult Contact()
        {
            return PartialView("_Contact");
        }
        public IActionResult EmergencyContact()
        {
            return PartialView("_EmergencyContact");
        }
        public IActionResult Dependents()
        {
            return PartialView("_Dependents");
        }
        public IActionResult Immigration()
        {
            return PartialView("_Immigration");
        }
        public IActionResult AssetAndLicense()
        {
            return PartialView("_AssetAndLicense");
        }
        public IActionResult Reporting()
        {
            return PartialView("_Reporting");
        }
        public IActionResult Experience()
        {
            return PartialView("_Experience");
        }
        public IActionResult Qualification()
        {
            return PartialView("_Qualification");
        }
        public IActionResult Skills()
        {
            return PartialView("_Skills");
        }
        public IActionResult Attachments()
        {
            return PartialView("_Attachments");
        }
        public IActionResult Allowance()
        {
            return PartialView("_Allowance");
        }
        public IActionResult Salary()
        {
            return PartialView("_Salary");
        }
    }
}
