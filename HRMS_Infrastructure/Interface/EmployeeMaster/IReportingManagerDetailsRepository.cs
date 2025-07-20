using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.EmployeeMaster
{
    public interface IReportingManagerDetailsRepository
    {
        Task<VMCommonResult> CreateReportingManagerDetail(ReportingManagerDetails model);
        Task<VMCommonResult> UpdateReportingManagerDetail(ReportingManagerDetails model);
        Task<VMCommonResult> DeleteReportingManagerDetail(DeleteRecordVM deleteRecord);
        Task<ReportingManagerDetails?> GetReportingManagerDetailById(vmCommonGetById vmCommonGetById);
        Task<List<ReportingManagerDetails>> GetAllReportingManagerDetails(vmCommonGetById vmCommonGetById);
    }
}
