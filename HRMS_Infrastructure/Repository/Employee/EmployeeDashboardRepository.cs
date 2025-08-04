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

        public async Task<List<UpcommingholidaysVM>> Getupcommingholidays(int Compid)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@CompanyId", Compid)

                    };

                var result = await _db.Set<UpcommingholidaysVM>()
                    .FromSqlRaw("EXEC sp_GetUpcomingHolidays @CompanyId", parameters)
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
