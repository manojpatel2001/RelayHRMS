using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyStructure;
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
                var data = await _unitOfWork.HolidayMasterRepository.GetAllHolidayMaster();
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
                var data = await _unitOfWork.HolidayMasterRepository.GetAsync(x => x.HolidayMasterId == holidayMasterId && x.IsEnabled == true && x.IsDeleted == false);
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
        public async Task<APIResponse> CreateHolidayMaster(vmCreateHoliayMaster holidayMaster)
        {
            try
            {
                if (holidayMaster == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Holiday master details cannot be null" };
                }
                var newData = new List<HolidayMaster>();

                if (holidayMaster.HolidayMasterId == 0)
                {
                   
                    foreach (BrancheAndLimit branch  in holidayMaster.Branches)
                    {
                        holidayMaster.BranchId = branch.BranchId;
                        holidayMaster.ApprovalMaxLimit=branch.ApprovalMaxLimit;
                        var isExists = await _unitOfWork.HolidayMasterRepository.GetAllAsync(asd => asd.HolidayName.ToLower().Trim() == holidayMaster.HolidayName.ToLower().Trim()
                      && asd.State == holidayMaster.State && asd.BranchId == holidayMaster.BranchId && asd.IsEnabled == true && asd.IsDeleted == false);
                        if (isExists.Any())
                        {
                            return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{holidayMaster.HolidayName}' already exists" };
                        }


                        holidayMaster.CreatedDate = DateTime.UtcNow;
                        var result = await _unitOfWork.HolidayMasterRepository.CreateHolidayMaster(holidayMaster);
                        if (result.Id > 0)
                        {
                            var newHolidayMaster = await _unitOfWork.HolidayMasterRepository.GetAsync(x => x.HolidayMasterId == result.Id);
                            newData.Add(newHolidayMaster);
                        }
                    }
                    return new APIResponse() { isSuccess = true, Data = newData, ResponseMessage = "The record has been saved successfully" };

                }
                else
                {
                    var checkValidId = await _unitOfWork.HolidayMasterRepository.GetAsync(x => x.HolidayMasterId == holidayMaster.HolidayMasterId && x.IsEnabled == true && x.IsDeleted == false);
                    if (checkValidId == null)
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = "Please select a valid record" };
                    }


                    var isExists = await _unitOfWork.HolidayMasterRepository.GetAllAsync(asd => asd.HolidayName.ToLower().Trim() == holidayMaster.HolidayName.ToLower().Trim()
                && asd.State == holidayMaster.State && asd.BranchId == holidayMaster.BranchId && asd.HolidayMasterId != holidayMaster.HolidayMasterId && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{holidayMaster.HolidayName}' already exists" };
                    }


                    holidayMaster.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.HolidayMasterRepository.UpdateHolidayMaster(holidayMaster);
                    if (result.Id > 0)
                    {
                        var updatedHolidayMaster = await _unitOfWork.HolidayMasterRepository.GetAsync(x => x.HolidayMasterId == holidayMaster.HolidayMasterId);
                        return new APIResponse() { isSuccess = true, Data = updatedHolidayMaster, ResponseMessage = "The record has been updated successfully" };

                    }

                    return new APIResponse() { isSuccess = false, ResponseMessage = "Unable to update record" };

                }
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
        public async Task<APIResponse> UpdateHolidayMaster(vmCreateHoliayMaster holidayMaster)
        {
            try
            {
                if (holidayMaster == null || holidayMaster.HolidayMasterId == 0)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Holiday master details cannot be null or invalid" };
                }

                var checkValidId = await _unitOfWork.HolidayMasterRepository.GetAsync(x => x.HolidayMasterId == holidayMaster.HolidayMasterId && x.IsEnabled == true && x.IsDeleted == false);
                if (checkValidId == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Please select a valid record" };
                }

                
                    var isExists = await _unitOfWork.HolidayMasterRepository.GetAllAsync(asd => asd.HolidayName.ToLower().Trim() == holidayMaster.HolidayName.ToLower().Trim()
                && asd.State == holidayMaster.State && asd.BranchId == holidayMaster.BranchId && asd.HolidayMasterId != holidayMaster.HolidayMasterId && asd.IsEnabled == true && asd.IsDeleted == false);
                    if (isExists.Any())
                    {
                        return new APIResponse() { isSuccess = false, ResponseMessage = $"Record with name '{holidayMaster.HolidayName}' already exists" };
                    }


                    holidayMaster.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.HolidayMasterRepository.UpdateHolidayMaster(holidayMaster);
                    if (result.Id > 0)
                    {
                        var updatedHolidayMaster = await _unitOfWork.HolidayMasterRepository.GetAsync(x => x.HolidayMasterId == holidayMaster.HolidayMasterId);
                      return new APIResponse() { isSuccess = true, Data = updatedHolidayMaster, ResponseMessage = "The record has been updated successfully" };

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

                var checkValidId = await _unitOfWork.HolidayMasterRepository.GetAsync(x => x.HolidayMasterId == deleteRecordVM.Id && x.IsEnabled == true && x.IsDeleted == false);
                if (checkValidId == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Please select a valid record" };
                }

                deleteRecordVM.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.HolidayMasterRepository.DeleteHolidayMaster(deleteRecordVM);
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
