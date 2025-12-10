using HRMS_Core.Salary;
using HRMS_Core.VM.Leave;
using HRMS_Core.VM.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Leave
{
    public interface ILeaveCancellationRepository: IRepository<LeaveCancellationReportViewModel>
    {
        Task<List<LeaveCancellationReportViewModel>> GetLeavecancellationReport(LeaveCancellationReportRequest vm);
    }
}
