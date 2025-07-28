using HRMS_Core.Employee;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Interface.Employee;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceRegularizationAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceRegularizationAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        public async Task<APIResponse> CreateAttendanceRegularization(List<AttendanceRegularization> attendances)
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

                foreach (var attendance in attendances)
                {
                    if (attendance == null || !attendance.ForDate.HasValue || string.IsNullOrWhiteSpace(attendance.ShiftTime))
                        continue;

                    attendance.CreatedDate = DateTime.UtcNow;

                    TimeSpan shiftStart, shiftEnd;
                    string[] delimiters = new[] { "-", "to", "TO" };
                    var parts = attendance.ShiftTime.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length != 2 ||
                        !TimeSpan.TryParse(parts[0].Trim(), out shiftStart) ||
                        !TimeSpan.TryParse(parts[1].Trim(), out shiftEnd) ||
                        shiftStart == TimeSpan.Zero || shiftEnd == TimeSpan.Zero)
                    {
                        continue; 
                    }
                    DateTime baseDate = attendance.ForDate.Value.Date;
                    DateTime? inTime = null, outTime = null;

                    TimeSpan shiftDuration = shiftEnd - shiftStart;
                    TimeSpan halfShift = TimeSpan.FromMinutes(shiftDuration.TotalMinutes / 2);

                    switch (attendance.Day?.Trim().ToLower())
                    {
                        case "full day":
                            inTime = baseDate.Add(shiftStart);
                            outTime = baseDate.Add(shiftEnd);
                            break;

                        case "first half":
                            inTime = baseDate.Add(shiftStart);
                            outTime = baseDate.Add(shiftStart + halfShift);
                            break;

                        case "second half":
                            inTime = baseDate.Add(shiftStart + halfShift);
                            outTime = baseDate.Add(shiftEnd);
                            break;

                        default:
                            continue; 
                    }

                    attendance.InTime = inTime;
                    attendance.OutTime = outTime;
                    if (attendance.InTime.HasValue && attendance.OutTime.HasValue)
                    {
                        var duration = attendance.OutTime.Value - attendance.InTime.Value;
                        if (duration.TotalMinutes > 0)
                            attendance.Duration = duration;
                    }

                    await _unitOfWork.AttendanceRegularizationRepository.AddAsync(attendance);
                }

                await _unitOfWork.CommitAsync();

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

                    await _unitOfWork.AttendanceRegularizationRepository.UpdateAttendanceRegularization(record);

                    if (record.IsApproved)
                    {
                        //var existingInOutList = await _unitOfWork.EmployeeInOut
                        //    .GetAllAsync(x => x.Emp_Id == record.EmpId && x.For_Date == record.ForDate);
                          var existingInOutList = await _unitOfWork.AttendanceRegularizationRepository.GetEmployeeInOut(attendance.EmpId , attendance.ForDate);

                        var existingInOut = existingInOutList.FirstOrDefault();

                        if (existingInOut != null)
                        {
                            int empInOutId = existingInOut.LastRecordId;
                            await _unitOfWork.AttendanceRegularizationRepository.Update(attendance , empInOutId);
                        }
                        else
                        {
                            var newInOut = new EmployeeInOutRecord
                            {
                                Emp_Id = record.EmpId ?? 0,
                                For_Date = record.ForDate ?? DateTime.Now,
                                In_Time = record.InTime,
                                Out_Time = record.OutTime,
                                Reason = record.Reason,
                              //  Duration = record.Duration,
                                CreatedDate = DateTime.Now,
                                CreatedBy = ""
                            };

                            await _unitOfWork.EmployeeInOut.AddAsync(newInOut);
                        }
                    }
                }

                await _unitOfWork.CommitAsync();

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

                var data = await _unitOfWork.AttendanceRegularizationRepository.SoftDelete(DeleteRecord);
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

    }
}
