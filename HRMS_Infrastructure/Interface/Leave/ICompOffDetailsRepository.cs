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
        Task<bool> InsertCompOffAsync(Comp_Off_Details model);
        Task<bool> Updateapproval(List<int> comoffid, string status);
        Task<bool> UpdateLeaveManger(List<int> comoffid, string status);
        Task<bool> UpdateLeavedetails(List<int> ids, string status);
        Task<List<VMCompOffDetails>> GetCompOffApplicationsAsync(SearchVmCompOff filter);
        Task<List<VMCompOffDetails>> GetCompOffApplicationsAdmin(SearchVmCompOff filter);
    }
}
