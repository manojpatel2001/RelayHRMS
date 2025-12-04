using Azure;
using HRMS_Core.VM.ApprovalManagement;
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
        // Approval Scheme Level
        Task<APIResponse> ManageApprovalSchemeLevel(List<ApprovalSchemeLevelVM> model);

        // Approval Scheme Level Department (Optional)
        Task<APIResponse> ManageApprovalSchemeLevelDepartment(ApprovalSchemeLevelDepartmentVM model);

        // Approval Request
        Task<APIResponse> ManageApprovalRequest(ApprovalRequestVM model);

        // Approval Request Level
        Task<APIResponse> ManageApprovalRequestLevel(ApprovalRequestLevelVM model);

        // Approval Request History
        Task<APIResponse> InsertApprovalRequestHistory(ApprovalRequestHistoryVM model);

        // Approve / Reject Request Level
        Task<APIResponse> ActionOnApprovalRequestLevel(ApprovalRequestLevelActionVM model);

        // GET LISTS (If required in UI)
        Task<IEnumerable<dynamic>> GetApprovalSchemeLevels();
        Task<IEnumerable<dynamic>> GetApprovalRequests();
        Task<IEnumerable<dynamic>> GetApprovalRequestLevels(int requestId);
        Task<IEnumerable<dynamic>> GetApprovalRequestHistory(int requestId);
        Task<APIResponse> GetAllApprovalSchemeLevelsByCompanyId(int companyId);
        Task<APIResponse> GetAllApprovalSchemeLevelsBySchemeId(int schemeId);
        Task<APIResponse> DeleteApprovalSchemeLevel(ApprovalSchemeLevelPara para);
    }
}

