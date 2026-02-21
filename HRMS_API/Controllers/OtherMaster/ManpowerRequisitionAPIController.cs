using HRMS_API.Services;
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

namespace HRMS_API.Controllers.OtherMaster
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManpowerRequisitionAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService _emailService;
        public ManpowerRequisitionAPIController(IUnitOfWork unitOfWork, EmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
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

                // Step 1: Create karo
                var result = await _unitOfWork.ManpowerRequisitionRepository.CreateManpowerRequisition(model);

                if (result.isSuccess)
                {
                    var createdData = result.Data as ManpowerRequisitionCreatedData;
                    int newId = createdData.ManpowerRequisitionId;

                    var requisitionData = await _unitOfWork.ManpowerRequisitionRepository
                                                .GetManpowerRequisitionEmailDetails(newId);

                    if (requisitionData != null && requisitionData.isSuccess)
                    {
                        var manpowerData = requisitionData.Data as dynamic;

                        var emailReport = await _unitOfWork.EmailReportRepository
                                                .GetEmailSendTime(EmailReportType.ManpowerRequisition.ToString());

                        if (emailReport != null && !string.IsNullOrEmpty(emailReport.ToEmails))
                        {
                            var placeholders = new Dictionary<string, string>
            {
                { "RequestId",      manpowerData?.RequestId?.ToString() ?? "N/A" },
                { "Department",     manpowerData?.Department ?? "N/A" },
                { "Designation",    manpowerData?.Designation ?? "N/A" },
                { "RequestedBy",    manpowerData?.RequestedByName ?? "N/A" },
                { "RequestedDate",  manpowerData?.CreatedDate != null
                                    ? ((DateTime)manpowerData.CreatedDate).ToString("dd-MM-yyyy")
                                    : "N/A" },
                { "CompanyName",    manpowerData?.CompanyName ?? "N/A" },
                { "Year",           DateTime.Now.Year.ToString() }
            };

                            var emailRequest = new EmailRequest
                            {
                                ToEmails = emailReport.ToEmails.Split(',').ToList(),
                                BccEmails = emailReport?.BccEmails?.Split(',').ToList(),
                                CcEmails = emailReport?.CcEmails?.Split(',').ToList(),
                                Subject = $"New Manpower Requisition Request - {createdData.SerialNo}", // ✅ SerialNo use karo
                                TemplateName = "ManpowerRequisitionEmailTemplate.html",
                                Placeholders = placeholders
                            };

                            var emailLogger = new EmailLogger
                            {
                                ToEmail = emailReport.ToEmails,
                                BCCEmail = emailReport?.BccEmails,
                                CCEmail = emailReport?.CcEmails,
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

                var approvalActionResult = await _unitOfWork.ApprovalManagementRepository.ManpowerApprovalRequestLevel(approvalAction);
                if (ApprovalResult.Success <= 0)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = ApprovalResult?.ResponseMessage ?? "Failed to update loan approval status"
                    };
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


    }

}
