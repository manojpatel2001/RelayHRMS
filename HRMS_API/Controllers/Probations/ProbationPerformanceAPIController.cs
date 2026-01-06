using Azure.Core;
using DocumentFormat.OpenXml.ExtendedProperties;
using HRMS_API.Services;
using HRMS_Core.Migrations;
using HRMS_Core.Probations;
using HRMS_Core.Services;
using HRMS_Core.VM;
using HRMS_Core.VM.ApprovalManagement;
using HRMS_Core.VM.EmailService;
using HRMS_Core.VM.Probations;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Text;

namespace HRMS_API.Controllers.Probations
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProbationPerformanceAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileUploadService _fileUploadService;
        private readonly EmailService _emailService;

        public ProbationPerformanceAPIController(IUnitOfWork unitOfWork, FileUploadService fileUploadService, EmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
            _emailService = emailService;
        }


        [HttpGet("GetAllProbationStatus")]
        public async Task<APIResponse> GetAllProbationStatus()
        {
            try
            {
                var result = await _unitOfWork.ProbationPerformanceRepository.GetAllProbationStatus();
                return result;
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to fetch probation status ."
                };
            }
        }

        [HttpGet("GetAllProbationEvaluationPeriods")]
        public async Task<APIResponse> GetAllProbationEvaluationPeriods([FromQuery] int? ProbationStatusId = null)
        {
            try
            {
                var result = await _unitOfWork.ProbationPerformanceRepository.GetAllProbationEvaluationPeriods(ProbationStatusId);
                return result;
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to fetch probation evaluation period ."
                };
            }
        }



        [HttpGet("GetAllProbationEmployees/{ProbationManagerId}")]
        public async Task<APIResponse> GetAllProbationEmployees(int ProbationManagerId)
        {
            try
            {
                var data = await _unitOfWork.ProbationPerformanceRepository.GetAllProbationEmployees(ProbationManagerId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetEmployeeForProbationByEmployeeId/{employeeId}")]
        public async Task<APIResponse> GetEmployeeForProbationByEmployeeId(int employeeId)
        {
            try
            {
                var data = await _unitOfWork.ProbationPerformanceRepository.GetEmployeeForProbationByEmployeeId(employeeId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateProbationPerformance")]
        public async Task<APIResponse> CreateProbationPerformance([FromBody] ProbationPerformance model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Probation performance details cannot be null." };
                
                var result = await _unitOfWork.ProbationPerformanceRepository.CreateProbationPerformance(model);
                if (result.isSuccess)
                {
                    var approval = new ApprovalRequestLevelActionPara
                    {
                        ApprovalRequestLevelId = model.ApprovalRequestLevelId,
                        ApprovalRequestId = model.ApprovalRequestId,
                        StatusId = model.ProbationStatusId,
                        Remarks = model.RemarksOfApprover == null ? "N/A" : model.RemarksOfApprover,
                        ActionBy = Convert.ToInt32(model.CreatedBy)
                    };

                    var checkApproval = await _unitOfWork.ApprovalManagementRepository.ApprovalRequestLevelAction(approval);

                    if (checkApproval.IsSuccess)
                    {
                        if (model.ProbationStatusId == 3 || model.ProbationStatusId == 4)
                        {
                            var EvaluationTitle = model.ProbationStatusId == 3 ? "Hold" : "Extension";
                            var EvaluationActivity = model.ProbationStatusId == 3 ? "Hold" : "Extended";
                            var EvaluationStatus = model.ProbationStatusId == 3 ? "Hold" : "Extend";


                            var placeholders = new Dictionary<string, string>
                            {
                                { "EmployeeName", model.EmployeeName??"N/A" },
                                { "EmployeeCode", model.EmployeeCode??"N/A" },

                                { "EvaluationTitle", EvaluationTitle },
                                { "EvaluationActivity", EvaluationActivity },
                                { "EvaluationStatus", EvaluationStatus },
                                { "EvaluationDuration", $"{model.EvaluationDays} days " },
                                { "EvaluationRemarks", model.RemarksOfApprover ?? "N/A" },
                                { "StarRating", Helper.GenerateStarRating(model.Rating)},

                                { "ApprovingManagerName", model.ApproverName??"N/A" },
                                { "ApprovingManagerEmployeeCode", model.ApproverCode??"N/A" },

                                { "PreviousProbationEndDate",
                                    model.ProbationEndDate.HasValue
                                        ? model.ProbationEndDate.Value.ToString("dd-MM-yyyy")
                                        : "N/A"
                                },
                                { "NewProbationEndDate",
                                    model.ProbationEvaluationDate.HasValue
                                        ? model.ProbationEvaluationDate.Value.ToString("dd-MM-yyyy")
                                        : "N/A"
                                },

                                { "CompanyName", model.CompanyName?? "N/A" },
                                { "Year", DateTime.Now.Year.ToString() }
                            };

                            var emailReport = await _unitOfWork.EmailReportRepository.GetEmailSendTime(EmailReportType.HoldAndExtendReport.ToString());
                            if (emailReport == null|| string.IsNullOrEmpty(emailReport.ToEmails))
                            {
                                return new APIResponse { isSuccess = result.isSuccess, ResponseMessage = result.ResponseMessage };
                            }

                            var Subject = $"Probation {EvaluationTitle} Notification";
                            var TemplateName = "ProbationEvaluationEmailTemplate.html";

                            var emailRequest = new EmailRequest
                            {
                                ToEmails = emailReport.ToEmails.Split(',').ToList(),
                                BccEmails = emailReport?.BccEmails?.Split(',').ToList(),
                                CcEmails = emailReport?.CcEmails?.Split(',').ToList(),
                                Subject = Subject,
                                TemplateName = TemplateName,
                                Placeholders = placeholders
                            };


                            var reportingEmailLogger = new EmailLogger
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


                            await _unitOfWork.EmailLoggerRepository.ManageEmailLoggerAsync(reportingEmailLogger, "CREATE");

                            //Send the email
                            bool checkStatus = await _emailService.SendEmailAsync(emailRequest);
                        }
                    }

                    //Add notification Logic

                }
                return new APIResponse { isSuccess = result.isSuccess, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
            }
         }

      

       

        //[HttpPost("GetPendingApprovalRequestsWithHistory1")]
        //public async Task<APIResponse> GetPendingApprovalRequestsWithHistory1(GetPendingApprovalRequestsWithHistoryPara1 model)
        //{
        //    try
        //    {
        //        var result = await _unitOfWork.ProbationPerformanceRepository.GetPendingApprovalRequestsWithHistory1(model);

        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        return new APIResponse
        //        {
        //            isSuccess = false,
        //            ResponseMessage = "Unable to fetch up comming probation ."
        //        };
        //    }
        //}

        [HttpPost("GetPendingApprovalRequestsWithHistory")]
        public async Task<APIResponse> GetPendingApprovalRequestsWithHistory(GetPendingApprovalRequestsWithHistoryPara model)
        {
            try
            {
                var result = await _unitOfWork.ProbationPerformanceRepository.GetPendingApprovalRequestsWithHistory(model);

                return result;
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to fetch up comming probation ."
                };
            }
        }

        [HttpPost("GetAllConfirmationProbationDetails")]
        public async Task<APIResponse> GetAllConfirmationProbationDetails(GetAllConfirmationProbationDetailsPara model)
        {
            try
            {
                var result = await _unitOfWork.ProbationPerformanceRepository.GetAllConfirmationProbationDetails(model);
                return new APIResponse { Data = result, isSuccess = true, ResponseMessage = "Successfully fetched!" };
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Unable to fetch up comming probation ."
                };
            }
        }


        [HttpPost("SendProbationConfirmationEmail")]
        public async Task<APIResponse> SendProbationConfirmationEmail(ConfirmationProbationDetailsPara model)
        {
            try
            {
                if (model == null || model.PersonalEmailId == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Employee details cannot be null." };

                var ToEmails = "";
                var Subject = "Probation Confirmation";
                var TemplateName = "ProbationConfirmationEmailTemplate.html";
                if(string.IsNullOrEmpty(model.PersonalEmailId))
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Employee email cannot be null." };

                }
                ToEmails= model.PersonalEmailId;

                if(!string.IsNullOrEmpty(model.PersonalEmailId))
                {
                    ToEmails = $"{ToEmails},{model.ReportingManagerEmail}";
                }

                


                byte[] pdfBytes = Convert.FromBase64String(model.ConfirmationPdf);


                var placeholders = new Dictionary<string, string>
                {
                    {"EmployeeCode", model.EmployeeCode??"N/A"},
                    {"EmployeeName", model.EmployeeName??"N/A"},
                    {"CompanyName", model.CompanyName ?? "N/A"},
                    {"HREmail", "hrd@relayexpress.in"},
                    {"HRPhone", "+91 9979865052"},
                    {"Year", DateTime.Now.Year.ToString()}
                };



                var emailRequest = new EmailRequest
                {
                    ToEmails = ToEmails.Split(',').ToList(),
                   // BccEmails = ToBccEmails?.Split(',').ToList(),
                    Subject = Subject,
                    TemplateName = TemplateName,
                    Placeholders = placeholders,
                    PdfFile = pdfBytes
                };
                var EmailLogger = new EmailLogger
                {
                    ToEmail = ToEmails,
                    //BCCEmail = ToBccEmails,
                    Subject = Subject,
                    Body = TemplateName,
                    Status = EmailStatus.Pending,
                    SentAt = DateTime.UtcNow,
                    Comments = "Email ready for sent"

                };

                await _unitOfWork.EmailLoggerRepository.ManageEmailLoggerAsync(EmailLogger, "CREATE");

                //Send the email
                bool checkStatus = await _emailService.SendEmailAsync(emailRequest);
                if (checkStatus)
                {
                    var updatedMailRequest = await _unitOfWork.ProbationPerformanceRepository.UpdateMailRequest(model.ApprovalRequestId, true);
                    return new APIResponse { isSuccess = true, ResponseMessage = "Mail sent successfully!" };

                }
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to send mail please try again!" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to send email. Please try again later." };
            }
        }



    }
}
