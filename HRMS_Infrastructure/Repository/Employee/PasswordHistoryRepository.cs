using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class PasswordHistoryRepository : Repository<PasswordHistory> , IPasswordHistory
    {
        private readonly HRMSDbContext _db;

        public PasswordHistoryRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<HRMSUserIdentity?> ChangePassword(PasswordHistory histroy)
        {
            try
            {
                var exist=await _db.HRMSUserIdentities.FirstOrDefaultAsync(x=>x.Id== histroy.EMPID);
                if (exist == null)
                {
                    return null;
                }
                exist.IsPasswordChange = true;
                await _db.SaveChangesAsync();   
                return exist;
            }
            catch
            {
                return null;
            }
        }
    }
}
