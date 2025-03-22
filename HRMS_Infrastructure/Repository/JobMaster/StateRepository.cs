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
    public class StateRepository : Repository<State> , IStateRepository
    {
        private readonly HRMSDbContext _db;

        public StateRepository(HRMSDbContext db) : base(db) 
        {
            _db = db;
        }

        public async Task<State> SoftDelete(DeleteRecordVM DeleteRecord)
        {
            var state = await _db.States.FirstOrDefaultAsync(asd => asd.StateId == DeleteRecord.Id);
            if (state == null)
            {
                return state;
            }
            else
            {
                state.IsEnabled = false;
                state.IsDeleted = true;
                state.DeletedDate = DateTime.UtcNow;
                state.DeletedBy = DeleteRecord.DeletedBy;
                return state;
            }
        }

        public async Task<bool> UpdateState(State state)
        {
            var existingRecord = await _db.States.SingleOrDefaultAsync(asd => asd.StateId == state.StateId);
            if (existingRecord == null)
            {
                return false;
            }
            existingRecord.StateName = state.StateName;
            existingRecord.CountryName = state.CountryName;
            existingRecord.PTDeductionType = state.PTDeductionType;
            existingRecord.PTDeductionPeriod = state.PTDeductionPeriod;
            existingRecord.EnrollmentCertificateNo = state.EnrollmentCertificateNo;
            existingRecord.ESICStateCode = state.ESICStateCode;
            existingRecord.ESICRegisteredOfficeAddress = state.ESICRegisteredOfficeAddress;
            existingRecord.ApplicablePTSettingForMale_Female = state.ApplicablePTSettingForMale_Female;
            existingRecord.UpdatedBy = state.UpdatedBy;
            existingRecord.UpdatedDate = DateTime.UtcNow;
            return true;
        }
    }
}
