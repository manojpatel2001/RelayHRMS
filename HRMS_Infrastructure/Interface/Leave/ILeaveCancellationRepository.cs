using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Leave;
using HRMS_Core.VM.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Leave
{
    public interface ILeaveCancellationRepository: IRepository<LeaveCancellationRequestVM>
    {
        Task<List<LeaveCancellationReportViewModel>> GetLeavecancellationReport(LeaveCancellationReportRequest vm);
        Task<List<EmpLeaveCancellationRequestReportViewModel>> GetEmpLeaveCancellationRequestReport(LeaveCancellationRequestFilterViewModel vm);
        Task<List<EmpLeaveCancellationRequestReportViewModel>> GetReportingWiseLeaveCancellationRequestReport(vmLeaveCancellationReportFilter vm);
        Task<SP_Response> CreateLeavecancellation(LeaveCancellationRequestVM model);
        Task<SP_Response> UpdateLeavecancellation(updateLeaveCancellationRequestVM model);
        Task<SP_Response> DeleteLeavecancellation(DeleteRecordVM deleteRecord);
    }
}
