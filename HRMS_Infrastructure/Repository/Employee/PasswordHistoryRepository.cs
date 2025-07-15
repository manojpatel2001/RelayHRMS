using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM;
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
                exist.Password = histroy.NewPassword;
                await _db.SaveChangesAsync();   
                return exist;
            }
            catch
            {
                return null;
            }
        }

        public async Task<VMCommonResult> CreateHistoryPassword(PasswordHistory history)
        {
            try
            {
                // Call stored procedure to insert password if not repeated
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManagePasswordHistory
                    @Action = {"CREATE"},
                    @EMPID = {history.EMPID},
                    @NewPassword = {history.NewPassword},
                    @IsDeleted = {history.IsDeleted},
                    @IsEnabled = {history.IsEnabled},
                    @IsBlocked = {history.IsBlocked},
                    @CreatedDate = {history.CreatedDate},
                    @CreatedBy = {history.CreatedBy},
                    @UpdatedDate = {history.UpdatedDate},
                    @UpdatedBy = {history.UpdatedBy},
                    @DeletedDate = {history.DeletedDate},
                    @DeletedBy = {history.DeletedBy}
            ").ToListAsync();

                var resultData = result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };

                

                return resultData;
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }
        public async Task<VMCommonResult> CheckLastPassword(PasswordHistory history)
        {
            try
            {
                // Call stored procedure to insert password if not repeated
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC ManagePasswordHistory
                    @Action = {"CheckLastPassword"},
                    @EMPID = {history.EMPID},
                    @NewPassword = {history.NewPassword}
                   
            ").ToListAsync();

                var resultData = result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };

                

                return resultData;
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }
    }
}
