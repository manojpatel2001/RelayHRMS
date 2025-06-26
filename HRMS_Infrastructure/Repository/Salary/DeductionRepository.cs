using HRMS_Core.DbContext;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.Salary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Salary
{
    public class DeductionRepository : Repository<Deduction>, IDeductionRepository
    {
        private HRMSDbContext _db;

        public DeductionRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Deduction> SoftDelete(DeleteRecordVM DeleteRecord)
        {
            var deduction = await _db.Deduction.FirstOrDefaultAsync(asd => asd.DeductionId == DeleteRecord.Id);
            if (deduction == null)
            {
                return deduction;
            }
            else
            {
                deduction.IsEnabled = false;
                deduction.IsDeleted = true;
                deduction.DeletedDate = DateTime.UtcNow;
                deduction.DeletedBy = DeleteRecord.DeletedBy;
                return deduction;
            }
        }

        public async Task<bool> UpdateDeduction(Deduction deduction)
        {
            var existingRecord = await _db.Deduction.SingleOrDefaultAsync(asd => asd.DeductionId == deduction.DeductionId);
            if (existingRecord == null)
            {
                return false;
            }
            existingRecord.EmployeeId = deduction.EmployeeId;
            existingRecord.PF = deduction.PF;
            existingRecord.ESIC = deduction.ESIC;
            existingRecord.PT = deduction.PT;
            existingRecord.Insurance = deduction.Insurance;
            existingRecord.LWF = deduction.LWF;
            existingRecord.TDS = deduction.TDS;
            existingRecord.UpdatedBy = deduction.UpdatedBy;
            existingRecord.UpdatedDate = DateTime.UtcNow;
            return true;
        }
    }
}
