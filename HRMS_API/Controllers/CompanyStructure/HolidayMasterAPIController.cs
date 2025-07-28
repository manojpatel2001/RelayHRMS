using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyStructure;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.CompanyStructure
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayMasterAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public HolidayMasterAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllHolidayMaster")]
        public async Task<APIResponse> GetAllHolidayMaster()
        {
            try
            {
                var data = await _unitOfWork.HolidayMasterRepository.GetAllHolidayMaster(new vmCommonGetById { });
                if (data == null || !data.Any())
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No records found"
                    };
                }
                return new APIResponse() { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully" };
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

        [HttpGet("GetByHolidayMasterId/{holidayMasterId}")]
        public async Task<APIResponse> GetByHolidayMasterId(int holidayMasterId)
        {
            try
            {
                var data = await _unitOfWork.HolidayMasterRepository.GetHolidayMasterById(new vmCommonGetById {Id=holidayMasterId });
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
                    ResponseMessage = "Unable to retrieve record, Please try again later!"
                };
            }
        }

        [HttpPost("CreateHolidayMaster")]
        public async Task<APIResponse> CreateHolidayMaster(HolidayMaster holidayMaster)
        {
            try
            {
                if (holidayMaster == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Holiday master details cannot be null" };
                }

                var exists = await _unitOfWork.HolidayMasterRepository.GetAllHolidayMaster(new vmCommonGetById { Title = holidayMaster.HolidayName.ToLower() });

                if (holidayMaster.Holidaycategory != "National")
                {
                    if (exists.Any(x => x.StateId != holidayMaster.StateId))
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{holidayMaster.HolidayName}' already exists." };
                }
                else
                {
                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{holidayMaster.HolidayName}' already exists." };

                }


                var result = await _unitOfWork.HolidayMasterRepository.CreateHoliday(holidayMaster);
                if (result.Id > 0)
                {
                    return new APIResponse() { isSuccess = true, ResponseMessage = "The record has been saved successfully" };

                    }

                    return new APIResponse() { isSuccess = false,  ResponseMessage = "Unable to save record!" };

               
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to add record, Please try again later!"
                };
            }
        }

        [HttpPut("UpdateHolidayMaster")]
        public async Task<APIResponse> UpdateHolidayMaster(HolidayMaster holidayMaster)
        {
            try
            {
                if (holidayMaster == null || holidayMaster.HolidayMasterId == 0)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Holiday master details cannot be null or invalid" };
                }

                var check = await _unitOfWork.HolidayMasterRepository.GetHolidayMasterById(new vmCommonGetById { Id = holidayMaster.HolidayMasterId});
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                var exists = await _unitOfWork.HolidayMasterRepository.GetAllHolidayMaster(new vmCommonGetById { Title = holidayMaster.HolidayName.ToLower() });
                if (holidayMaster.Holidaycategory != "National")
                {
                    if (exists.Any(x => x.StateId != holidayMaster.StateId && x.HolidayMasterId != holidayMaster.HolidayMasterId && x.HolidayName.ToLower() == holidayMaster.HolidayName.ToLower()))
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{holidayMaster.HolidayName}' already exists." };
                }
                else
                {
                    if (exists.Any(x =>  x.HolidayMasterId != holidayMaster.HolidayMasterId && x.HolidayName.ToLower() == holidayMaster.HolidayName.ToLower()))
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{holidayMaster.HolidayName}' already exists." };

                }


                var result = await _unitOfWork.HolidayMasterRepository.UpdateHoliday(holidayMaster);
                if (result.Id > 0)
                {
                    return new APIResponse() { isSuccess = true, ResponseMessage = "The record has been updated successfully" };

                }

                return new APIResponse() { isSuccess = false, ResponseMessage = "Unable to update record" };

            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to update record, Please try again later!"
                };
            }
        }

        [HttpDelete("DeleteHolidayMaster")]
        public async Task<APIResponse> DeleteHolidayMaster(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                if (deleteRecordVM == null || deleteRecordVM.Id == 0)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var check = await _unitOfWork.HolidayMasterRepository.GetHolidayMasterById(new vmCommonGetById { Id = deleteRecordVM.Id });
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };


                var result = await _unitOfWork.HolidayMasterRepository.DeleteHoliday(deleteRecordVM);
                if (result.Id > 0)
                {
                    var deletedHolidayMaster = await _unitOfWork.HolidayMasterRepository.GetAsync(x => x.HolidayMasterId == deleteRecordVM.Id);
                    return new APIResponse() { isSuccess = true, Data = deletedHolidayMaster, ResponseMessage = "The record has been deleted successfully" };
                }

                return new APIResponse() { isSuccess = false, ResponseMessage = "Unable to delete record" };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to delete record, Please try again later!"
                };
            }
        }

        

    }
}
