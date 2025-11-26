
using HRMS_Core.VM.EmailService;
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
        Task<List<DailyAbsentReportResult>> GetDailyAbsentReport();
        Task<EmailReport?> GetEmailSendTime(string ReportName);
        Task<List<TodayLeftEmployeeEmailVM>> GetTodayLeftEmployeesEmailData();
     }
}
