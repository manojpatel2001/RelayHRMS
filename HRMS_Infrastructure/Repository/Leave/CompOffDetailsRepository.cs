using HRMS_Core.DbContext;
using HRMS_Core.Leave;
using HRMS_Core.VM.Leave;
using HRMS_Infrastructure.Interface.Leave;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Leave
{
    public class CompOffDetailsRepository:Repository<Comp_Off_Details>,ICompOffDetailsRepository
    {

        private HRMSDbContext _db;

        public CompOffDetailsRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<VMCompOffDetails>> GetCompOffApplicationsAsync(SearchVmCompOff filter)
        {
            var parameters = new[]
            {
            new SqlParameter("@SearchType", (object?)filter.SearchType ?? DBNull.Value),
            new SqlParameter("@SearchFor", (object?)filter.SearchFor ?? DBNull.Value),
            new SqlParameter("@Status", (object?)filter.Status ?? DBNull.Value),
            new SqlParameter("@ExtraWorkDate", (object?)filter.ExtraWorkDate ?? DBNull.Value)
        };

            return await _db.Set<VMCompOffDetails>()
                .FromSqlRaw("EXEC GetCompOffApplications @SearchType, @SearchFor, @Status, @ExtraWorkDate", parameters)
                .ToListAsync();
        }
        public async Task<bool> InsertCompOffAsync(Comp_Off_Details model)
        {
            try
            {
                var parameters = new[]
                {
                new SqlParameter("@Cmp_Id", model.Cmp_Id),
                new SqlParameter("@Emp_Id", model.Emp_Id),
                new SqlParameter("@Rep_Person_Id", model.Rep_Person_Id),
                new SqlParameter("@ApplicationDate", model.ApplicationDate),
                new SqlParameter("@Extra_Work_Day", model.Extra_Work_Day),
                new SqlParameter("@Extra_Work_Hours", model.Extra_Work_Hours ?? (object)DBNull.Value),
                new SqlParameter("@Application_Status", model.Application_Status ?? (object)DBNull.Value),
                new SqlParameter("@Comp_Off_Type", model.Comp_Off_Type ?? (object)DBNull.Value),
                new SqlParameter("@CreatedBy", model.CreatedBy ?? (object)DBNull.Value),
                new SqlParameter("@ComoffReason", model.ComoffReason ?? (object)DBNull.Value),
                new SqlParameter("@Emp_Code", model.Emp_Code ?? (object)DBNull.Value)
            };

                await _db.Database.ExecuteSqlRawAsync(
                    "EXEC usp_InsertCompOffDetails @Cmp_Id, @Emp_Id,@Emp_Code, @Rep_Person_Id, @ApplicationDate, @Extra_Work_Day, @Extra_Work_Hours, @Application_Status, @Comp_Off_Type,@CreatedBy,@ComoffReason",
                    parameters
                );

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Updateapproval(List<int> comoffid, string status)
        {
            try
            {
                var parameters = new[]
                {
            new SqlParameter("@compOffDetailsId", comoffid),
            new SqlParameter("@status", status)
        };

                await _db.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateCompOffApproval @compOffDetailsId, @status",
                    parameters
                );

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
