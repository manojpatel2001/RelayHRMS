
using HRMS_Core.VM.EmailService;
using HRMS_Core.VM.Report;
using HRMS_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.EmailService
{
    public interface IEmailReportRepository
    {
        Task<List<DailyAbsentReportResult>> GetEmployeeEmailDataGrouped();
         Task InsertAbsentEmployeeEmailData();
        Task<EmailReport?> GetEmailSendTime(string ReportName);
        Task<ManpowerRequisitionEmailViewModel> GetManpowerRequisitionEmail(int ManpowerRequisitionId);
        Task<AccessRequestViewModel> GetManpowerRequisitionEmailITandHR(int ManpowerRequisitionId);
        Task<List<TodayLeftEmployeeEmailVM>> GetTodayLeftEmployeesEmailData();
        Task<List<EmailAllReport>> GetAllEmailSendTime();
        Task UpdateEmailReport(EmailAllReport report);
     }
}
