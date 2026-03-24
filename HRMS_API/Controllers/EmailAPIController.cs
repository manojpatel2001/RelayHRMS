//using DocumentFormat.OpenXml.Bibliography;
//using DocumentFormat.OpenXml.Spreadsheet;
//using HRMS_API.Services;
//using HRMS_Core.Services;
//using HRMS_Infrastructure.Interface;
//using HRMS_Infrastructure.Repository;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace HRMS_API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EmailAPIController : ControllerBase
//    {
//        private readonly EmailJobService _emailJobService;
//        private readonly IUnitOfWork _unitOfWork;

        

//        public EmailAPIController(IUnitOfWork unitOfWork,EmailJobService emailJobService)
//        {
//            _emailJobService = emailJobService;
//            _unitOfWork = unitOfWork;
//        }

//        [HttpPost("daily-absent-reporting")]
//        public async Task<IActionResult> DailyAbsent()
//        {
//            var emailReport = await _unitOfWork.EmailReportRepository.GetEmailSendTime(EmailReportType.DailyAbsentEmployeesReport);
//            if (emailReport == null)
//            {

//            }
//            if (emailReport.LastRunDate?.Date == now.Date && !report.IsForceSend)
//            {

//            }
//                var result = await _emailJobService.SendReportingEmailAsync();
//            return Ok(result);
//        }

//        [HttpPost("daily-absent-all")]
//        public async Task<IActionResult> DailyAbsentAll()
//        {
//            var result = await _emailJobService.RunReport("Daily Absent All Employees Report");
//            return Ok(result);
//        }

//        [HttpPost("daily-left")]
//        public async Task<IActionResult> DailyLeft()
//        {
//            var result = await _emailJobService.RunReport("Daily Left Employee Report");
//            return Ok(result);
//        }

//    }
//}
