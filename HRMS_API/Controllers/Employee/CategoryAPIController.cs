using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllCategories")]
        public async Task<APIResponse> GetAllCategories()
        {
            try
            {
                var data = await _unitOfWork.CategoryRepository.GetAllCategories();
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetCategoryById/{categoryId}")]
        public async Task<APIResponse> GetCategoryById(int categoryId)
        {
            try
            {
                var data = await _unitOfWork.CategoryRepository.GetCategoryById(categoryId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false,  ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateCategory")]
        public async Task<APIResponse> CreateCategory(Category model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Category details cannot be null." };

                if (model.CategoryId == 0)
                {
                    var exists = await _unitOfWork.CategoryRepository.GetAllAsync(c =>
                        c.CategoryName.ToLower().Trim() == model.CategoryName.ToLower().Trim() &&
                        c.IsDeleted == false && c.IsEnabled == true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.CategoryName}' already exists." };

                    model.CreatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.CategoryRepository.CreateCategory(model);

                    if (result.Id > 0)
                    {
                        var newCategory = await _unitOfWork.CategoryRepository.GetCategoryById((int)result.Id);
                        return new APIResponse { isSuccess = true, Data = newCategory, ResponseMessage = "The record has been added successfully." };
                    }

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
                }
                else
                {
                    var check = await _unitOfWork.CategoryRepository.GetCategoryById(model.CategoryId);
                    if (check == null)
                        return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid Category record." };

                    var exists = await _unitOfWork.CategoryRepository.GetAllAsync(c =>
                        c.CategoryId != model.CategoryId &&
                        c.CategoryName.ToLower().Trim() == model.CategoryName.ToLower().Trim() &&
                        c.IsDeleted == false && c.IsEnabled == true);

                    if (exists.Any())
                        return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{model.CategoryName}' already exists." };

                    model.UpdatedDate = DateTime.UtcNow;
                    var result = await _unitOfWork.CategoryRepository.UpdateCategory(model);

                    if (result.Id > 0)
                    {
                        var updated = await _unitOfWork.CategoryRepository.GetCategoryById(model.CategoryId);
                        return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully." };
                    }

                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateCategory")]
        public async Task<APIResponse> UpdateCategory(Category category)
        {
            try
            {
                if (category == null || category.CategoryId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Category details cannot be null." };

                var check = await _unitOfWork.CategoryRepository.GetCategoryById(category.CategoryId);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid Category record." };

                var isExists = await _unitOfWork.CategoryRepository.GetAllAsync(x =>
                    x.CategoryId != category.CategoryId &&
                    x.CategoryName.ToLower().Trim() == category.CategoryName.ToLower().Trim() &&
                    x.IsEnabled == true &&
                    x.IsDeleted == false);

                if (isExists.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = $"Record with name '{category.CategoryName}' already exists." };

                category.UpdatedDate = DateTime.UtcNow;
                var result = await _unitOfWork.CategoryRepository.UpdateCategory(category);

                if (result.Id > 0)
                {
                    var updated = await _unitOfWork.CategoryRepository.GetCategoryById(category.CategoryId);
                    return new APIResponse { isSuccess = true, Data = updated, ResponseMessage = "The record has been updated successfully." };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update Category. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteCategory")]
        public async Task<APIResponse> DeleteCategory(DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var check = await _unitOfWork.CategoryRepository.GetCategoryById(model.Id);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid Category record." };

                model.DeletedDate = DateTime.UtcNow;
                var result = await _unitOfWork.CategoryRepository.DeleteCategory(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "The record has been deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete Category. Please try again later." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }
    }
}
