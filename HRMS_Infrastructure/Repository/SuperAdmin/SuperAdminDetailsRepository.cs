using HRMS_Core.DbContext;
using HRMS_Core.SuperAdmin;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.SuperAdmin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.SuperAdmin
{
    internal class SuperAdminDetailsRepository:Repository<SuperAdminDetails>, ISuperAdminDetailsRepository
    {

        private readonly HRMSDbContext _db;

        public SuperAdminDetailsRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<SuperAdminDetails?> GetSuperAdminByCredentials(vmLogin vmLogin)
        {
            try
            {
                var  result= await _db.Set<SuperAdminDetails>().FromSqlInterpolated($"EXEC GetSuperAdminByCredentials @Email={vmLogin.Email},@Password={vmLogin.Password}").ToListAsync();
                return result.FirstOrDefault() ?? null;
            }
            catch
            {
                return null;
            }
        }
    }
}
