using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.Salary;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Salary
{
    public class EarningRepository : Repository<Earning>, IEarningRepository
    {
        private readonly HRMSDbContext _db;

        public EarningRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Earning> SoftDelete(DeleteRecordVM DeleteRecord)
        {
            var earning = await _db.Earning.FirstOrDefaultAsync(asd => asd.EarningId == DeleteRecord.Id);
            if (earning == null)
            {
                return earning;
            }
            else
            {
                earning.IsEnabled = false;
                earning.IsDeleted = true;
                earning.DeletedDate = DateTime.UtcNow;
                earning.DeletedBy = DeleteRecord.DeletedBy;
                return earning;
            }
        }

        public async Task<bool> UpdateEarning(Earning earning)
        {
            var existingRecord = await _db.Earning.SingleOrDefaultAsync(asd => asd.EarningId == earning.EarningId);
            if (existingRecord == null)
            {
                return false;
            }
            existingRecord.EmployeeId = earning.EmployeeId;
            existingRecord.Basic = earning.Basic;
            existingRecord.HRA = earning.HRA;
            existingRecord.Conveyance = earning.Conveyance;
            existingRecord.Medical = earning.Medical;
            existingRecord.Deputation = earning.Deputation;
            existingRecord.UpdatedBy = earning.UpdatedBy;
            existingRecord.UpdatedDate = DateTime.UtcNow;
            return true;
        }
    }
}
