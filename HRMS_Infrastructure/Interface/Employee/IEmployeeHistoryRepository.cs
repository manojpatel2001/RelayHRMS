using HRMS_Core.VM.Ess.RecentActivity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeHistoryRepository
    {
        Task<List<vmEmployeeLifecycleEvent>> GetEmployeeLifecycleTimeline(int employeeId, int companyId);
    }
}
