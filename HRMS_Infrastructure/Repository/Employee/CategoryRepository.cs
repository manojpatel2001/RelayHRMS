using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public   class CategoryRepository:Repository<Category>, ICategoryRepository
    {
        private readonly HRMSDbContext _db;

        public CategoryRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateCategory(Category category)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageCategory
                    @Action = {"CREATE"},
                    @CategoryName = {category.CategoryName},
                    @IsDeleted = {category.IsDeleted},
                    @IsEnabled = {category.IsEnabled},
                    @IsBlocked = {category.IsBlocked},
                    @CreatedDate = {category.CreatedDate},
                    @CreatedBy = {category.CreatedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception ex)
            {
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<VMCommonResult> UpdateCategory(Category category)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageCategory
                    @Action = {"UPDATE"},
                    @CategoryId = {category.CategoryId},
                    @CategoryName = {category.CategoryName},
                    @UpdatedDate = {DateTime.UtcNow},
                    @UpdatedBy = {category.UpdatedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception ex)
            {
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<VMCommonResult> DeleteCategory(DeleteRecordVM category)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageCategory
                    @Action = {"DELETE"},
                    @CategoryId = {category.Id},
                    @DeletedDate = {DateTime.UtcNow},
                    @DeletedBy = {category.DeletedBy}").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception ex)
            {
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<Category?> GetCategoryById(int categoryId)
        {
            try
            {
                var result = await _db.Set<Category>()
                                      .FromSqlInterpolated($"EXEC GetCategoryById @CategoryId = {categoryId}")
                                      .ToListAsync();

                return result.FirstOrDefault() ?? null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Category>> GetAllCategories()
        {
            try
            {
                var result = await _db.Set<Category>()
                                      .FromSqlInterpolated($"EXEC GetAllCategories")
                                      .ToListAsync();

                return result ?? new List<Category>();
            }
            catch (Exception ex)
            {
                return new List<Category>();
            }
        }
    }
}
