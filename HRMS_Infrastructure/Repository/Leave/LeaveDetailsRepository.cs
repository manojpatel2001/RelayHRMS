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
    public class LeaveDetailsRepository:Repository<LeaveDetails>,ILeaveDetailsRepository
    {
        private HRMSDbContext _db;

        public LeaveDetailsRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> InsertLeaveManageAsync(LeaveDetails model)
        {
            

            try
            {
                var parameters = new[]
                {
                new SqlParameter("@EmpId", model.Emp_Id),
                new SqlParameter("@CompId", model.Comp_Id),
          
                new SqlParameter("@CreatedBy", model.CreatedBy),
                new SqlParameter("@CreatedDate", model.CreatedDate)
             
               
            };

                await _db.Database.ExecuteSqlRawAsync(
                    "EXEC sp_AlwaysInsertLeaveDetails @EmpId,@CompId,@CreatedBy,@CreatedDate",
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
