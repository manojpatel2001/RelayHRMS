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
    public class DesignationRepository : Repository<Designation> , IDesignationRepository
    {
        private readonly HRMSDbContext _db;

        public DesignationRepository(HRMSDbContext db) : base(db) 
        {
            _db = db;
        }

        public async Task<Designation> SoftDelete(DeleteRecordVM DeleteRecord)
        {
            var designation = await _db.Designations.FirstOrDefaultAsync(asd => asd.DesignationId == DeleteRecord.Id);
            if (designation == null)
            {
                return designation;
            }
            else
            {
                designation.IsEnabled = false;
                designation.IsDeleted = true;
                designation.DeletedDate = DateTime.UtcNow;
                designation.DeletedBy = DeleteRecord.DeletedBy;
                return designation;
            }
        }

        public async Task<bool> UpdateDesignation(Designation designation)
        {
            var existingRecord = await _db.Designations.SingleOrDefaultAsync(asd => asd.DesignationId == designation.DesignationId);
            if (existingRecord == null)
            {
                return false;
            }
            existingRecord.DesignationName = designation.DesignationName;
            existingRecord.DesignationCode = designation.DesignationCode;
            existingRecord.SortingNo = designation.SortingNo;
            existingRecord.ManagerialPost = designation.ManagerialPost;
            existingRecord.IsMain = designation.IsMain;
            existingRecord.IsEnabled = designation.IsEnabled;
            existingRecord.Allow_ReimEligibilityAmount = designation.Allow_ReimEligibilityAmount;
            existingRecord.UpdatedBy = designation.UpdatedBy;
            existingRecord.UpdatedDate = DateTime.UtcNow;
            return true;
        }
    }
}
