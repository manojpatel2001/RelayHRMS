using HRMS_Core.DbContext;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Leave;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class EmployeeDirecotryRepository : IEmployeeDirectory
    {

        private HRMSDbContext _db;

        public EmployeeDirecotryRepository(HRMSDbContext db)
        {
            _db = db;
        }


        public async Task<List<EmployeeDirectoryResultVM>> GetEmployeeDirectoryAsync(EmpDirectorysearchVm vm)
        {
            try
            {
                var parameters = new[]
                    {
                            new SqlParameter("@BranchId", vm.BranchId ?? (object)DBNull.Value),
                            new SqlParameter("@DepartmentId", vm.DepartmentId ?? (object)DBNull.Value),
                            new SqlParameter("@DesignationId", vm.DesignationId ?? (object)DBNull.Value),
                            new SqlParameter("@EmpCodeName", string.IsNullOrEmpty(vm.EmpCodeName) ? (object)DBNull.Value : vm.EmpCodeName),
                            new SqlParameter("@isenable", vm.isenable ?? (object)DBNull.Value),
                            new SqlParameter("@isdeleted", vm.isdeleted ?? (object)DBNull.Value)
                };


                var result = await _db.Set<EmployeeDirectoryResultVM>()
                    .FromSqlRaw("EXEC GetEmployeeDirecotry  @BranchId, @DepartmentId, @DesignationId, @EmpCodeName, @isenable, @isdeleted", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetLeaveApplicationsAsync Error: " + ex.Message);
                return new List<EmployeeDirectoryResultVM>();
            }
        }



       
     }
}
