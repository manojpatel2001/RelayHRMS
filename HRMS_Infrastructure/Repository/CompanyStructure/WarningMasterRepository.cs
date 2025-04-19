using HRMS_Core.DbContext;
using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.CompanyStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.CompanyStructure
{
    public class WarningMasterRepository : Repository<WarningMaster>, IWarningMasterRepository
    {
        private HRMSDbContext _db;

        public WarningMasterRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<WarningMaster>> GetAllWarningMaster()
        {
            try
            {
                var result = await _db.Set<WarningMaster>()
                                      .FromSqlInterpolated($"EXEC GetAllWarningMaster")
                                      .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                return new List<WarningMaster>();
            }
        }

        public async Task<WarningMaster?> GetByWarningMasterId(int warningMasterId)
        {
            try
            {
                var result = await _db.Set<WarningMaster>()
                                      .FromSqlInterpolated($"EXEC GetByWarningMasterId @WarningMasterId = {warningMasterId}")
                                      .ToListAsync();

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<VMCommonResult> CreateWarningMaster(WarningMaster warningMaster)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageWarningMaster 
                        @Action = {"CREATE"},
                        @WarningName = {warningMaster.WarningName},
                        @Level = {warningMaster.Level},
                        @Remarks = {warningMaster.Remarks},
                        @DeductionType = {warningMaster.DeductionType},
                        @DeductionTypeValue = {warningMaster.DeductionTypeValue},
                        @IsDeleted = {warningMaster.IsDeleted},
                        @IsEnabled = {warningMaster.IsEnabled},
                        @IsBlocked = {warningMaster.IsBlocked},
                        @CreatedDate = {warningMaster.CreatedDate},
                        @CreatedBy = {warningMaster.CreatedBy},
                        @UpdatedDate = {warningMaster.UpdatedDate},
                        @UpdatedBy = {warningMaster.UpdatedBy},
                        @DeletedDate = {warningMaster.DeletedDate},
                        @DeletedBy = {warningMaster.DeletedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateWarningMaster(WarningMaster warningMaster)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageWarningMaster 
                        @Action = {"UPDATE"},
                        @WarningMasterId = {warningMaster.WarningMasterId},
                        @WarningName = {warningMaster.WarningName},
                        @Level = {warningMaster.Level},
                        @Remarks = {warningMaster.Remarks},
                        @DeductionType = {warningMaster.DeductionType},
                        @DeductionTypeValue = {warningMaster.DeductionTypeValue},
                        @UpdatedDate = {warningMaster.UpdatedDate},
                        @UpdatedBy = {warningMaster.UpdatedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteWarningMaster(DeleteRecordVM deleteRecordVM)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageWarningMaster 
                        @Action = {"DELETE"},
                        @WarningMasterId = {deleteRecordVM.Id},
                        @DeletedDate = {deleteRecordVM.DeletedDate},
                        @DeletedBy = {deleteRecordVM.DeletedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

    }
}
