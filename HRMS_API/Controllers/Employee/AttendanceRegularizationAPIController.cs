using HRMS_API.NotificationService.HubService;
using HRMS_API.NotificationService.ManageService;
using HRMS_Core.Employee;
using HRMS_Core.Notifications;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Report;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Interface.Employee;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceRegularizationAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;
        public AttendanceRegularizationAPIController(IUnitOfWork unitOfWork, IHubContext<NotificationRemainderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }


        [HttpGet("GetAll")]
        public async Task<APIResponse> GetAll()
        {
            try
            {
                var data = await _unitOfWork.AttendanceRegularizationRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false);
                return new APIResponse() { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrieve records, Please try again later!"
                };
            }
        }
        [HttpGet("GetAttendanceReasonsByLimitType")]
        public async Task<APIResponse> GetAttendanceReasonsByLimitType()
        {
            try
            {
                var data = await _unitOfWork.AttendanceRegularizationRepository.GetAttendanceReasonsByLimitType();
                return new APIResponse() { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrieve records, Please try again later!"
                };
            }
        }


        [HttpGet("GetById/{id}")]
        public async Task<APIResponse> GetById(int Id)
        {
            try
            {
                var data = await _unitOfWork.AttendanceRegularizationRepository.GetAsync(x => x.AttendanceRegularizationId == Id && x.IsEnabled == true && x.IsDeleted == false);
                if (data == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Record not found"
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Record fetched successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrive records, Please try again later!"
                };
            }

        }
        [HttpPost("CreateAttendanceRegularization")]
        public async Task<APIResponse> CreateAttendanceRegularization([FromBody] List<AttendanceRegularization> attendances)
        {
            try
            {
                if (attendances == null || attendances.Count == 0)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No attendance details received."
                    };
                }

                string CreatedBy = "0";
                var attdenceDateList = new List<DateTime>();
                bool finalSuccess = true;
                string finalMessage = string.Empty;

                foreach (var attendance in attendances)
                {
                    if (attendance == null || !attendance.ForDate.HasValue || string.IsNullOrWhiteSpace(attendance.ShiftTime))
                        continue;

                    var exists = await _unitOfWork.AttendanceRegularizationRepository.GetAsync(x =>
                      x.EmpId == attendance.EmpId &&
                      x.ForDate == attendance.ForDate &&
                      x.IsEnabled == true &&
                      x.IsDeleted == false);

                    if (exists != null)
                    {
                        return new APIResponse
                        {
                            isSuccess = false,
                            ResponseMessage = $"Request already exists for EmpId {attendance.EmpId} on {attendance.ForDate:yyyy-MM-dd}"
                        };
                    }

                    // Create attendance and get result from stored procedure
                    var result = await _unitOfWork.AttendanceRegularizationRepository.Create(attendance);

                    // Check if create failed (attendance locked or salary generated)
                    if (!result.isSuccess)
                    {
                        return new APIResponse
                        {
                            isSuccess = false,
                            ResponseMessage = result.ResponseMessage
                        };
                    }

                    // Store success message from repository
                    finalSuccess = result.isSuccess;
                    finalMessage = result.ResponseMessage;

                    attdenceDateList.Add((DateTime)attendance.ForDate);
                    CreatedBy = attendance.CreatedBy;
                }

                //Notification send to reporting person
                var employeeDetails = await _unitOfWork.EmployeeManageRepository.GetEmployeeById(Convert.ToInt32(CreatedBy));
                if (employeeDetails != null)
                {
                    var attdenceDate = string.Join(", ", attdenceDateList.Select(date => date.ToString("dd-MM-yyyy")));
                    var notification = new NotificationRemainders()
                    {
                        NotificationMessage = $"{employeeDetails?.FullName} has requested approval for attendance on Date: {attdenceDate}",
                        NotificationTime = DateTime.UtcNow,
                        SenderId = employeeDetails?.Id.ToString(),
                        ReceiverIds = employeeDetails?.ReportingManagerId.ToString(),
                        NotificationType = NotificationType.AttendanceApplication,
                        NotificationAffectedId = Convert.ToInt32(CreatedBy)
                    };
                    var savedNotification = await _unitOfWork.NotificationRemainderRepository.CreateNotificationRemainder(notification);
                    if (savedNotification.Success > 0)
                    {
                        notification.NotificationRemainderId = savedNotification.Success;
                        var reprtingConnection = NotificationRemainderConnectionManager.GetConnections(employeeDetails?.ReportingManagerId.ToString());
                        if (reprtingConnection.Any())
                        {
                            await _hubContext.Clients.Clients(reprtingConnection).SendAsync("ReceiveNotificationRemainder", notification);
                        }
                    }
                }

                return new APIResponse
                {
                    isSuccess = finalSuccess,
                    Data = attendances,
                    ResponseMessage = !string.IsNullOrEmpty(finalMessage) ? finalMessage : "Records have been saved successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = err.Message
                };
            }
        }
        //[HttpPut("UpdateAttendanceRegularization")]
        //public async Task<APIResponse> UpdateAttendanceRegularization(List<AttendanceRegularization> attendances)
        //{
        //    try
        //    {
        //        if (attendances == null || !attendances.Any())
        //        {
        //            return new APIResponse
        //            {
        //                isSuccess = false,
        //                ResponseMessage = "No attendance records provided."
        //            };
        //        }

        //        foreach (var attendance in attendances)
        //        {
        //            var record = await _unitOfWork.AttendanceRegularizationRepository.GetAsync(x => x.AttendanceRegularizationId == attendance.AttendanceRegularizationId && x.IsEnabled == true && x.IsDeleted == false);
        //            if (record != null)
        //            {                
        //                await _unitOfWork.AttendanceRegularizationRepository.UpdateAttendanceRegularization(record);
        //            }
        //        }

        //        await _unitOfWork.CommitAsync();

        //        return new APIResponse
        //        {
        //            isSuccess = true,
        //            ResponseMessage = "Status updated successfully."
        //        };
        //    }
        //    catch (Exception err)
        //    {
        //        return new APIResponse
        //        {
        //            isSuccess = false,
        //            Data = err.Message,
        //            ResponseMessage = "Unable to update the records, please try again later."
        //        };
        //    }
        //}

        [HttpPut("UpdateAttendanceRegularization")]
        public async Task<APIResponse> UpdateAttendanceRegularization([FromBody] List<AttendanceRegularization> attendances)
        {
            try
            {
                if (attendances == null || !attendances.Any())
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No attendance records provided."
                    };
                }

                bool finalSuccess = true;
                string finalMessage = string.Empty;

                foreach (var attendance in attendances)
                {
                    var record = await _unitOfWork.AttendanceRegularizationRepository.GetAsync(
                        x => x.AttendanceRegularizationId == attendance.AttendanceRegularizationId &&
                             x.IsEnabled == true &&
                             x.IsDeleted == false
                    );
                    if (record == null) continue;

                    // Update status fields
                    record.Status = attendance.Status;
                    record.IsApproved = attendance.Status == "Approved";
                    record.IsRejected = attendance.Status == "Rejected";
                    record.IsPending = attendance.Status == "Pending";

                    record.UpdatedBy = attendance.CreatedBy;
                    record.UpdatedDate = DateTime.UtcNow;

                    // Save changes and get result from stored procedure
                    var result = await _unitOfWork.AttendanceRegularizationRepository.Update(record);

                    // Check if update failed (attendance locked or salary generated)
                    if (!result.isSuccess)
                    {
                        return new APIResponse
                        {
                            isSuccess = false,
                            ResponseMessage = result.ResponseMessage
                        };
                    }

                    // Store success message from repository
                    finalSuccess = result.isSuccess;
                    finalMessage = result.ResponseMessage;

                    // If approved, update or create InOut records
                    if (record.IsApproved)
                    {
                        var existingInOutList = await _unitOfWork.AttendanceRegularizationRepository.GetEmployeeInOut(attendance.EmpId, attendance.ForDate);
                        var existingInOut = existingInOutList.FirstOrDefault();
                        if (existingInOut != null)
                        {
                            int empInOutId = existingInOut.Id;
                            var updateModel = new AttendanceDetailsViewModel
                            {
                                AttendanceDetailsid = empInOutId,
                                EmployeeId = attendance.EmpId,
                                ShiftDate = attendance.ForDate,
                                InTime = attendance.InTime ?? DateTime.Now,
                                OutTime = attendance.OutTime ?? DateTime.Now,
                                WorkingHours = attendance.Duration,
                                SalaryDay = 1,
                                CreatedOn = DateTime.Now
                            };
                            await _unitOfWork.EmployeeInOutRepository.UpdateAttendanceDetails(updateModel);
                        }
                        else
                        {
                            var newInOut = new AttendanceDetailsViewModel
                            {
                                EmployeeId = attendance.EmpId,
                                ShiftDate = attendance.ForDate,
                                InTime = attendance.InTime ?? DateTime.Now,
                                OutTime = attendance.OutTime ?? DateTime.Now,
                                WorkingHours = attendance.Duration,
                                SalaryDay = 1,
                                CreatedOn = DateTime.Now
                            };
                            await _unitOfWork.EmployeeInOutRepository.CreateAttendanceDetails(newInOut);
                            var updateNotification = await _unitOfWork.NotificationRemainderRepository.UpdateNotificationRemainder(
                                new NotificationRemainders
                                {
                                    NotificationType = NotificationType.AttendanceApplication,
                                    SenderId = attendance.EmpId.ToString()
                                }
                            );
                        }
                    }

                    // Send notification to the reporting person
                    var employeeDetails = await _unitOfWork.EmployeeManageRepository.GetEmployeeById(Convert.ToInt32(attendance.CreatedBy));
                    if (employeeDetails != null)
                    {
                        var notification = new NotificationRemainders()
                        {
                            NotificationMessage = $"Your attendance for Date: {attendance.ForDate:dd-MM-yyyy} has been {attendance.Status?.ToLower()} by {employeeDetails?.FullName}.",
                            NotificationTime = DateTime.UtcNow,
                            SenderId = employeeDetails?.Id.ToString(),
                            ReceiverIds = attendance?.EmpId.ToString(),
                            NotificationType = NotificationType.AttendanceApproval,
                            NotificationAffectedId = Convert.ToInt32(attendance?.EmpId)
                        };
                        var savedNotification = await _unitOfWork.NotificationRemainderRepository.CreateNotificationRemainder(notification);
                        if (savedNotification.Success > 0)
                        {
                            notification.NotificationRemainderId = savedNotification.Success;
                            var reportingConnection = NotificationRemainderConnectionManager.GetConnections(attendance?.EmpId.ToString());
                            if (reportingConnection.Any())
                            {
                                await _hubContext.Clients.Clients(reportingConnection).SendAsync("ReceiveNotificationRemainder", notification);
                            }
                        }
                    }
                }

                return new APIResponse
                {
                    isSuccess = finalSuccess,
                    ResponseMessage = !string.IsNullOrEmpty(finalMessage) ? finalMessage : "Attendance and In/Out records processed successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = ex.Message
                };
            }
        }
        [HttpDelete("Delete")]
        public async Task<APIResponse> Delete([FromBody] DeleteRecordVModel DeleteRecord)
        {
            try
            {
                if (DeleteRecord == null)
                {
                    return new APIResponse()
                    {
                        isSuccess = false,
                        ResponseMessage = "Delete details cannot be null"
                    };
                }

                // Delete and get result from stored procedure
                var result = await _unitOfWork.AttendanceRegularizationRepository.Delete(DeleteRecord);

                // Check if delete failed (attendance locked or salary generated)
                if (!result.isSuccess)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = result.ResponseMessage
                    };
                }

                await _unitOfWork.CommitAsync();

                return new APIResponse()
                {
                    isSuccess = result.isSuccess,
                    Data = DeleteRecord,
                    ResponseMessage = !string.IsNullOrEmpty(result.ResponseMessage) ? result.ResponseMessage : "The record has been deleted successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = err.Message
                };
            }
        }

        [HttpPost("GetAttendanceRegularization")]
        public async Task<APIResponse> GetAttendanceRegularization([FromBody] AttendanceRegularizationSearchFilterVM attendance)
        {
            try
            {
                if (attendance == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Emp_Id,Month,Year are required."
                    };
                }


                var data = await _unitOfWork.AttendanceRegularizationRepository.GetAttendanceRegularization(attendance);


                if (data == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No matching IN record found or update failed."
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Data Fetched successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "An error occurred while updating out time."
                };
            }
        }

        [HttpPost("GetAttendanceRegularizationApproval")]
        public async Task<APIResponse> GetAttendanceRegularizationApproval([FromBody] AttendanceRegularizationSearchFilterVM attendance)
        {
            try
            {
                if (attendance == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Emp_Id,Month,Year are required."
                    };
                }


                var data = await _unitOfWork.AttendanceRegularizationRepository.GetAttendanceRegularizationApproval(attendance);


                if (data == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No matching IN record found or update failed."
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Data Fetched successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "An error occurred while updating out time."
                };
            }
        }

        [HttpPost("GetAttendanceRegularizationForAdmin")]
        public async Task<APIResponse> GetAttendanceRegularizationForAdmin([FromBody] AttendanceRegularizationSearchFilterForAdminVM attendance)
        {
            try
            {
                if (attendance == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Attendance details are required."
                    };
                }

                var data = await _unitOfWork.AttendanceRegularizationRepository.GetAttendanceRegularizationForAdmin(attendance);

                if (data == null || data.Count == 0)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No data found.",
                        Data = new { items = new List<AttendanceRegularizationAdmin>(), totalRecords = 0 }
                    };
                }

                int totalRecords = data.FirstOrDefault()?.TotalRecords ?? 0;

                return new APIResponse
                {
                    isSuccess = true,
                    Data = new { items = data, totalRecords = totalRecords },
                    ResponseMessage = "Data fetched successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "An error occurred while fetching data."
                };
            }
        }
        [HttpPost("GetAttendanceRequestAdminReport")]
        public async Task<APIResponse> GetAttendanceRequestAdminReport([FromBody] AttendanceRequestReportFilterVm attendance)
        {
            try
            {
                if (attendance == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Attendance details are required."
                    };
                }

                if (attendance.FromDate.HasValue && attendance.ToDate.HasValue &&
                    attendance.FromDate > attendance.ToDate)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "From Date must be before To Date."
                    };
                }

                var data = await _unitOfWork.AttendanceRegularizationRepository.GetAttendanceRequestAdminReport(attendance);
                if (data == null || !data.Any())
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No matching records found."
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Data fetched successfully."
                };
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using ILogger)
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "An error occurred while fetching data."
                };
            }
        }

        [HttpPost("GetAttendanceDetails")]
        public async Task<APIResponse> GetAttendanceDetails([FromBody] EmployeeInOutFilterVM attendance)
        {
            try
            {
                if (attendance == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Emp_Id,Month,Year are required."
                    };
                }


                var data = await _unitOfWork.AttendanceRegularizationRepository.GetAttendanceDetails(attendance);


                if (data == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No matching IN record found or update failed."
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Data Fetched successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "An error occurred while updating out time."
                };
            }
        }
        [HttpGet("GetEmployeeDetails")]
        public async Task<APIResponse> GetEmployeeDetails(int EmpId)
        {
            try
            {
                if (EmpId == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Emp_Id is required."
                    };
                }


                var data = await _unitOfWork.AttendanceRegularizationRepository.GetEmployeeDetails(EmpId);


                if (data == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No matching IN record found or update failed."
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Data Fetched successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "An error occurred while updating out time."
                };
            }
        }
        [HttpGet("GetEmployeeAttendanceRequestsCountForCurrentMonth")]
        public async Task<APIResponse> GetEmployeeAttendanceRequestsCountForCurrentMonth(int EmpId,int Month,int Year)
        {
            try
            {
                if (EmpId == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Emp_Id is required."
                    };
                }


                var data = await _unitOfWork.AttendanceRegularizationRepository.GetEmployeeAttendanceRequestsCountForCurrentMonth(EmpId,Month,Year);


                if (data == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No matching IN record found or update failed."
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Data Fetched successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "An error occurred while updating out time."
                };
            }
        }

    }
}
