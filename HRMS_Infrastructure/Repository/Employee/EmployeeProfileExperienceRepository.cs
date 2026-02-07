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
    public class EmployeeProfileExperienceRepository : IEmployeeProfileExperienceRepository
    {
        private readonly HRMSDbContext _db;
        public EmployeeProfileExperienceRepository(HRMSDbContext db)
        {
            _db = db;
        }
        public async Task<SP_Response> CreateEmployeeExperience(VmExperience model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageEmployeeProfileExperience
                        @Action = {"INSERT"},
                        @EmployeeId = {model.EmployeeId},
                        @Employer = {model.Employer},
                        @Branch = {model.Branch},
                        @Location = {model.Location},
                        @Designation = {model.Designation},
                        @StartDate = {model.StartDate},
                        @EndDate = {model.EndDate},
                        @CTC = {model.CTC},
                        @GrossSalary = {model.GrossSalary},
                        @Manager = {model.Manager},
                        @ManagerContactNo = {model.ManagerContactNo},
                        @Remarks = {model.Remarks},
                        @IndustryType = {model.IndustryType},
                        @DocumentPath = {model.DocumentPath},
                        @CreatedBy = {model.CreatedBy}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }
        public async Task<SP_Response> UpdateEmployeeExperience(VmExperience model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageEmployeeProfileExperience
                        @Action = {"UPDATE"},
                        @ExperienceId = {model.ExperienceId},
                        @EmployeeId = {model.EmployeeId},
                        @Employer = {model.Employer},
                        @Branch = {model.Branch},
                        @Location = {model.Location},
                        @Designation = {model.Designation},
                        @StartDate = {model.StartDate},
                        @EndDate = {model.EndDate},
                        @CTC = {model.CTC},
                        @GrossSalary = {model.GrossSalary},
                        @Manager = {model.Manager},
                        @ManagerContactNo = {model.ManagerContactNo},
                        @Remarks = {model.Remarks},
                        @IndustryType = {model.IndustryType},
                        @DocumentPath = {model.DocumentPath},
                        @UpdatedBy = {model.UpdatedBy}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }


        public async Task<SP_Response> DeleteEmployeeExperience(VmExperience model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageEmployeeProfileExperience
                        @Action = {"DELETE"},
                        @ExperienceId = {model.ExperienceId},
                        @DeletedBy = {model.DeletedBy}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<List<VmExperience>> GetAllEmployeeExperience()
        {
            // GET ALL
            try
            {
                return await _db.Database.SqlQueryRaw<VmExperience>(
                "EXEC ManageEmployeeProfileExperience @Action={0}", "GET"
                 ).ToListAsync();
            }
            catch
            {
                return new List<VmExperience>();
            }
        }

    }
}

