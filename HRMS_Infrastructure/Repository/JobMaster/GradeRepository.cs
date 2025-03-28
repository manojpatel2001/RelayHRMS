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
    public class GradeRepository : Repository<Grade> , IGradeRepository
    {
        private readonly HRMSDbContext _db;

        public GradeRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Grade> SoftDelete(DeleteRecordVM DeleteRecord)
        {
            var grade = await _db.Grades.FirstOrDefaultAsync(asd => asd.GradeId == DeleteRecord.Id);
            if (grade == null)
            {
                return grade;
            }
            else
            {
                grade.IsEnabled = false;
                grade.IsDeleted = true;
                grade.DeletedDate = DateTime.UtcNow;
                grade.DeletedBy = DeleteRecord.DeletedBy;
                return grade;
            }
        }

        public async Task<bool> UpdateGrade(Grade grade)
        {
            var existingRecord = await _db.Grades.SingleOrDefaultAsync(asd => asd.GradeId == grade.GradeId);
            if (existingRecord == null)
            {
                return false;
            }
            existingRecord.GradeName =grade.GradeName;
            existingRecord.Description = grade.Description;
            existingRecord.BasicSalary = grade.BasicSalary;
            existingRecord.Govt_MinimumBasics = grade.Govt_MinimumBasics;
            existingRecord.NoticePeriodDays = grade.NoticePeriodDays;
            existingRecord.NoticePeriodWorkingDays = grade.NoticePeriodWorkingDays;
            existingRecord.SalaryRange = grade.SalaryRange;
            existingRecord.EligibilityAmount = grade.EligibilityAmount;
            existingRecord.WagesType = grade.WagesType;
            existingRecord.OverTimeApplicable = grade.OverTimeApplicable;
            existingRecord.UpdatedBy = grade.UpdatedBy;
            existingRecord.UpdatedDate = DateTime.UtcNow;
            return true;
        }
    }
}
