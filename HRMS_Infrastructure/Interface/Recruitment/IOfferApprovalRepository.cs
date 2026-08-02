using HRMS_Core.Recruitment;
using HRMS_Utility;
using System;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface IOfferApprovalRepository
    {
        Task<APIResponse> GetLevelConfig(int? companyId);
        Task<APIResponse> CreateLevelConfig(OfferApprovalLevelConfig model);
        Task<APIResponse> UpdateLevelConfig(OfferApprovalLevelConfig model);
        Task<APIResponse> DeleteLevelConfig(int offerApprovalLevelConfigId, int? deletedBy);

        Task<APIResponse> CreateApprovalRequest(int offerId, int? createdBy);
        Task<APIResponse> AddRequestLevel(int offerApprovalRequestId, int levelNo, int? approverEmployeeId, DateTime? escalationDueOn, int? createdBy);
        Task<APIResponse> ActionOnRequestLevel(int offerApprovalRequestLevelId, string action, int actionBy, string? remarks);

        Task<APIResponse> GetRequestByOfferId(int offerId);
        Task<APIResponse> GetRequestLevelsByRequestId(int offerApprovalRequestId);
        Task<APIResponse> GetPendingApprovalsByApproverEmployeeId(int approverEmployeeId);
        Task<APIResponse> GetRequestHistoryByRequestId(int offerApprovalRequestId);
        Task<APIResponse> GetAndMarkOverdueLevels();
    }
}
