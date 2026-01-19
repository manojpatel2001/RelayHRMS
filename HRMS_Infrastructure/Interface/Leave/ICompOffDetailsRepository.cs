using HRMS_Core.EmployeeMaster;
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
    public interface ICompOffDetailsRepository:IRepository<Comp_Off_Details>
    {
        Task<SP_Response> InsertCompOffAsync(Comp_Off_Details model);
        Task<SP_Response> UpdateCompOffApproval(ApproveandrejectVM compOffVM);
        Task<bool> UpdateLeaveManger(List<int> comoffid, string status);
        Task<bool> UpdateLeavedetails(List<int> ids, string status);
        Task<List<VMCompOffDetails>> GetCompOffApplicationsAsync(SearchVmCompOff filter);
        Task<List<VMCompOffDetails>> GetCompOffApplicationsAdmin(SearchVmCompOff filter);
        Task<List<CompOffBalanceReportViewModel>> GetCompOffAvailableBalanceReport(CompOffBalanceReportParamViewModel filter);
        Task<List<CompOffReportDetailedModel>> GetCompOffReportDetailed(CompOffBalanceReportParamViewModel filter);
        Task<List<CompOffDetailsReportViewModelAdmin>> GetCompOffDetailsReportForAdmin(SearchVmForCompoffAdmin filter);
        Task<Comp_Off_Details?> GetCompOffApplicationById(int Comp_Off_DetailsId); 
        Task<List<Comp_Off_Details?>> GetApprovedCompOffDetails(Comp_offpara model); 
    }
}
