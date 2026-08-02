using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Utility;
using System;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface IInterviewRepository
    {
        Task<APIResponse> GetAllInterviews(int? candidateApplicationId);
        Task<APIResponse> GetInterviewById(int interviewId);
        Task<APIResponse> CreateInterview(Interview model);
        Task<APIResponse> UpdateInterview(Interview model);
        Task<APIResponse> DeleteInterview(DeleteRecordVM model);
        Task<APIResponse> GetCalendarEvents(int? companyId, DateTime fromDate, DateTime toDate);
        Task<APIResponse> GetInterviewsByPanelistEmployeeId(int employeeId);

        Task<APIResponse> AddPanelist(InterviewPanelist model);
        Task<APIResponse> UpdatePanelist(InterviewPanelist model);
        Task<APIResponse> RemovePanelist(DeleteRecordVM model);
        Task<APIResponse> GetPanelistsByInterviewId(int interviewId);
    }
}
