using Azure;
using HRMS_Core.VM.ApprovalManagement;
using HRMS_Core.VM.Probations;
using HRMS_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.ApprovalManagement
{
    public interface IApprovalManagementRepository
    {
        Task<APIResponse> GetApprovalDropdownDetails();
        Task<APIResponse> GetAllEmployByDepartmentId(int? companyId, int? departmentId);
        // Approval Scheme Level
        Task<APIResponse> ManageApprovalLevel(List<ApprovalLevelVM> models);


        // Approval Request
        Task<APIResponse> ManageApprovalRequest(ApprovalRequestVM model);

        // Approval Request Level
        Task<APIResponse> ManageApprovalRequestLevel(ApprovalRequestLevelVM model);

        // Approval Request History
        Task<APIResponse> InsertApprovalRequestHistory(ApprovalRequestHistoryVM model);

        // Approve / Reject Request Level
        Task<APIResponse> ActionOnApprovalRequestLevel(ApprovalRequestLevelActionVM model);

     
        Task<APIResponse> GetAllApprovalLevelsByCompanyId(int companyId);
        Task<APIResponse> GetAllApprovalLevelsByApprovalMasterId(int ApprovalMasterId);
        Task<APIResponse> DeleteApprovalLevel(ApprovalLevelPara para);
        Task<APIResponse> AutomateProbationEndApprovalRequests(int approvalMasterId);
        Task<APIResponse> AutomateLoanEndApprovalRequests(int approvalMasterId);
        Task<EscalationReturnPara> EscalatePendingApprovalRequests();
        Task<ApprovalRequestLevelActionVm> ApprovalRequestLevelAction(ApprovalRequestLevelActionPara para);
        Task<APIResponse> GetPendingApprovalRequests(GetPendingApprovalRequestsPara para);
        Task<APIResponse> GetUpcomingProbationDetails(GetUpcomingProbationDetailsPara para);
       

    }
}

