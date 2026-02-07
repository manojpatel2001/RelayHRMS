using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.importData;
using HRMS_Infrastructure.Interface.Salary;
using Microsoft.Data.SqlClient;
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

        public async Task<List<GetAllDeductionData>> GetDeductionDataAsync(SearchFilterModel filter)
        {
            try
            {
                var monthParam = new SqlParameter("@Month", (object?)filter.Month ?? DBNull.Value);
                var yearParam = new SqlParameter("@Year", (object?)filter.Year ?? DBNull.Value);
                var empCodeParam = new SqlParameter("@EmpCode", (object?)filter.EmpCode ?? DBNull.Value);
                var branchidParam = new SqlParameter("@BranchId", (object?)filter.BranchId ?? DBNull.Value);
                var startDateParam = new SqlParameter("@StartDate", (object?)filter.StartDate ?? DBNull.Value);
                var endDateParam = new SqlParameter("@EndDate", (object?)filter.EndDate ?? DBNull.Value);

                return await _db.Set<GetAllDeductionData>()
              .FromSqlRaw("EXEC [dbo].[GetMonthlyDeductionData] @Month, @Year, @EmpCode,@BranchId, @StartDate, @EndDate",
                  monthParam, yearParam, empCodeParam, branchidParam, startDateParam, endDateParam)
              .ToListAsync();
            }
            catch (Exception ex)
            {


                return new List<GetAllDeductionData>();
            }
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
            //existingRecord.Insurance = deduction.Insurance;
            existingRecord.LWF = deduction.LWF;
            existingRecord.TDS = deduction.TDS;
            existingRecord.UpdatedBy = deduction.UpdatedBy;
            existingRecord.UpdatedDate = DateTime.UtcNow;
            return true;
        }
    }
}
