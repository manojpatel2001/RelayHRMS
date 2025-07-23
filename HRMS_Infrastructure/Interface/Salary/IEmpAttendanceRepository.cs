using HRMS_Core.Migrations;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.importData;
using HRMS_Core.VM.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Salary
{
    public interface IEmpAttendanceRepository : IRepository<EmpAttendanceImport>
    {
        Task<bool> UpdateEmpAttendance(EmpAttendanceImport empAttendanceImport);
        Task<EmpAttendanceImport> SoftDelete(DeleteRecordVM DeleteRecord);
        Task<List<EmpAttendanceVM>> GetEmpAttendanceDataAsync(SearchFilterModel filter);

    }
}
