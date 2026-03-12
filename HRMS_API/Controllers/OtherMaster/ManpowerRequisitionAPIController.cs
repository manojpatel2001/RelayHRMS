using HRMS_API.NotificationService.HubService;
using HRMS_API.NotificationService.ManageService;
using HRMS_API.Services;
using HRMS_Core.Notifications;
using HRMS_Core.Services;
using HRMS_Core.VM;
using HRMS_Core.VM.ApprovalManagement;
using HRMS_Core.VM.EmailService;
using HRMS_Core.VM.Leave;
using HRMS_Core.VM.OtherMaster;
using HRMS_Core.VM.Report;
using HRMS_Core.VM.Salary;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HRMS_API.Controllers.OtherMaster
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManpowerRequisitionAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService _emailService;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;
        private readonly AutoJobService _autoJobService;
        public ManpowerRequisitionAPIController(IUnitOfWork unitOfWork, EmailService emailService, IHubContext<NotificationRemainderHub> hubContext, AutoJobService autoJobService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _hubContext = hubContext;
            _autoJobService = autoJobService;
        }

        [HttpPost("GetAllManpowerRequisitions")]
        public async Task<APIResponse> GetAllManpowerRequisitions(CommonParameter commonParameter)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRequisitionRepository.GetAllManpowerRequisitions(commonParameter);
                return data;
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve manpower requisitions. Please try again later." };
            }
        }

        [HttpGet("GetDropDownForManpower/{CompanyId}")]
        public async Task<APIResponse> GetDropDownForManpower(int CompanyId)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRequisitionRepository.GetDropDownForManpower(CompanyId);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }
        [HttpPost("CreateManpowerRequisition")]
        public async Task<APIResponse> CreateManpowerRequisition([FromBody] ManpowerRequisition model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Manpower requisition details cannot be null." };

                model.CreatedDate = DateTime.UtcNow;
                model.IsEnabled = true;
                model.IsDeleted = false;

                var result = await _unitOfWork.ManpowerRequisitionRepository.CreateManpowerRequisition(model);

                if (result.isSuccess)
                {
                    var createdData = result.Data as ManpowerRequisitionCreatedData;
                    int newId = createdData.ManpowerRequisitionId;

                    dynamic emailReport = null;

                    var requisitionData = await _unitOfWork.ManpowerRequisitionRepository
                                                .GetManpowerRequisitionEmailDetails(newId);

                    if (requisitionData != null && requisitionData.isSuccess)
                    {
                        var manpowerData = requisitionData.Data as dynamic;

                        emailReport = await _unitOfWork.EmailReportRepository
                                            .GetManpowerRequisitionEmail(newId);

                        if (emailReport != null && !string.IsNullOrEmpty((string)emailReport.ToEmail_1stApprover_RM))
                        {
                            var placeholders = new Dictionary<string, string>
                    {
                        { "Department",    manpowerData?.Department ?? "N/A" },
                        { "Designation",   manpowerData?.Designation ?? "N/A" },
                        { "RequestedBy",   manpowerData?.RequestedByName ?? "N/A" },
                        { "RequestedDate", manpowerData?.CreatedDate != null
                                           ? ((DateTime)manpowerData.CreatedDate).ToString("dd-MM-yyyy")
                                           : "N/A" },
                        { "CompanyName",   manpowerData?.CompanyName ?? "N/A" },
                        { "Year",          DateTime.Now.Year.ToString() }
                    };

                            var emailRequest = new EmailRequest
                            {
                                ToEmails = ((string)emailReport.ToEmail_1stApprover_RM).Split(',').ToList(),
                                BccEmails = emailReport.BccEmail_Director != null
                                            ? ((string)emailReport.BccEmail_Director).Split(',').ToList()
                                            : null,
                                CcEmails = emailReport.ToEmail_2ndApprover_HOD != null
                                            ? ((string)emailReport.ToEmail_2ndApprover_HOD).Split(',').ToList()
                                            : null,
                                Subject = $"New Manpower Requisition Request - {createdData.SerialNo}",
                                TemplateName = "ManpowerRequisitionEmailTemplate.html",
                                Placeholders = placeholders
                            };

                            var emailLogger = new EmailLogger
                            {
                                ToEmail = (string)emailReport.ToEmail_1stApprover_RM,
                                BCCEmail = (string)emailReport.BccEmail_Director,
                                CCEmail = (string)emailReport.ToEmail_2ndApprover_HOD,
                                Subject = emailRequest.Subject,
                                Body = emailRequest.TemplateName,
                                Status = EmailStatus.Pending,
                                SentAt = DateTime.UtcNow,
                                Comments = "Email ready for sent"
                            };

                            await _unitOfWork.EmailLoggerRepository.ManageEmailLoggerAsync(emailLogger, "CREATE");
                            bool emailSent = await _emailService.SendEmailAsync(emailRequest);
                        }
                    }

                    var employeeDetails = await _unitOfWork.EmployeeManageRepository
                                                .GetEmployeeById(Convert.ToInt32(model.CreatedBy));

                    if (employeeDetails != null && emailReport != null)
                    {
                        var rmId = (int?)emailReport.RM_Id;

                        var notification = new NotificationRemainders()
                        {
                            NotificationMessage = $"{employeeDetails.FullName} has requested approval for Manpower Requisition: {createdData.SerialNo}",
                            NotificationTime = DateTime.UtcNow,
                            SenderId = model.CreatedBy.ToString(),
                            ReceiverIds = rmId?.ToString() ?? string.Empty,
                            NotificationType = NotificationType.ManpowerRequisition.ToString(),
                            NotificationAffectedId = newId
                        };

                        var savedNotification = await _unitOfWork.NotificationRemainderRepository
                                                      .CreateNotificationRemainder(notification);

                        if (savedNotification.Success > 0)
                        {
                            notification.NotificationRemainderId = savedNotification.Success;

                            if (rmId.HasValue)
                            {
                                var reportingConnection = NotificationRemainderConnectionManager
                                                            .GetConnections(rmId.Value.ToString()) as IEnumerable<string>
                                                            ?? Enumerable.Empty<string>();

                                if (reportingConnection.Any())
                                {
                                    await _hubContext.Clients.Clients(reportingConnection)
                                          .SendAsync("ReceiveNotificationRemainder", notification);
                                }
                            }
                        }
                    }

                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = result.ResponseMessage,
                        Data = result.Data
                    };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create manpower requisition. Please try again later." };
            }
        }

        [HttpPost("UpdateManpowerRequisition")]
        public async Task<APIResponse> UpdateManpowerRequisition([FromBody] ManpowerRequisition model)
        {
            try
            {
                if (model == null || model.ManpowerRequisitionId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Manpower requisition details cannot be null." };

                model.UpdatedDate = DateTime.UtcNow;

                var result = await _unitOfWork.ManpowerRequisitionRepository.UpdateManpowerRequisition(model);

                return new APIResponse
                {
                    isSuccess = result.isSuccess,
                    ResponseMessage = result.ResponseMessage
                };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update manpower requisition. Please try again later." };
            }
        }

        [HttpDelete("DeleteManpowerRequisition")]
        public async Task<APIResponse> DeleteManpowerRequisition(DeleteRecordVM model)
        {
            try
            {
                if (model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Manpower requisition ID cannot be zero." };

                var result = await _unitOfWork.ManpowerRequisitionRepository.DeleteManpowerRequisition(model);

                return new APIResponse
                {
                    isSuccess = result.isSuccess,
                    ResponseMessage = result.ResponseMessage
                };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete manpower requisition. Please try again later." };
            }
        }
        [HttpPost("GetAllSerialNo")]
        public async Task<APIResponse> GetAllSerialNo(CommonParameter model)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRequisitionRepository.GetAllSerialNo(model);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }

        [HttpGet("GetManpowerRequisitionByManpowerRequisitionId/{ManpowerRequisitionId}")]
        public async Task<APIResponse> GetManpowerRequisitionByManpowerRequisitionId(int ManpowerRequisitionId)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRequisitionRepository.GetManpowerRequisitionByManpowerRequisitionId(ManpowerRequisitionId);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }
        [HttpPost("UpdateJoinningDetails")]
        public async Task<APIResponse> UpdateJoinningDetails(UpdateJoinningDetailsModel model)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRequisitionRepository.UpdateJoinningDetails(model);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }

        [HttpPost("GetAllJoiningManpowerRequisitions")]
        public async Task<APIResponse> GetAllJoiningManpowerRequisitions(CommonParameter commonParameter)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRequisitionRepository.GetAllJoiningManpowerRequisitions(commonParameter);
                return data;
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve manpower requisitions. Please try again later." };
            }
        }

        [HttpPut("ManPowerApproval")]
        public async Task<APIResponse> ManPowerApproval([FromBody] ManPowerfilter model)
        {
            try
            {
                if (model == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Man Power details cannot be null"
                    };
                }

                var ApprovalResult = await _unitOfWork.ManpowerRequisitionRepository.ApprovalManPower(model);

                var approvalAction = new ApprovalRequestLevelActionPara
                {
                    ApprovalRequestLevelId = model.ApprovalRequestLevelId,
                    ApprovalRequestId = model.ApprovalRequestId,
                    StatusId = model.StatusId,
                    Remarks = model.Remarks ?? "N/A",
                    ActionBy = Convert.ToInt32(model.UpdatedBy)
                };

                var approvalActionResult = await _unitOfWork.ApprovalManagementRepository
                                                 .ManpowerApprovalRequestLevel(approvalAction);

                if (ApprovalResult.Success <= 0)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = ApprovalResult?.ResponseMessage ?? "Failed to update manpower approval status"
                    };
                }

                var approverDetails = await _unitOfWork.EmployeeManageRepository
                                            .GetEmployeeById(Convert.ToInt32(model.UpdatedBy));
                var requisitionData = await _unitOfWork.ManpowerRequisitionRepository
                                               .GetManpowerRequisitionEmailDetails(model.ManpowerRequisitionId);
                if (approverDetails != null)
                {
                    var manpowerData = requisitionData.Data as dynamic;
                    string requestCreatedBy = manpowerData?.CreatedBy?.ToString();
                    string statusLabel = model.StatusId switch
                    {
                        1 => "approved",
                        2 => "rejected",
                        3 => "pending",
                        _ => "updated"
                    };

                    var notification = new NotificationRemainders
                    {
                        NotificationMessage = $"Your Manpower Requisition request has been {statusLabel} by {approverDetails.FullName}.",
                        NotificationTime = DateTime.UtcNow,
                        SenderId = approverDetails.Id.ToString(),
                        ReceiverIds = requestCreatedBy.ToString(),
                        NotificationType = NotificationType.ManpowerRequisitionApproval,
                        NotificationAffectedId = model.ManpowerRequisitionId
                    };

                    var savedNotification = await _unitOfWork.NotificationRemainderRepository
                                                  .CreateNotificationRemainder(notification);

                    if (savedNotification.Success > 0)
                    {
                        notification.NotificationRemainderId = savedNotification.Success;

                        var connections = NotificationRemainderConnectionManager
                                            .GetConnections(requestCreatedBy.ToString());

                        if (connections.Any())
                        {
                            await _hubContext.Clients.Clients(connections)
                                  .SendAsync("ReceiveNotificationRemainder", notification);
                        }
                    }
                }

                return new APIResponse
                {
                    isSuccess = true,
                    ResponseMessage = ApprovalResult.ResponseMessage
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to update record. Please try again later!"
                };
            }
        }
        [HttpPost("GetAllManpowerRequisitionsAdmin")]
        public async Task<APIResponse> GetAllManpowerRequisitionsAdmin(CommonParameter commonParameter)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRequisitionRepository.GetAllManpowerRequisitionsAdmin(commonParameter);
                return data;
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve manpower requisitions. Please try again later." };
            }
        }
        [HttpPost("GetAllJoiningWithApprovalCheck_Admin")]
        public async Task<APIResponse> GetAllJoiningWithApprovalCheck_Admin(CommonParameter commonParameter)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRequisitionRepository.GetAllJoiningWithApprovalCheck_Admin(commonParameter);
                return data;
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve manpower requisitions. Please try again later." };
            }
        }

        [HttpPost("GetAllManpowerRequisitionsEss")]
        public async Task<APIResponse> GetAllManpowerRequisitionsEss([FromBody] SearchVmCompOff filter)
        {
            try
            {
                var result = await _unitOfWork.ManpowerRequisitionRepository.GetAllManpowerRequisitionsEss(filter);
                if (result == null)
                {
                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = "Data Fetched not Sucessfully"
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = result,
                    ResponseMessage = "Data Fetched Sucessfully"
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = true,
                    ResponseMessage = "Data not fetched successfully."
                };
            }
        }
        [HttpPost("GetAllJoingWithApprovalCheck")]
        public async Task<APIResponse> GetAllJoingWithApprovalCheck([FromBody] SearchVmCompOff filter)
        {
            try
            {
                var result = await _unitOfWork.ManpowerRequisitionRepository.GetAllJoingWithApprovalCheck(filter);
                if (result == null)
                {
                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = "Data Fetched not Sucessfully"
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = result,
                    ResponseMessage = "Data Fetched Sucessfully"
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = true,
                    ResponseMessage = "Data not fetched successfully."
                };
            }
        }


        [HttpGet("GetActiveEmployee/{Employeeid}")]
        public async Task<APIResponse> GetActiveEmployee(int Employeeid)
        {
            try
            {
                var data = await _unitOfWork.ManpowerRequisitionRepository.sp_GetActiveEmployee(Employeeid);

                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve data. Please try again later." };
            }
        }

        //[HttpGet("TestProbation")]
        //public async Task TestProbation()
        //{
        //    await _autoJobService.ScheduleDailyProbationNotification();
        //}  


        [HttpPost("GetManpowerRequisitionApprovalStatus")]
        public async Task<APIResponse> GetManpowerRequisitionApprovalStatus([FromBody] ManpowerApprovalStatusRequestDto request)
        {
            try
            {
                var result = await _unitOfWork.ManpowerRequisitionRepository.GetManpowerRequisitionApprovalStatus(request);

                return new APIResponse
                {
                    isSuccess = result.isSuccess,
                    Data = result.isSuccess ? result.Data : null,
                    ResponseMessage = result.ResponseMessage
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = null,
                    ResponseMessage = $"Unable to fetch manpower approval status. Error: {ex.Message}"
                };
            }
        }
    }

}
