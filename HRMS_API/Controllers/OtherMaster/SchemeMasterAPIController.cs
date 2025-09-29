using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.OtherMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemeMasterAPIController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public SchemeMasterAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("GetAllSchemeMaster")]
        public async Task<APIResponse> GetAllSchemeMaster()
        {
            try
            {
                var data = await _unitOfWork.SchemeMasterRepository.GetAllSchemeMaster();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }



        [HttpGet("GetBySchemeId/{id}")]
        public async Task<APIResponse> GetBySchemeId(int id)
        {
            try
            {
                var data = await _unitOfWork.SchemeMasterRepository.GetBySchemeId(id);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }


        [HttpPost("AddScheme")]
        public async Task<APIResponse> AddScheme([FromBody] SchemeMasterViewModel model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "LeftEmployee details cannot be null." };

                var result = await _unitOfWork.SchemeMasterRepository.AddSchemeMaster(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };

            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateScheme")]
        public async Task<APIResponse> UpdateScheme([FromBody] SchemeMasterViewModel employee)
        {
            try
            {
                if (employee == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Employee details cannot be null" };
                }
                var result = await _unitOfWork.SchemeMasterRepository.UpdateSchemeMaster(employee);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
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

        [HttpDelete("Delete")]
        public async Task<APIResponse> Delete(DeleteRecordVM DeleteRecord)
        {
            try
            {
                if (DeleteRecord == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var result = await _unitOfWork.SchemeMasterRepository.DeleteSchemeMaster(DeleteRecord);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
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

    }
}
