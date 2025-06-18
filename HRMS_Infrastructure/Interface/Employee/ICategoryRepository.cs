using HRMS_Core.Employee;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface ICategoryRepository:IRepository<Category>
    {
        Task<VMCommonResult> CreateCategory(Category category);
        Task<VMCommonResult> UpdateCategory(Category category);
        Task<VMCommonResult> DeleteCategory(DeleteRecordVM category);
        Task<Category?> GetCategoryById(int categoryId);
        Task<List<Category>> GetAllCategories();
    }
}
