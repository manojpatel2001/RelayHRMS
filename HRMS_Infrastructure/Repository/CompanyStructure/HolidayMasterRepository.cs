using HRMS_Core.DbContext;
using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyStructure;
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

        public async Task<VMCommonResult> CreateHolidayMaster(vmCreateHoliayMaster holidayMaster)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC CreateHolidayMaster
                        @HolidayName = {holidayMaster.HolidayName},
                        @State = {holidayMaster.State},
                        @BranchId = {holidayMaster.BranchId},
                        @MultipleHoliday = {holidayMaster.MultipleHoliday},
                        @FromDate = {holidayMaster.FromDate},
                        @ToDate = {holidayMaster.ToDate},
                        @MessageText = {holidayMaster.MessageText},
                        @Holidaycategory = {holidayMaster.Holidaycategory},
                        @RepeatAnnually = {holidayMaster.RepeatAnnually},
                        @HalfDay = {holidayMaster.HalfDay},
                        @PresentCompulsory = {holidayMaster.PresentCompulsory},
                        @SMS = {holidayMaster.SMS},
                        @OptionalHoliday = {holidayMaster.OptionalHoliday},
                        @ApprovalMaxLimit = {holidayMaster.ApprovalMaxLimit},
                        @IsDeleted = {holidayMaster.IsDeleted},
                        @IsEnabled = {holidayMaster.IsEnabled},
                        @IsBlocked = {holidayMaster.IsBlocked},
                        @CreatedDate = {holidayMaster.CreatedDate},
                        @CreatedBy = {holidayMaster.CreatedBy},
                        @UpdatedDate = {holidayMaster.UpdatedDate},
                        @UpdatedBy = {holidayMaster.UpdatedBy},
                        @DeletedDate = {holidayMaster.DeletedDate},
                        @DeletedBy = {holidayMaster.DeletedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult
                {
                    Id = 0,
                };
            }
            catch (Exception ex)
            {
                // Optional: log ex here
                return new VMCommonResult
                {
                    Id = 0,
                };
            }
        }


        public async Task<VMCommonResult> DeleteHolidayMaster(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC DeleteHolidayMaster
                        @HolidayMasterId = {deleteRecordVM.Id},
                        @DeletedBy = {deleteRecordVM.DeletedBy},
                        @DeletedDate = {deleteRecordVM.DeletedDate}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult
                {
                    Id = 0
                };
            }
            catch (Exception ex)
            {
                // Optional: Log ex
                return new VMCommonResult
                {
                    Id = 0
                };
            }
        }

        public async Task<List<vmGetAllHolidayMaster>> GetAllHolidayMaster()
        {
            try
            {
                var result = await _db.Set<vmGetAllHolidayMaster>().FromSqlRaw(@" EXEC GetAllHolidayMaster").ToListAsync();

                // Return an empty list if result is null
                return result ?? new List<vmGetAllHolidayMaster>();
            }
            catch (Exception ex)
            {
                // Optional: Log ex
                return new List<vmGetAllHolidayMaster>();
            }
        }


        public async Task<VMCommonResult> UpdateHolidayMaster(vmCreateHoliayMaster holidayMaster)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC UpdateHolidayMaster
                        @HolidayMasterId = {holidayMaster.HolidayMasterId},
                        @HolidayName = {holidayMaster.HolidayName},
                        @State = {holidayMaster.State},
                        @BranchId = {holidayMaster.BranchId},
                        @MultipleHoliday = {holidayMaster.MultipleHoliday},
                        @FromDate = {holidayMaster.FromDate},
                        @ToDate = {holidayMaster.ToDate},
                        @MessageText = {holidayMaster.MessageText},
                        @Holidaycategory = {holidayMaster.Holidaycategory},
                        @RepeatAnnually = {holidayMaster.RepeatAnnually},
                        @HalfDay = {holidayMaster.HalfDay},
                        @PresentCompulsory = {holidayMaster.PresentCompulsory},
                        @SMS = {holidayMaster.SMS},
                        @OptionalHoliday = {holidayMaster.OptionalHoliday},
                        @ApprovalMaxLimit = {holidayMaster.ApprovalMaxLimit},
                        @UpdatedDate = {holidayMaster.UpdatedDate},
                        @UpdatedBy = {holidayMaster.UpdatedBy}
                        
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult
                {
                    Id = 0
                };
            }
            catch (Exception ex)
            {
                // Optional: Log ex
                return new VMCommonResult
                {
                    Id = 0
                };
            }
        }

    }
}
