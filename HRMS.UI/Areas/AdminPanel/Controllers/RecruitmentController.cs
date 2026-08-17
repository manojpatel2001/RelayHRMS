using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class RecruitmentController : Controller
    {
        public IActionResult JobPositionMaster()
        {
            return View();
        }

        public IActionResult JobPosting()
        {
            return View();
        }

        public IActionResult CandidateApplication()
        {
            return View();
        }

        public IActionResult CandidateApplicationForm()
        {
            return View();
        }

        public IActionResult CandidatePipeline()
        {
            return View();
        }

        public IActionResult InterviewScheduling()
        {
            return View();
        }

        public IActionResult OfferManagement()
        {
            return View();
        }

        public IActionResult OfferApprovalConfig()
        {
            return View();
        }

        public IActionResult CandidateDocumentTypeMaster()
        {
            return View();
        }

        public IActionResult SalaryStructureMaster()
        {
            return View();
        }

        public IActionResult CTCCalculationPreview()
        {
            return View();
        }

        public IActionResult CandidateOnboarding()
        {
            return View();
        }
    }
}
