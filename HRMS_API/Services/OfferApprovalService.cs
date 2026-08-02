using HRMS_API.NotificationService.HubService;
using HRMS_API.NotificationService.ManageService;
using HRMS_Core.Notifications;
using HRMS_Core.Recruitment;
using HRMS_Infrastructure.Interface;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS_API.Services
{
    // Self-contained Offer Approval workflow — resolves an admin-configured approver
    // chain (Recruiter/HiringManager from the Job Position, or a fixed employee for
    // DepartmentHead/HRManager/Finance/Management) into real employee ids and drives the
    // request/level lifecycle. Deliberately does not touch the generic ApprovalMaster/
    // ApprovalLevel/ApprovalRequest/ApprovalRequestLevel tables (see Phase 2 plan).
    public class OfferApprovalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;

        public OfferApprovalService(IUnitOfWork unitOfWork, IHubContext<NotificationRemainderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        public async Task<(bool Success, string Message)> CreateApprovalRequestAsync(int offerId, int? createdBy)
        {
            var offerResponse = await _unitOfWork.OfferRepository.GetOfferById(offerId);
            if (offerResponse?.Data is not Offer offer)
                return (false, "Offer not found.");

            var positionResponse = await _unitOfWork.JobPositionRepository.GetJobPositionById(offer.JobPositionId);
            var position = positionResponse?.Data as JobPositionMaster;

            var configResponse = await _unitOfWork.OfferApprovalRepository.GetLevelConfig(position?.CompanyId);
            var levels = (configResponse?.Data as List<OfferApprovalLevelConfig>)?.OrderBy(l => l.LevelNo).ToList() ?? new List<OfferApprovalLevelConfig>();

            if (levels.Count == 0)
                return (false, "No offer approval chain has been configured for this company.");

            var requestResponse = await _unitOfWork.OfferApprovalRepository.CreateApprovalRequest(offerId, createdBy);
            if (!requestResponse.isSuccess)
                return (false, requestResponse.ResponseMessage ?? "Failed to create the approval request.");

            int requestId = Convert.ToInt32(requestResponse.Data);

            foreach (var level in levels)
            {
                int? approverEmployeeId = ResolveApprover(level, position);
                var escalationDueOn = DateTime.UtcNow.AddDays(level.EscalationDays);
                await _unitOfWork.OfferApprovalRepository.AddRequestLevel(requestId, level.LevelNo, approverEmployeeId, escalationDueOn, createdBy);
            }

            await _unitOfWork.OfferRepository.SetOfferStatus(offerId, "PendingApproval", createdBy);

            var firstLevel = levels.First();
            var firstApproverId = ResolveApprover(firstLevel, position);
            if (firstApproverId.HasValue)
                await NotifyApproverAsync(firstApproverId.Value, offerId, "A new offer is pending your approval.", NotificationType.OfferApprovalRequest);

            return (true, "Offer submitted for approval successfully.");
        }

        public async Task<(bool Success, string Message)> ActionOnLevelAsync(int offerApprovalRequestLevelId, string action, int actionBy, string? remarks)
        {
            var actionResult = await _unitOfWork.OfferApprovalRepository.ActionOnRequestLevel(offerApprovalRequestLevelId, action, actionBy, remarks);
            if (!actionResult.isSuccess)
                return (false, actionResult.ResponseMessage ?? "Failed to record the approval action.");

            var data = actionResult.Data as OfferApprovalActionResult;
            bool isAllLevelsCompleted = data?.IsAllLevelsCompleted ?? false;
            int? offerApprovalRequestId = data?.OfferApprovalRequestId;

            if (offerApprovalRequestId == null)
                return (true, actionResult.ResponseMessage ?? "Recorded.");

            var requestLevelsResponse = await _unitOfWork.OfferApprovalRepository.GetRequestLevelsByRequestId(offerApprovalRequestId.Value);
            var requestLevels = (requestLevelsResponse?.Data as List<OfferApprovalRequestLevel>) ?? new List<OfferApprovalRequestLevel>();
            var currentLevel = requestLevels.FirstOrDefault(l => l.OfferApprovalRequestLevelId == offerApprovalRequestLevelId);
            var allLevels = requestLevels.OrderBy(l => l.LevelNo).ToList();
            int offerId = currentLevel?.OfferId ?? 0;

            if (string.Equals(action, "Reject", StringComparison.OrdinalIgnoreCase))
            {
                if (offerId > 0) await _unitOfWork.OfferRepository.SetOfferStatus(offerId, "Rejected", actionBy);
            }
            else if (isAllLevelsCompleted)
            {
                if (offerId > 0) await _unitOfWork.OfferRepository.SetOfferStatus(offerId, "Approved", actionBy);
            }
            else
            {
                var nextLevel = allLevels.FirstOrDefault(l => l.LevelNo == (currentLevel?.LevelNo ?? 0) + 1);
                if (nextLevel?.ApproverEmployeeId != null && offerId > 0)
                    await NotifyApproverAsync(nextLevel.ApproverEmployeeId.Value, offerId, "An offer is now pending your approval.", NotificationType.OfferApprovalRequest);
            }

            return (true, actionResult.ResponseMessage ?? "Recorded.");
        }

        private static int? ResolveApprover(OfferApprovalLevelConfig level, JobPositionMaster? position)
        {
            return level.ApproverRole?.ToUpperInvariant() switch
            {
                "RECRUITER" => position?.OwnerRecruiterId,
                "HIRINGMANAGER" => position?.ReportingManagerId,
                _ => level.FixedApproverEmployeeId
            };
        }

        private async Task NotifyApproverAsync(int approverEmployeeId, int offerId, string message, string notificationType)
        {
            var notification = new NotificationRemainders
            {
                NotificationMessage = message,
                NotificationTime = DateTime.UtcNow,
                ReceiverIds = approverEmployeeId.ToString(),
                NotificationType = notificationType,
                NotificationAffectedId = offerId
            };

            var saved = await _unitOfWork.NotificationRemainderRepository.CreateNotificationRemainder(notification);
            if (saved.Success > 0)
            {
                notification.NotificationRemainderId = saved.Success;
                var connections = NotificationRemainderConnectionManager.GetConnections(approverEmployeeId.ToString());
                if (connections.Any())
                    await _hubContext.Clients.Clients(connections).SendAsync("ReceiveNotificationRemainder", notification);
            }
        }
    }
}
