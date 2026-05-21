using HRMS_Core.DbContext;
using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyStructure;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface.CompanyStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.CompanyStructure
{
    public class HolidayMasterRepository:Repository<HolidayMaster>, IHolidayMasterRepository
    {
        private HRMSDbContext _db;

        public HolidayMasterRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VMCommonResult> CreateHoliday(HolidayMaster model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageHolidayMaster
                    @Action = {"CREATE"},
                    @HolidayName = {model.HolidayName},
                    @StateId = {model.StateId},
                    @MultipleHoliday = {model.MultipleHoliday},
                    @FromDate = {model.FromDate},
                    @ToDate = {model.ToDate},
                    @MessageText = {model.MessageText},
                    @HolidayCategory = {model.Holidaycategory},
                    @RepeatAnnually = {model.RepeatAnnually},
                    @CompanyId ={model.CompanyId},
                    @IsActive = {model.IsActive},
                    @CreatedBy = {model.CreatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateHoliday(HolidayMaster model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageHolidayMaster
                    @Action = {"UPDATE"},
                    @HolidayMasterId = {model.HolidayMasterId},
                    @HolidayName = {model.HolidayName},
                    @StateId = {model.StateId},
                    @MultipleHoliday = {model.MultipleHoliday},
                    @FromDate = {model.FromDate},
                    @ToDate = {model.ToDate},
                    @MessageText = {model.MessageText},
                    @HolidayCategory = {model.Holidaycategory},
                    @RepeatAnnually = {model.RepeatAnnually},
                    @CompanyId ={model.CompanyId},
                    @IsActive = {model.IsActive},
                    @UpdatedBy = {model.UpdatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteHoliday(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManageHolidayMaster
                    @Action = {"DELETE"},
                    @HolidayMasterId = {deleteRecord.Id},
                    @DeletedBy = {deleteRecord.DeletedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<List<vmGetAllHolidayMaster>> GetAllHolidayMaster(vmCommonGetById filters)
        {
            try
            {
                var result = await _db.Set<vmGetAllHolidayMaster>().FromSqlInterpolated($@"
                EXEC GetAllHolidayMaster
                     @HolidayName={filters.Title},
                    @IsDeleted = {filters.IsDeleted},
                    @IsEnabled = {filters.IsEnabled},
                    @CompanyId={filters.Id}
            ").ToListAsync();

                return result;
            }
            catch
            {
                return new List<vmGetAllHolidayMaster>();
            }
        }

        public async Task<vmGetAllHolidayMaster?> GetHolidayMasterById(vmCommonGetById filter)
        {
            try
            {
                var result = await _db.Set<vmGetAllHolidayMaster>().FromSqlInterpolated($@"
                EXEC GetHolidayMasterById
                    @HolidayMasterId = {filter.Id},
                    @IsDeleted = {filter.IsDeleted},
                    @IsEnabled = {filter.IsEnabled}
            ").ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

    }
}
