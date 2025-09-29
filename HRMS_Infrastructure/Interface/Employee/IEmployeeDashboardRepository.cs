using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Leave;
using HRMS_Core.VM.Report;
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
        Task<List<UpcommingholidaysVM>> Getupcommingholidays(int EmployeeId, int Compid);
        Task<List<GetCountDirectOrIndirectEmployeesVM>> GetCountDirectOrIndirectEmployees(int Compid ,int EmployeeId);
        Task<List<EmployeeDirectIndirectReport>> GetDirectIndirectEmp(int Compid, int EmployeeId, string Action);
        Task<List<MyTeamleavesVM>> GetMyteamleave(int EmpId, int Compid, int Repoid);
        Task<List<EmployeeDetailsViewModel>> GetEmployeeDetails(int EmpId);
        Task<List<RecentJoinedEmplForAdmin>> GetRecentJoinedEmployeesForAdmin();
        Task<List<NewJoinerDetailsViewModel>> GetBranchNewJoinerDetails(int CompId , int BranchId);
    }
}
