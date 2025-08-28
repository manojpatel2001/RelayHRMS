using HRMS_Core.DbContext;
using HRMS_Core.ManagePermission;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class EmployeeProfileSkillRepository : IEmployeeProfileSkillRepository
    {
        private readonly HRMSDbContext _db;

        public EmployeeProfileSkillRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<SP_Response> CreateEmpskill(VmSkill model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageEmployeeProfileSkill
                        @Action = {"CREATE"},
                        @EmployeeId = {model.EmployeeId},
                        @SkillName = {model.SkillName},
                        @YearsOfExperience = {model.YearsOfExperience},
                        @Comments = {model.Comments},
                        @CreatedBy = {model.CreatedBy}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Some thing went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Some thing went wrong!" };
            }
        }

        public async Task<SP_Response> UpdateEmpProfileSkill(VmSkill model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                            EXEC ManageEmployeeProfileSkill
                                @Action = {"UPDATE"},
                                @SkillId = {model.SkillId}, 
                                @EmployeeId = {model.EmployeeId},
                                @SkillName = {model.SkillName},
                                @YearsOfExperience = {model.YearsOfExperience},
                                @Comments = {model.Comments},
                                @UpdatedBy = {model.UpdatedBy}
                                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response
                {
                    Success = 0,
                    ResponseMessage = "Something went wrong!"
                };
            }
            catch (Exception ex)
            {
                return new SP_Response
                {
                    Success = -1,
                    ResponseMessage = "Exception: " + ex.Message
                };
            }
        }

        public async Task<SP_Response> DeleteEmpProfileSkill(VmSkill model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                             EXEC ManageEmployeeProfileSkill
                                 @Action = {"DELETE"},
                                 @SkillId = {model.SkillId}
                                 ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Some thing went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Some thing went wrong!" };
            }
        }

        public async Task<List<VmSkillMaster>> GetAllSkill()
        {
            try
            {
                    var result = await _db.Database
                    .SqlQueryRaw<VmSkillMaster>
                    ($@"
                     EXEC ManageEmployeeProfileSkill @Action = 'GET'
                     ")
                    .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                return new List<VmSkillMaster>();
            }
        }
    }
}
