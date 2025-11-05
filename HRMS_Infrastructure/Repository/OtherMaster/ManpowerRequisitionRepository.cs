using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

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

   
     public async Task<APIResponse> GetAllManpowerRequisitions(CommonParameter commonParameter)
    {
        var response = new APIResponse();
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CompanyId", commonParameter.CompanyId);
                parameters.Add("@BranchId", commonParameter.BranchId);

                // Execute the stored procedure and map results to a dynamic list
                var result = await connection.QueryAsync<dynamic>(
                    "GetAllManpowerRequisitions",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (!result.AsList().Any())
                {
                    response.isSuccess = false;
                    response.ResponseMessage = "No records found.";
                    response.Data = new List<dynamic>(); 
                    return response;
                }

                response.isSuccess = true;
                response.ResponseMessage = "Success!";
                response.Data = result.AsList(); 
            }
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.ResponseMessage = ex.Message;
            response.Data = new List<dynamic>(); 
        }
        return response;
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
                            Departments = (await multi.ReadAsync<DepartmentViewModel>()).AsList(),
                            Branches = (await multi.ReadAsync<branchViewModel>()).AsList()
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


        public async Task<APIResponse> GetManpowerRequisitionByManpowerRequisitionId(int ManpowerRequisitionId)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@ManpowerRequisitionId", ManpowerRequisitionId);

                    // Execute the stored procedure and map the result to a dynamic object
                    var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                        "GetManpowerRequisitionByManpowerRequisitionId",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result != null)
                    {
                        response.isSuccess = true;
                        response.Data = result;
                        response.ResponseMessage = "Fetch successfully!";
                    }
                    else
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "No Record found!";
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                response.isSuccess = false;
                response.ResponseMessage = "Something went wrong!";
                response.Data = null;
            }
            return response;
        }
        public async Task<APIResponse> GetAllSerialNo(CommonParameter commonParameter)
        {
            try
            {
                var data= await _db.Set<SerialNoViewModel>()
                    .FromSqlInterpolated($@"EXEC GetAllSerialNo @CompanyId={commonParameter.CompanyId}")
                    .ToListAsync();

                if (data.Any())
                {
                    return new APIResponse { isSuccess = true ,Data=data ,ResponseMessage = "Fetch successfully!" };
                }
                else
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "No Record found!" };
                }
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                return new APIResponse { isSuccess = false ,ResponseMessage="Some thing went wrong!"};
            }
        }

        public async Task<APIResponse> UpdateJoinningDetails(UpdateJoinningDetailsModel model)
        {
            try
            {
                //UPDATE_EmployeeInfo,UPDATE_JoiningDetails,UPDATE_SalaryDetails,UPDATE_ContactDetails,UPDATE_DocumentDetails
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                    EXEC UpdateJoinningDetails
                        @Action = {model.Action},
                        @ManpowerRequisitionId = {model.ManpowerRequisitionId},
                        @OperationType = {model.OperationType},
                        @PresentAddress = {model.PresentAddress},
                        @PermanentAddress = {model.PermanentAddress},
                        @MaritalStatus = {model.MaritalStatus},
                        @DateOfBirth = {model.DateOfBirth},
                        @BloodGroup = {model.BloodGroup},
                       
                        @InterviewedBy = {model.InterviewedBy},
                        @InterviewDate = {model.InterviewDate},
                        @InterviewPlace = {model.InterviewPlace},
                        @ClientName = {model.ClientName},
                        @ERPCode = {model.ERPCode},
                        @PreviousCompanyUAN = {model.PreviousCompanyUAN},
                        @PreviousCompanyESIC = {model.PreviousCompanyESIC},
                       
                        @PFUAN = {model.PFUAN},
                        @ESICNo = {model.ESICNo},
                        @PAN = {model.PAN},
                        @GrossSalary = {model.GrossSalary},
                        @NetSalary = {model.NetSalary},
                        
                        @ContactNo1 = {model.ContactNo1},
                        @ContactPersonName1 = {model.ContactPersonName1},
                        @ContactPersonRelation1 = {model.ContactPersonRelation1},
                        @ContactNo2 = {model.ContactNo2},
                        @ContactPersonName2 = {model.ContactPersonName2},
                        @ContactPersonRelation2 = {model.ContactPersonRelation2},
                        @ContactNo3 = {model.ContactNo3},
                        @ContactPersonName3 = {model.ContactPersonName3},
                        @ContactPersonRelation3 = {model.ContactPersonRelation3},
                        
                        @AadharCardNo = {model.AadharCardNo},
                        @AadharCardCopyPath = {model.AadharCardCopyPath},
                        @PanCardNo = {model.PanCardNo},
                        @PanCardCopyPath = {model.PanCardCopyPath},
                        @FreshResumePath = {model.FreshResumePath},
                        @PassportSizePhotoCopyPath = {model.PassportSizePhotoCopyPath},
                        @CancelledChequePath = {model.CancelledChequePath},
                        @PayslipPath = {model.PayslipPath},
                        @ExperienceCertificatePath = {model.ExperienceCertificatePath}
                ")
                    .ToListAsync();
                var data = result.FirstOrDefault() ?? null;
                if (data != null && data.Success>0)
                {
                    return new APIResponse { isSuccess = true, Data = data, ResponseMessage = data.ResponseMessage };
                }
                else
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = data.ResponseMessage??"Some thing went wrong!" };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Some thing went wrong!" };
            }
        }
    }

}
