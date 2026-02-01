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
    public class EmployeeProfileEducationRepository : IEmployeeProfileEducationRepository
    {
        private readonly HRMSDbContext _db;

        public EmployeeProfileEducationRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<SP_Response> CreateEmployeeEducation(VmEducation model)
        {



            try
            {
                // Note: SP must return columns: Success, ResponseMessage
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
            EXEC ManageEmployeeProfileEducation
                 @Action = {"INSERT"},
                 @EmployeeId = {model.EmployeeId},
                 @Education = {model.Education},
                 @Specialization = {model.Specialization},
                 @Year = {model.Year},
                 @ScoreOrClass = {model.ScoreOrClass},
                 @StartDate = {model.StartDate},
                 @EndDate = {model.EndDate},
                 @Comments = {model.Comments},
                 @DocumentPath = {model.DocumentPath},
                 @CreatedBy = {model.CreatedBy}
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
                    ResponseMessage = $"Error: {ex.Message}"
                };
            }
        }

        




        public async Task<SP_Response> UpdateEmployeeEducation(VmEducation model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                            EXEC ManageEmployeeProfileEducation
                                 @Action = {"UPDATE"},
                                 @EducationId = {model.EducationId},
                                 @EmployeeId = {model.EmployeeId},
                                 @Education = {model.Education},
                                 @Specialization = {model.Specialization},
                                 @Year = {model.Year},
                                 @ScoreOrClass = {model.ScoreOrClass},
                                 @StartDate = {model.StartDate},
                                 @EndDate = {model.EndDate},
                                 @Comments = {model.Comments},
                                 @DocumentPath = {model.DocumentPath},
                                 @UpdatedBy = {model.UpdatedBy}
                        ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response
                {
                    Success = 0,
                    ResponseMessage = "Something went wrong while updating Education record!"
                };
            }
            catch (Exception ex)
            {
                return new SP_Response
                {
                    Success = -1,
                    ResponseMessage = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<SP_Response> DeleteEmployeeEducation(VmEducation model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                             EXEC ManageEmployeeProfileEducation
                                 @Action = {"DELETE"},
                                 @EducationId = {model.EducationId}
                                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response
                {
                    Success = 0,
                    ResponseMessage = "Some thing went wrong!"
                };
            }
            catch
            {
                return new SP_Response
                {
                    Success = -1,
                    ResponseMessage = "Some thing went wrong!"
                };
            }
        }

        public async Task<List<VmEducation>> GetAllEmployeeEducation()
        {
          
            try
            {
                var result = await _db.Database
                    .SqlQueryRaw<VmEducation>($@"
                EXEC ManageEmployeeProfileEducation @Action = 'GET'
            ")
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                return new List<VmEducation>();
            }
       
    }

}
}

