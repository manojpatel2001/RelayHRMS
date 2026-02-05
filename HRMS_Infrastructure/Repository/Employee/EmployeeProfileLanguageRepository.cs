using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class EmployeeProfileLanguageRepository : IEmployeeProfileLanguageRepository
    {
        private readonly HRMSDbContext _db;

        public EmployeeProfileLanguageRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<SP_Response> AddEmpProfileLanguge(VmLanguage model)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                 .FromSqlInterpolated($@"
                    EXEC ManageEmployeeProfileLanguage 
                        @Action = {"INSERT"},
                        @EmployeeId = {model.EmployeeId},
                        @LanguageName = {model.LanguageName},
                        @Fluency = {model.Fluency},
                        @CanWrite = {model.CanWrite},
                        @CanRead = {model.CanRead},
                        @CanSpeak = {model.CanSpeak},
                        @CanUnderstand = {model.CanUnderstand},
                        @CreatedBy = {model.CreatedBy}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Some thing went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Some thing went wrong!" };
            }
        }

        public async Task<SP_Response> UpdateEmpProfileLanguge(VmLanguage model)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                        EXEC ManageEmployeeProfileLanguage 
                            @Action = {"UPDATE"},
                            @LanguageId = {model.LanguageId},
                            @EmployeeId = {model.EmployeeId},
                            @LanguageName = {model.LanguageName},
                            @Fluency = {model.Fluency},
                            @CanWrite = {model.CanWrite},
                            @CanRead = {model.CanRead},
                            @CanSpeak = {model.CanSpeak},
                            @CanUnderstand = {model.CanUnderstand},
                            @UpdatedBy = {model.UpdatedBy}
                    ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> DeleteEmpProfileLanguge(VmLanguage model)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                        EXEC ManageEmployeeProfileLanguage 
                            @Action = {"DELETE"},
                            @LanguageId = {model.LanguageId},
                            @UpdatedBy = {model.UpdatedBy}
                    ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<List<VmLanguage>> GetAllEmpProfileLanguage()
        {
            try
            {
                return await _db.Database.SqlQueryRaw<VmLanguage>(
                "EXEC ManageEmployeeProfileLanguage @Action={0}", "GET"
                 ).ToListAsync();
            }
            catch
            {
                return new List<VmLanguage>();
            }
        }

    }
}
