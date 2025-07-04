using HRMS_Core.DbContext;
using HRMS_Core.Leave;
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
                new SqlParameter("@CreatedBy", model.CreatedBy ?? (object)DBNull.Value)
            };

                await _db.Database.ExecuteSqlRawAsync(
                    "EXEC usp_InsertCompOffDetails @Cmp_Id, @Emp_Id, @Rep_Person_Id, @ApplicationDate, @Extra_Work_Day, @Extra_Work_Hours, @Application_Status, @Comp_Off_Type,@CreatedBy",
                    parameters
                );

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Updateapproval(int empId, string status)
        {
            try
            {
                var parameters = new[]
                {
            new SqlParameter("@emp_id", empId),
            new SqlParameter("@status", status)
        };

                await _db.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateCompOffApproval @emp_id, @status",
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
