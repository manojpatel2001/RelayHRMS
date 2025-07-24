using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface.JobMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.JobMaster
{
    public class StateRepository : Repository<State> , IStateRepository
    {
        private readonly HRMSDbContext _db;

        public StateRepository(HRMSDbContext db): base(db)
        {
            _db = db;
        }


        public async Task<VMCommonResult> CreateState(State model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageState
                        @Action = {"CREATE"},
                        @StateName = {model.StateName},
                        @CountryName = {model.CountryName},
                        @PTDeductionType = {model.PTDeductionType},
                        @PTDeductionPeriod = {model.PTDeductionPeriod},
                        @EnrollmentCertificateNo = {model.EnrollmentCertificateNo},
                        @ESICStateCode = {model.ESICStateCode},
                        @ESICRegisteredOfficeAddress = {model.ESICRegisteredOfficeAddress},
                        @ApplicablePTSettingForMale_Female = {model.ApplicablePTSettingForMale_Female},
                        @CreatedBy = {model.CreatedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateState(State model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageState
                        @Action = {"UPDATE"},
                        @StateId = {model.StateId},
                        @StateName = {model.StateName},
                        @CountryName = {model.CountryName},
                        @PTDeductionType = {model.PTDeductionType},
                        @PTDeductionPeriod = {model.PTDeductionPeriod},
                        @EnrollmentCertificateNo = {model.EnrollmentCertificateNo},
                        @ESICStateCode = {model.ESICStateCode},
                        @ESICRegisteredOfficeAddress = {model.ESICRegisteredOfficeAddress},
                        @ApplicablePTSettingForMale_Female = {model.ApplicablePTSettingForMale_Female},
                        @UpdatedBy = {model.UpdatedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteState(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageState
                        @Action = {"DELETE"},
                        @StateId = {deleteRecord.Id},
                        @DeletedBy = {deleteRecord.DeletedBy}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        // Optional: If you have stored procedures for these
        public async Task<List<State>> GetAllStates(vmCommonGetById filters)
        {
            try
            {
                if (string.IsNullOrEmpty(filters.Title))
                {
                    filters.Title = "";
                }
                var result = await _db.Set<State>().FromSqlInterpolated($@"
                    EXEC GetAllStates
                        @IsDeleted = {filters.IsDeleted},
                        @IsEnabled = {filters.IsEnabled},
                          @StateName={filters.Title}
                ").ToListAsync();

                return result;
            }
            catch
            {
                return new List<State>();
            }
        }

        public async Task<State?> GetStateById(vmCommonGetById filter)
        {
            try
            {
                var result = await _db.Set<State>().FromSqlInterpolated($@"
                    EXEC GetStateById
                        @StateId = {filter.Id},
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
