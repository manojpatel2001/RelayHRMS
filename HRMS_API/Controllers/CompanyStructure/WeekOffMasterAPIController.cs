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
    public class WeekOffMasterAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public WeekOffMasterAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("GetAllWeekOffDetails")]
        public async Task<APIResponse> GetAllWeekOffDetails(vmCommonParameters vmCommonParameters)
        {
            try
            {
                var data = await _unitOfWork.WeekOffMasterRepository.GetAllWeekOffDetails(vmCommonParameters);
                if (data == null || !data.Any())
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found" };
                }

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Unable to retrieve records, please try again later!"
                };
            }
        }

        [HttpGet("GetAllWeekOffDetailsByBranchId/{BranchId}")]
        public async Task<APIResponse> GetAllWeekOffDetailsByBranchId(int BranchId)
        {
            try
            {
                var data = await _unitOfWork.WeekOffMasterRepository.GetAllAsync(x=>x.BranchId== BranchId&&x.IsDeleted==false&&x.IsEnabled==true);
                if (data == null || !data.Any())
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found" };
                }

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Unable to retrieve records, please try again later!"
                };
            }
        }

        [HttpGet("GetByWeekOffDetailsId/{WeekOffDetailsId}")]
        public async Task<APIResponse> GetByWeekOffDetailsId(int WeekOffDetailsId)
        {
            try
            {
                var data = await _unitOfWork.WeekOffMasterRepository.GetAsync(x => x.BranchId == WeekOffDetailsId && x.IsDeleted == false && x.IsEnabled == true);
                if (data == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found" };
                }

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Unable to retrieve records, please try again later!"
                };
            }
        }

        [HttpPost("CreateWeekOffDetails")]
        public async Task<APIResponse> CreateWeekOffDetails(VmWeekOffMaster weekOffDetails)
        {
            try
            {
                if (weekOffDetails == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Week off details cannot be null" };

                foreach (var item in weekOffDetails.WeekOff)
                {
                    if (item.WeekOffDetailsId == 0)
                    {
                        var weekOff = new WeekOffDetails
                        {
                            BranchId= weekOffDetails.BranchId,
                            CompanyId= weekOffDetails.CompanyId,
                            WeekOffDay=item.WeekOffDay,
                            WeekOffName=item.WeekOffName,
                            CreatedBy=weekOffDetails.CreatedBy,
                        };
                        var result = await _unitOfWork.WeekOffMasterRepository.CreateWeekOffDetails(weekOff);

                    }
                    else
                    {
                        var checkValidId = await _unitOfWork.WeekOffMasterRepository.GetAsync(asd => asd.WeekOffDetailsId == item.WeekOffDetailsId && asd.IsEnabled == true && asd.IsDeleted == false);
                        if (checkValidId != null)
                        {
                            var weekOff = new WeekOffDetails
                            {
                                WeekOffDetailsId = (int)item.WeekOffDetailsId,
                                BranchId = weekOffDetails.BranchId,
                                CompanyId = weekOffDetails.CompanyId,
                                WeekOffDay = item.WeekOffDay,
                                WeekOffName = item.WeekOffName,
                                UpdatedBy= weekOffDetails.UpdatedBy
                            };
                            var result = await _unitOfWork.WeekOffMasterRepository.UpdateWeekOffDetails(weekOff);

                        }


                        

                    }

                }
                return new APIResponse { isSuccess = true, ResponseMessage = "The record has been saved successfully" };

            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Unable to add records, please try again later!"
                };
            }
        }

        [HttpPut("UpdateWeekOffDetails")]
        public async Task<APIResponse> UpdateWeekOffDetails(WeekOffDetails weekOffDetails)
        {
            try
            {
                if (weekOffDetails == null || weekOffDetails.WeekOffDetailsId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Week off details cannot be null" };

                var checkValidId = await _unitOfWork.WeekOffMasterRepository.GetAsync(asd => asd.WeekOffDetailsId == weekOffDetails.WeekOffDetailsId && asd.IsEnabled == true && asd.IsDeleted == false);
                if (checkValidId == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Please select valid record" };
                }

                weekOffDetails.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.WeekOffMasterRepository.UpdateWeekOffDetails(weekOffDetails);

                if (result.Id > 0)
                {
                    var updatedWeekOffDetails = await _unitOfWork.WeekOffMasterRepository.GetAsync(asd => asd.WeekOffDetailsId == weekOffDetails.WeekOffDetailsId);

                    return new APIResponse { isSuccess = true, Data = updatedWeekOffDetails, ResponseMessage = "The record has been updated successfully" };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record" };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Unable to update records, please try again later!"
                };
            }
        }

        [HttpDelete("DeleteWeekOffDetails")]
        public async Task<APIResponse> DeleteWeekOffDetails(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                if (deleteRecordVM == null || deleteRecordVM.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null" };

                var checkValidId = await _unitOfWork.WeekOffMasterRepository.GetAsync(asd => asd.WeekOffDetailsId == deleteRecordVM.Id && asd.IsEnabled == true && asd.IsDeleted == false);
                if (checkValidId == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = $"Please select valid record" };
                }
                deleteRecordVM.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.WeekOffMasterRepository.DeleteWeekOffDetails(deleteRecordVM);

                if (result.Id > 0)
                {
                    var deletedWeekOffDetails = await _unitOfWork.WeekOffMasterRepository.GetAsync(asd => asd.WeekOffDetailsId == deleteRecordVM.Id);
                    return new APIResponse { isSuccess = true, Data = deletedWeekOffDetails, ResponseMessage = "The record has been deleted successfully" };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record" };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Unable to delete records, please try again later!"
                };
            }



        }
    }
}
