using HRMS_API.NotificationService.HubService;
using HRMS_API.NotificationService.ManageService;
using HRMS_Core.Employee;
using HRMS_Core.Notifications;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
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
                    await _unitOfWork.AttendanceRegularizationRepository.Create(attendance);

                    attdenceDateList.Add((DateTime)attendance.ForDate);
                    CreatedBy = attendance.CreatedBy;

                }

                //Notification send to reporting persion
                var employeeDetails = await _unitOfWork.EmployeeManageRepository.GetEmployeeById(Convert.ToInt32(CreatedBy));
                if (employeeDetails != null && employeeDetails?.Id != Convert.ToInt32(CreatedBy))
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
                    isSuccess = true,
                    Data = attendances,
                    ResponseMessage = "Records have been saved successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to add records, please try again later!"
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

               
                foreach (var attendance in attendances)
                {
                    var record = await _unitOfWork.AttendanceRegularizationRepository.GetAsync(x => x.AttendanceRegularizationId == attendance.AttendanceRegularizationId && x.IsEnabled == true && x.IsDeleted == false);

                    if (record == null) continue;

                    // 1. Update attendance status
                    record.Status = attendance.Status;
                    record.IsApproved = attendance.Status == "Approved";
                    record.IsRejected = attendance.Status == "Rejected";
                    record.IsPending = attendance.Status == "Pending";

                    await _unitOfWork.AttendanceRegularizationRepository.Update(record);

                    if (record.IsApproved)
                    {
                        var existingInOutList = await _unitOfWork.AttendanceRegularizationRepository.GetEmployeeInOut(attendance.EmpId, attendance.ForDate);

                        var existingInOut = existingInOutList.FirstOrDefault();

                        if (existingInOut != null)
                        {
                            int empInOutId = existingInOut.Id;

                            var updateModel = new AttendanceDetailsViewModel
                            {
                                AttendanceDetailsid= empInOutId,
                                EmployeeId = attendance.EmpId,
                                ShiftDate = attendance.ForDate,
                                InTime = attendance.InTime ?? DateTime.Now,
                                OutTime = attendance.OutTime ?? DateTime.Now,
                                WorkingHours = attendance.Duration,
                                //AttendanceStatus = attendance.Status,
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
                                //AttendanceStatus = attendance.Status,
                                SalaryDay = 1,
                                CreatedOn = DateTime.Now
                            };
                            await _unitOfWork.EmployeeInOutRepository.CreateAttendanceDetails(newInOut);
                            var updateNotification = await _unitOfWork.NotificationRemainderRepository.UpdateNotificationRemainder(new NotificationRemainders { NotificationType= NotificationType.AttendanceApplication,NotificationAffectedId=attendance.EmpId});

                        }

                    }

                   
                    //Notification send to reporting persion
                    var employeeDetails = await _unitOfWork.EmployeeManageRepository.GetEmployeeById(Convert.ToInt32(attendance.CreatedBy));
                    if (employeeDetails != null && employeeDetails?.Id != Convert.ToInt32(attendance.CreatedBy))
                    {
                        var notification = new NotificationRemainders()
                        {
                            NotificationMessage = $" Your attendance for Date: {attendance.ForDate:dd-MM-yyyy} has been {attendance.Status?.ToLower()} by {employeeDetails?.FullName}.",
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
                            var reprtingConnection = NotificationRemainderConnectionManager.GetConnections(attendance?.EmpId.ToString());
                            if (reprtingConnection.Any())
                            {
                                await _hubContext.Clients.Clients(reprtingConnection).SendAsync("ReceiveNotificationRemainder", notification);
                            }
                        }
                    }
                }

                
                return new APIResponse
                {
                    isSuccess = true,
                    ResponseMessage = "Attendance and In/Out records processed successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Something went wrong.",
                    Data = ex.Message
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
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var data = await _unitOfWork.AttendanceRegularizationRepository.Delete(DeleteRecord);
                await _unitOfWork.CommitAsync();

                return new APIResponse() { isSuccess = true, Data = DeleteRecord, ResponseMessage = "The record has been deleted successfully" };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to delete records, Please try again later!"
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
                        ResponseMessage = "attendance details are required."
                    };
                }


                var data = await _unitOfWork.AttendanceRegularizationRepository.GetAttendanceRegularizationForAdmin(attendance);


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

    }
}
