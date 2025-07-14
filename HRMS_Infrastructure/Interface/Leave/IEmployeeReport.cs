using HRMS_Core.VM.Leave;
using HRMS_Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Leave
{
    public interface IEmployeeReport
    {

        Task<List<EmployeeAttendanceReportVm>> GetAttendanceReport(AttendanceReportVm VM);


    }
}
