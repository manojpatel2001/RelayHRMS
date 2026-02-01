using HRMS_Core.DbContext;
using HRMS_Core.ManagePermission;
using HRMS_Core.Master.OtherMaster;
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

        public async Task<SP_Response> CreateEmployeeProfileSkill(EmployeeProfile_Skill model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageEmployeeProfileSkill
                        @Action = {"CREATE"},
                        @EmployeeId = {model.EmployeeId},
                        @SkillMasterId = {model.SkillMasterId},
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

        public async Task<SP_Response> UpdateEmployeeProfileSkill(EmployeeProfile_Skill model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                            EXEC ManageEmployeeProfileSkill
                                @Action = {"UPDATE"},
                                @EmployeeProfile_SkillId = {model.EmployeeProfile_SkillId}, 
                                @EmployeeId = {model.EmployeeId},
                                @SkillMasterId = {model.SkillMasterId},
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
                return new SP_Response { Success = -1, ResponseMessage = "Some thing went wrong!" };

            }
        }

        public async Task<SP_Response> DeleteEmployeeProfileSkill(DeleteRecordVM model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                             EXEC ManageEmployeeProfileSkill
                                 @Action = {"DELETE"},
                                 @EmployeeProfile_SkillId = {model.Id},
                                 @DeletedBy = {model.DeletedBy}
                                 ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Some thing went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Some thing went wrong!" };
            }
        }

        public async Task<List<EmployeeProfile_Skill>> GetAllEmployeeProfile_Skills(int EmployeeId)
        {
            try
            {
                return await _db.Set<EmployeeProfile_Skill>()
                   .FromSqlInterpolated($" EXEC GetAllEmployeeProfile_Skills @EmployeeId = {EmployeeId}")
                   .ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<EmployeeProfile_Skill>();
            }
        }
    }
}
