using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeDashboardRepository
    {
        Task<List<WishesReportVM>> GetTodayMarriageAnnivarsary(int Companyid);
        Task<List<WishesReportVM>> GetTodayBirthdaysByCompany(int Companyid);
        Task<List<WishesReportVM>> GetTodayWorkAnniversary(int Companyid);
        Task<List<RecentEmployeeVM>> GetRecentJoinedEmployees(int Companyid);
        Task<List<UpcommingholidaysVM>> Getupcommingholidays(int Compid);
    }
}
