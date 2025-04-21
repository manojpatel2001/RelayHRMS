using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.JobMaster;
using HRMS_Infrastructure.Interface.JobMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.JobMaster
{
    public class CityRepository : Repository<City> ,ICityRepository
    {
        private readonly HRMSDbContext _db;

        public CityRepository(HRMSDbContext db) : base(db) 
        {
            _db = db;
        }

        public async Task<List<vmGetAllCity>> GetAllCity()
        {
            try
            {
                return await _db.Set<vmGetAllCity>().FromSqlInterpolated($"EXEC GetAllCity").ToListAsync();
            }
            catch
            {
                return new List<vmGetAllCity>();
            }
        }

        public async Task<vmGetAllCity?> GetByCityId(int cityId)
        {
            try
            {
                var result = await _db.Set<vmGetAllCity>()
                    .FromSqlInterpolated($"EXEC GetByCityId @CityID = {cityId}").ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task<VMCommonResult> CreateCity(City city)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageCity 
                        @Action = {"CREATE"},
                        @CityName = {city.CityName},
                        @StateId = {city.StateId},
                        @CityCategoryId = {city.CityCategoryId},
                        @Remarks = {city.Remarks},
                        @Country = {city.Country},
                        @IsDeleted = {city.IsDeleted},
                        @IsEnabled = {city.IsEnabled},
                        @IsBlocked = {city.IsBlocked},
                        @CreatedDate = {city.CreatedDate},
                        @CreatedBy = {city.CreatedBy},
                        @UpdatedDate = {city.UpdatedDate},
                        @UpdatedBy = {city.UpdatedBy},
                        @DeletedDate = {city.DeletedDate},
                        @DeletedBy = {city.DeletedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateCity(City city)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageCity 
                        @Action = {"UPDATE"},
                        @CityID = {city.CityID},
                        @CityName = {city.CityName},
                        @StateId = {city.StateId},
                        @CityCategoryId = {city.CityCategoryId},
                        @Country = {city.Country},
                        @Remarks = {city.Remarks},
                        @UpdatedDate = {city.UpdatedDate},
                        @UpdatedBy = {city.UpdatedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteCity(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageCity 
                        @Action = {"DELETE"},
                        @CityID = {deleteRecordVM.Id},
                        @DeletedDate = {deleteRecordVM.DeletedDate},
                        @DeletedBy = {deleteRecordVM.DeletedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

    }
}
