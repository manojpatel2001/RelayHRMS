using HRMS_Core.Probations;
using HRMS_Core.VM;
using HRMS_Core.VM.ApprovalManagement;
using HRMS_Core.VM.Probations;
using HRMS_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Probations
{
    public interface IProbationPerformanceRepository
    {
        Task<APIResponse> GetAllProbationStatus();
        Task<APIResponse> GetAllProbationEvaluationPeriods(int? probationStatusId = null);
        Task<List<ProbationEmployeeVM>> GetAllProbationEmployees(int ProbationManagerId);
        Task<APIResponse> GetPendingApprovalRequestsWithHistory(GetPendingApprovalRequestsWithHistoryPara parameters);
        Task<EmployeeProbationDetailVM?> GetEmployeeForProbationByEmployeeId(int EmployeeId);
        Task<APIResponse> CreateProbationPerformance(ProbationPerformance probationPerformance);
        Task<ApproverDetailsViewModel?> GetApproverDetails(int ApprovalRequestId);
        Task<List<ConfirmationProbationDetails>> GetAllConfirmationProbationDetails(GetAllConfirmationProbationDetailsPara parameters);

        Task<APIResponse> UpdateMailRequest(int approvalRequestId, bool isMailSent);
    }
}
