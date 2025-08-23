using HRMS_Core.DbContext;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Leave;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HRMS_Infrastructure.Repository.Employee
{
    internal class EmployeeDashboardRepository : IEmployeeDashboardRepository
    {

        private readonly HRMSDbContext _db;

        public EmployeeDashboardRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<List<GetCountDirectOrIndirectEmployeesVM>> GetCountDirectOrIndirectEmployees( int EmployeeId, int Compid)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@ReportingId", EmployeeId) ,
                    new SqlParameter("@CompanyId", Compid) ,
                   

                    };

                var result = await _db.Set<GetCountDirectOrIndirectEmployeesVM>()
                    .FromSqlRaw("EXEC GetCountDirectOrIndirectEmployees @ReportingId, @CompanyId" ,parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<GetCountDirectOrIndirectEmployeesVM>();
            }
        }

        public async Task<List<EmployeeDirectIndirectReport>> GetDirectIndirectEmp(int Compid, int EmployeeId, string Action)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@ReportingId", EmployeeId) ,
                    new SqlParameter("@CompanyId", Compid) ,
                    new SqlParameter("@Action", Action) 

                    };

                var result = await _db.Set<EmployeeDirectIndirectReport>()
                    .FromSqlRaw("EXEC GetDirectOrIndirectEmployees @ReportingId, @CompanyId, @Action", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<EmployeeDirectIndirectReport>();
            }
        }

        public async Task<List<MyTeamleavesVM>> GetMyteamleave(int EmpId, int Compid, int Repoid)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@empid", EmpId) ,
                    new SqlParameter("@companyid", Compid) ,
                    new SqlParameter("@repoid", Repoid) ,


                    };

                var result = await _db.Set<MyTeamleavesVM>()
                    .FromSqlRaw("EXEC MyTeamLeaves @empid, @companyid , @repoid", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<MyTeamleavesVM>();
            }
        }

        public async Task<List<RecentEmployeeVM>> GetRecentJoinedEmployees(int Companyid)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@CompanyId", Companyid)

                };

                var result = await _db.Set<RecentEmployeeVM>()
                    .FromSqlRaw("EXEC sp_GetRecentJoinedEmployees @CompanyId", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<RecentEmployeeVM>();
            }
        }

        public async Task<List<WishesReportVM>> GetTodayBirthdaysByCompany(int Companyid)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@CompanyId", Companyid)

                };

                var result = await _db.Set<WishesReportVM>()
                    .FromSqlRaw("EXEC sp_GetTodayBirthdaysByCompany @CompanyId", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<WishesReportVM>();
            }

        }

        public async Task<List<WishesReportVM>> GetTodayMarriageAnnivarsary(int Companyid)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@CompanyId", Companyid)

                };

                var result = await _db.Set<WishesReportVM>()
                    .FromSqlRaw("EXEC sp_GetTodayMarriageAnnivarsary @CompanyId", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<WishesReportVM>();
            }

        }

        public async Task<List<WishesReportVM>> GetTodayWorkAnniversary(int Companyid)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@CompanyId", Companyid)

                };

                var result = await _db.Set<WishesReportVM>()
                    .FromSqlRaw("EXEC sp_GetTodayWorkAnniversary @CompanyId", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<WishesReportVM>();
            }

        }

        public async Task<List<UpcommingholidaysVM>> Getupcommingholidays(int Compid, int EmployeeId)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@CompanyId", Compid) ,
                    new SqlParameter("@EmployeeId", EmployeeId)

                    };

                var result = await _db.Set<UpcommingholidaysVM>()
                    .FromSqlRaw("EXEC sp_GetUpcomingHolidays @CompanyId  , @EmployeeId", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<UpcommingholidaysVM>();
            }
        }

      

    }
}
