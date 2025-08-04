using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.EmergencyContacts
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelationShipAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RelationShipAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllRelationShips")]
        public async Task<APIResponse> GetAllRelationShips()
        {
            try
            {
                var data = await _unitOfWork.RelationShipRepository.GetAllRelationShips(new vmCommonGetById { });
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetRelationShipById/{id}")]
        public async Task<APIResponse> GetRelationShipById(int id)
        {
            try
            {
                var data = await _unitOfWork.RelationShipRepository.GetRelationShipById(new vmCommonGetById { Id = id });
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateRelationShip")]
        public async Task<APIResponse> CreateRelationShip(RelationShip model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "RelationShip details cannot be null." };

                var exists = await _unitOfWork.RelationShipRepository.GetAllRelationShips(new vmCommonGetById { Title = model.RelationName.ToLower() });

                if (exists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.RelationName}' already exists." };

                model.CreatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.RelationShipRepository.CreateRelationShip(model);

                if (result.Id > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been added successfully." };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateRelationShip")]
        public async Task<APIResponse> UpdateRelationShip(RelationShip model)
        {
            try
            {
                if (model == null || model.RelationShipId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "RelationShip details cannot be null." };

                var check = await _unitOfWork.RelationShipRepository.GetRelationShipById(new vmCommonGetById { Id = model.RelationShipId });
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                var exists = await _unitOfWork.RelationShipRepository.GetAllRelationShips(new vmCommonGetById { Title = model.RelationName.ToLower() }); ;

                if (exists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.RelationName}' already exists." };

                model.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.RelationShipRepository.UpdateRelationShip(model);

                if (result.Id > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been updated successfully." };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteRelationShip")]
        public async Task<APIResponse> DeleteRelationShip(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var check = await _unitOfWork.RelationShipRepository.GetRelationShipById(new vmCommonGetById { Id = model.Id });
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.RelationShipRepository.DeleteRelationShip(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }
    }
}
