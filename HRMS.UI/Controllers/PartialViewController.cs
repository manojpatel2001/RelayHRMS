using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Controllers
{
    public class PartialViewController : Controller
    {
        public IActionResult EmployeeInfo()
        {
            return PartialView("_EmployeeInfo");
        }


        public IActionResult Contact()
        {
            return PartialView("_Contact");
        }

        public IActionResult EmergencyContact()
        {
            return PartialView("_EmergencyContact");
        }

        public IActionResult Reporting()
        {
            return PartialView("_Reporting");
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
        public IActionResult LoadCompanyModal()
        {
            return PartialView("_CompanyModal");
        }



    }
}
