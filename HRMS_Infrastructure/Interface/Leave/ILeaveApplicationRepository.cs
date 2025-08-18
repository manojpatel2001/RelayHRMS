using HRMS_Core.Leave;
using HRMS_Core.VM;
using HRMS_Core.VM.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Leave
{
    public interface ILeaveApplicationRepository: IRepository<LeaveApplication>
    {
        Task<SP_Response> InsertLeaveApplicationAsync(LeaveApplication model);
        Task<List<VMLeaveApplicationSearchResult>> GetLeaveApplicationsAsync(SearchVmCompOff filter);
        Task<List<VmLeaveApplicationforApprove>> GetLeaveApplicationsforApprove(SearchVmCompOff filter);
        Task<List<VmLeaveApplicationforApprove>> GetLeaveApplicationsforApproveAdmin(SearchVmCompOff filter);
        Task<List<LeaveTypevm>> GetLeaveDetails(LeaveDetailsvm vm);
        Task<bool> Updateapproval(List<int> applicationid, string status,DateTime Date);
        Task<bool> softdelete(LeaveApplication Leave);
        Task<LeaveApplication?> GetLeaveApplicationById(int leaveApplicationId);


    }
}
