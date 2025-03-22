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
    public class DepartmentRepository : Repository<Department> , IDepartmentRepository
    {
        private readonly HRMSDbContext _db;

        public DepartmentRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Department> SoftDelete(DeleteRecordVM DeleteRecord)
        {
            var department = await _db.Departments.FirstOrDefaultAsync(asd => asd.DepartmentId == DeleteRecord.Id);
            if (department == null)
            {
                return department;
            }
            else
            {
                department.IsEnabled = false;
                department.IsDeleted = true;
                department.DeletedDate = DateTime.UtcNow;
                department.DeletedBy = DeleteRecord.DeletedBy;
                return department;
            }
        }

        public async Task<bool> UpdateDepartment(Department department)
        {
            var existingRecord = await _db.Departments.SingleOrDefaultAsync(asd => asd.DepartmentId == department.DepartmentId);
            if (existingRecord == null)
            {
                return false;
            }
            existingRecord.DepartmentName = department.DepartmentName;
            existingRecord.Code = department.Code;
            existingRecord.SortingNo = department.SortingNo;
            existingRecord.MinimumWages = department.MinimumWages;
            existingRecord.OJTApplicable = department.OJTApplicable;
            existingRecord.UpdatedBy = department.UpdatedBy;
            existingRecord.UpdatedDate = DateTime.UtcNow;
            return true;
        }
    }
}
