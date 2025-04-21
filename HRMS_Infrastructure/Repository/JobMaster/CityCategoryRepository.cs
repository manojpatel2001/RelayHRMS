using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.JobMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.JobMaster
{
    public class CityCategoryRepository : Repository<CityCategory>, ICityCategoryRepository
    {
        private readonly HRMSDbContext _db;

        public CityCategoryRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<CityCategory>> GetAllCityCategory()
        {
            try
            {
                return await _db.Set<CityCategory>()
                                .FromSqlInterpolated($"EXEC GetAllCityCategory")
                                .ToListAsync();
            }
            catch (Exception)
            {
                return new List<CityCategory>();
            }
        }

        public async Task<CityCategory?> GetByCityCategoryId(int categoryId)
        {
            try
            {
                var result = await _db.Set<CityCategory>()
                                      .FromSqlInterpolated($"EXEC GetByCityCategoryId @CityCategoryId = {categoryId}")
                                      .ToListAsync();

                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<VMCommonResult> CreateCityCategory(CityCategory category)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
            EXEC ManageCityCategory 
                @Action = {"CREATE"},
                @CityCategoryName = {category.CityCategoryName},
                @Description = {category.Description},
                @IsDeleted = {category.IsDeleted},
                @IsEnabled = {category.IsEnabled},
                @IsBlocked = {category.IsBlocked},
                @CreatedDate = {category.CreatedDate},
                @CreatedBy = {category.CreatedBy},
                @UpdatedDate = {category.UpdatedDate},
                @UpdatedBy = {category.UpdatedBy},
                @DeletedDate = {category.DeletedDate},
                @DeletedBy = {category.DeletedBy}
        ").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateCityCategory(CityCategory category)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageCityCategory 
                    @Action = {"UPDATE"},
                    @CityCategoryId = {category.CityCategoryId},
                    @CityCategoryName = {category.CityCategoryName},
                @Description = {category.Description},
                    @UpdatedDate = {category.UpdatedDate},
                    @UpdatedBy = {category.UpdatedBy}
            ").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteCityCategory(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageCityCategory 
                    @Action = {"DELETE"},
                    @CityCategoryId = {deleteRecordVM.Id},
                    @DeletedDate = {deleteRecordVM.DeletedDate},
                    @DeletedBy = {deleteRecordVM.DeletedBy}
            ").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }
    }

}
