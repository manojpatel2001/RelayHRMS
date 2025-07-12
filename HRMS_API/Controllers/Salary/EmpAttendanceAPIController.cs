using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.importData;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Salary
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpAttendanceAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmpAttendanceAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllEmpAttendance")]
        public async Task<APIResponse> GetAllEmpAttendance()
        {
            try
            {
                var data = await _unitOfWork.EmpAttendanceRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false);
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


        [HttpGet("GetByEmpAttendanceId/{id}")]
        public async Task<APIResponse> GetByEmpAttendanceId(int EmpAttendanceId)
        {
            try
            {
                var data = await _unitOfWork.EmpAttendanceRepository.GetAsync(x => x.EmpAttendanceId == EmpAttendanceId && x.IsEnabled == true && x.IsDeleted == false);
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


        [HttpPost("CreateEmpAttendance")]
        public async Task<APIResponse> CreateEmpAttendance(EmpAttendanceImport empAttendance)
        {
            try
            {
                if (empAttendance == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "empAttendance details cannot be null" };
                }

                empAttendance.CreatedDate = DateTime.UtcNow;
                await _unitOfWork.EmpAttendanceRepository.AddAsync(empAttendance);
                await _unitOfWork.CommitAsync();

                return new APIResponse() { isSuccess = true, Data = empAttendance, ResponseMessage = "The record has been saved successfully" };

            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to add records, Please try again later!"
                };
            }
        }

        [HttpPut("UpdateEmpAttendance")]
        public async Task<APIResponse> UpdateEmpAttendance(EmpAttendanceImport empAttendanceImport)
        {
            try
            {
                if (empAttendanceImport == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Invalid earning details provided."
                    };
                }

                await _unitOfWork.EmpAttendanceRepository.UpdateEmpAttendance(empAttendanceImport);
                await _unitOfWork.CommitAsync();

                return new APIResponse
                {
                    isSuccess = true,
                    Data = empAttendanceImport,
                    ResponseMessage = "The record has been updated successfully."
                };
            }
            catch (Exception err)
            {
                // Optionally log the exception here

                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to update the record, please try again later."
                };
            }
        }


        [HttpDelete("Delete")]
        public async Task<APIResponse> Delete(DeleteRecordVM DeleteRecord)
        {
            try
            {
                if (DeleteRecord == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var data = await _unitOfWork.EmpAttendanceRepository.SoftDelete(DeleteRecord);
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


        [HttpPost("GetEmpAttendance")]
        public async Task<APIResponse> GetEmpAttendance(SearchFilterModel searchFilter)
        {
            try
            {
                var data = await _unitOfWork.EmpAttendanceRepository.GetEmpAttendanceDataAsync(searchFilter);

                return new APIResponse()
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
                    Data = null,
                    ResponseMessage = $"Error: {err.Message}"
                };
            }
        }
    }
}
