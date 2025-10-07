using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.OtherMaster;
using HRMS_Core.VM.UpdateEmployee;
using HRMS_Infrastructure.Interface.OtherMaster;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.OtherMaster
{
    public class ManpowerRequisitionRepository : IManpowerRequisitionRepository
    {
        private HRMSDbContext _db;
        private readonly string _connectionString;

        public ManpowerRequisitionRepository(HRMSDbContext db) 
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<List<ManpowerRequisitionViewModel>> GetAllManpowerRequisitions( int CompanyId)
        {
            try
            {
                return await _db.Set<ManpowerRequisitionViewModel>()
                    .FromSqlInterpolated($"EXEC GetAllManpowerRequisitions @CompanyId={CompanyId}")
                    .ToListAsync();
            }
            catch
            {
                return new List<ManpowerRequisitionViewModel>();
            }
        }

       
        public async Task<SP_Response> CreateManpowerRequisition(ManpowerRequisition manpowerRequisition)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                    EXEC ManageManpowerRequisition
                        @Action = {"CREATE"},
                        @DepartmentId = {manpowerRequisition.DepartmentId},
                        @RequirementType = {manpowerRequisition.RequirementType},
                        @EmployeeName = {manpowerRequisition.EmployeeName},
                        @PersonalEmail = {manpowerRequisition.PersonalEmail},
                        @ContactNumber = {manpowerRequisition.ContactNumber},
                        @ClosureBy = {manpowerRequisition.ClosureBy},
                        @DesignationId = {manpowerRequisition.DesignationId},
                        @ExperienceRange = {manpowerRequisition.ExperienceRange},
                        @EducationalQualification = {manpowerRequisition.EducationalQualification},
                        @ComputerSkills = {manpowerRequisition.ComputerSkills},
                        @JobResponsibility = {manpowerRequisition.JobResponsibility},
                        @Age = {manpowerRequisition.Age},
                        @Gender = {manpowerRequisition.Gender},
                        @OtherBenefits = {manpowerRequisition.OtherBenefits},
                        @SystemRequire = {manpowerRequisition.SystemRequire},
                        @EmailIdRequire = {manpowerRequisition.EmailIdRequire},
                        @SIMRequire = {manpowerRequisition.SIMRequire},
                        @MobileHandsetRequire = {manpowerRequisition.MobileHandsetRequire},
                        @ReportingToId = {manpowerRequisition.ReportingToId},
                        @DateOfJoining = {manpowerRequisition.DateOfJoining},
                        @CategoryOfEmployment = {manpowerRequisition.CategoryOfEmployment},
                        @CTC_Monthly = {manpowerRequisition.CTC_Monthly},
                        @GrossSalary = {manpowerRequisition.GrossSalary},
                        @TakeHomeSalary = {manpowerRequisition.TakeHomeSalary},
                        @CreatedBy = {manpowerRequisition.CreatedBy},
                        @CompanyId = {manpowerRequisition.CompanyId}
                    ")
                    .ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Failed to create requisition." };
            }
            catch (Exception ex)
            {
                return new SP_Response { Success = -1, ResponseMessage = $"Error: {ex.Message}" };
            }
        }

        public async Task<SP_Response> UpdateManpowerRequisition(ManpowerRequisition manpowerRequisition)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                    EXEC ManageManpowerRequisition
                        @Action = {"UPDATE"},
                        @ManpowerRequisitionId = {manpowerRequisition.ManpowerRequisitionId},
                        @DepartmentId = {manpowerRequisition.DepartmentId},
                        @RequirementType = {manpowerRequisition.RequirementType},
                        @EmployeeName = {manpowerRequisition.EmployeeName},
                        @PersonalEmail = {manpowerRequisition.PersonalEmail},
                        @ContactNumber = {manpowerRequisition.ContactNumber},
                        @ClosureBy = {manpowerRequisition.ClosureBy},
                        @DesignationId = {manpowerRequisition.DesignationId},
                        @ExperienceRange = {manpowerRequisition.ExperienceRange},
                        @EducationalQualification = {manpowerRequisition.EducationalQualification},
                        @ComputerSkills = {manpowerRequisition.ComputerSkills},
                        @JobResponsibility = {manpowerRequisition.JobResponsibility},
                        @Age = {manpowerRequisition.Age},
                        @Gender = {manpowerRequisition.Gender},
                        @OtherBenefits = {manpowerRequisition.OtherBenefits},
                        @SystemRequire = {manpowerRequisition.SystemRequire},
                        @EmailIdRequire = {manpowerRequisition.EmailIdRequire},
                        @SIMRequire = {manpowerRequisition.SIMRequire},
                        @MobileHandsetRequire = {manpowerRequisition.MobileHandsetRequire},
                        @ReportingToId = {manpowerRequisition.ReportingToId},
                        @DateOfJoining = {manpowerRequisition.DateOfJoining},
                        @CategoryOfEmployment = {manpowerRequisition.CategoryOfEmployment},
                        @CTC_Monthly = {manpowerRequisition.CTC_Monthly},
                        @GrossSalary = {manpowerRequisition.GrossSalary},
                        @TakeHomeSalary = {manpowerRequisition.TakeHomeSalary},
                        @IsEnabled = {manpowerRequisition.IsEnabled},
                        @IsDeleted = {manpowerRequisition.IsDeleted},
                        @UpdatedBy = {manpowerRequisition.UpdatedBy}
                ")
                    .ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Failed to update requisition." };
            }
            catch (Exception ex)
            {
                return new SP_Response { Success = -1, ResponseMessage = $"Error: {ex.Message}" };
            }
        }

        public async Task<SP_Response> DeleteManpowerRequisition(DeleteRecordVM model)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                    EXEC ManageManpowerRequisition
                        @Action = {"DELETE"},
                        @ManpowerRequisitionId = {model.Id},
                        @UpdatedBy = {model.DeletedBy}
                ")
                    .ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Failed to delete requisition." };
            }
            catch (Exception ex)
            {
                return new SP_Response { Success = -1, ResponseMessage = $"Error: {ex.Message}" };
            }
        }

   
        public async Task<APIResponse> GetDropDownForManpower(int CompanyId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var multi = await connection.QueryMultipleAsync(
                        "GetDropDownForManpower",
                        new { CompanyId },
                        commandType: CommandType.StoredProcedure))
                    {
                        var result = new VMGetDropDownForManpower
                        {
                            ReportingEmployees = (await multi.ReadAsync<ReportingEmployeeViewModel>()).AsList(),

                           Designations = (await multi.ReadAsync<DesignationViewModel>()).AsList(),
                            Departments = (await multi.ReadAsync<DepartmentViewModel>()).AsList()
                        };



                        return new APIResponse { Data = result, ResponseMessage = "Fetched successfully!", isSuccess = true };
                    }
                }
            }
            catch (Exception)
            {
                return new APIResponse { ResponseMessage = "Some thing Went wrong!", isSuccess = false };
            }
        }

    }

}
