using Azure.Core;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Leave
{
    public interface ILeaveApplicationRepository : IRepository<HRMS_Core.Leave.LeaveApplication>
    {
        Task<SP_Response> InsertLeaveApplicationAsync(HRMS_Core.Leave.LeaveApplication model);
        Task<List<VMLeaveApplicationSearchResult>> GetLeaveApplicationsAsync(SearchVmCompOff filter);
        Task<List<VmLeaveApplicationforApprove>> GetLeaveApplicationsforApprove(SearchVmCompOff filter);
        Task<List<VmLeaveApplicationforApprove>> GetLeaveApplicationsforApproveAdmin(SearchVmCompOff filter);
        Task<List<LeaveTypevm>> GetLeaveDetails(LeaveDetailsvm vm);
        Task<List<LeaveApprovalReportVM>> GetLeaveApproval(LeaveApp_Param vm);
        Task<List<LeaveBalanceViewModel>> GetLeaveBalance(LeaveBalance_Param vm);
        Task<SP_Response> Updateapproval(LeaveaprovalVM LVM);
        Task<bool> softdelete(HRMS_Core.Leave.LeaveApplication Leave);
        Task<VMCommonResult> Delete(DeleteRecordVModel deleteRecord);
        Task<HRMS_Core.Leave.LeaveApplication?> GetLeaveApplicationById(int leaveApplicationId);
        Task<CompoffLeaveBalanceViewModel?> GetLastLeaveBalanceDate(int Emp_Id);
        Task<List<YearlyLeaveReportViewModel>> GetYearlyLeaveReport(GetYearlyLeaveReportRequest request);
        Task<List<LeaveApplicationReportModel>> GetLeaveApplicationsReport(GetYearlyLeaveReportRequest request);
    }
}