using Dapper;
using HRMS_Core.ControlPanel.CompanyInformation;
using HRMS_Core.DbContext;
using HRMS_Core.ProfileManage;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyInformation;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Core.VM.UpdateEmployee;
using HRMS_Infrastructure.Interface.EmployeeMaster;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.EmployeeMaster
{
    public class EmployeeManageRepository : Repository<HRMSUserIdentity>, IEmployeeManageRepository
    {
        private HRMSDbContext _db;
        private readonly string _connectionString;

        public EmployeeManageRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }



        public async Task<VMCommonResult> UpdateEmployee(vmUpdateEmployee employee)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageEmployee
                        @Action = {"UPDATE"},
                        @Id = {employee.Id},
                        @FullName = {employee.FullName},
                        @Initial = {employee.Initial},
                        @FirstName = {employee.FirstName},
                        @MiddleName = {employee.MiddleName},
                        @LastName = {employee.LastName},
                        @EmployeeCode = {employee.EmployeeCode},
                        @AlfaEmployeeCode = {employee.AlfaEmployeeCode},
                        @AlfaCode = {employee.AlfaCode},
                        @DateOfJoining = {employee.DateOfJoining},
                        @BranchId = {employee.BranchId},
                        @GradeId = {employee.GradeId},
                        @ShiftMasterId = {employee.ShiftMasterId},
                        @CTC = {employee.CTC},
                        @DesignationId = {employee.DesignationId},
                        @GrossSalary = {employee.GrossSalary},
                        @CategoryId = {employee.CategoryId},
                        @BasicSalary = {employee.BasicSalary},
                        @DepartmentId = {employee.DepartmentId},
                        @EmployeeTypeId = {employee.EmployeeTypeId},
                        @DateOfBirth = {employee.DateOfBirth},
                        @LoginAlias = {employee.LoginAlias},
                        @Password = {employee.Password},
                        @ReportingManagerId = {employee.ReportingManagerId},
                        @SubBranch = {employee.SubBranch},
                        @EnrollNo = {employee.EnrollNo},
                        @CompanyId = {employee.CompanyId},
                        @Pt = {employee.Pt},
                        @EmployeeProfileUrl = {employee.EmployeeProfileUrl},
                        @EmployeeSignatureUrl = {employee.EmployeeSignatureUrl},
                        @UpdatedDate = {employee.UpdatedDate},
                        @UpdatedBy = {employee.UpdatedBy},
                        @WeekOffDetailsId = {employee.WeekOffDetailsId},
                        @IsPermissionPunchInOut = {employee.IsPermissionPunchInOut},
                        @IsPFApplicable = {employee.IsPFApplicable},
                        
                        -- Personal Info
                        @Gender = {employee.Gender},
                        @PersonalEmailId = {employee.PersonalEmailId},
                        @FatherName = {employee.FatherName},
                        @MotherName = {employee.MotherName},
                        @BloodGroup = {employee.BloodGroup},
                        @Height = {employee.Height},
                        @MaritalStatus = {employee.MaritalStatus},
                        @MarriageDate = {employee.MarriageDate},
                        @MarkIdentification = {employee.MarkIdentification},
                        @Religion = {employee.Religion},
                        @Caste = {employee.Caste},
                        @CastCategory = {employee.CastCategory},
                        @AadharCardNo = {employee.AadharCardNo},
                        @PANNo = {employee.PANNo},
                        @Dispensary = {employee.Dispensary},
                        @DoctorName = {employee.DoctorName},
                        @DispensaryAddress = {employee.DispensaryAddress},
                        @UANNumber = {employee.UANNumber},
                        @DrivingLicense = {employee.DrivingLicense},
                        @DrivingLicenseExpiry = {employee.DrivingLicenseExpiry},
                        @RationCardType = {employee.RationCardType},
                        @RationCardNo = {employee.RationCardNo},
                        @ProbationCompletionPeriod = {employee.ProbationCompletionPeriod},
                        @ProbationPeriodType = {employee.ProbationPeriodType},
                        @ManagerProbationId = {employee.ManagerProbationId},
                        @ConfirmDate = {employee.ConfirmDate},
                        @RetirementDate = {employee.RetirementDate},
                        @OfferDate = {employee.OfferDate},
                        @TraineeCompletionPeriod = {employee.TraineeCompletionPeriod},
                        @TraineePeriodType = {employee.TraineePeriodType},
                         @PFNo = {employee.PFNo},
                            @ESICNo = {employee.ESICNo},
                            @NoOfChildren = {employee.NoOfChildren},
                        -- Contact Details
                        @PresentAddress = {employee.PresentAddress},
                        @PresentTehsil = {employee.PresentTehsil},
                        @PresentDistrict = {employee.PresentDistrict},
                        @PresentCity = {employee.PresentCity},
                        @PresentStateId = {employee.PresentStateId},
                        @PresentPincode = {employee.PresentPincode},
                        @PresentThanaId = {employee.PresentThanaId},
                        @PermanentAddress = {employee.PermanentAddress},
                        @PermanentTehsil = {employee.PermanentTehsil},
                        @PermanentDistrict = {employee.PermanentDistrict},
                        @PermanentCity = {employee.PermanentCity},
                        @PermanentStateId = {employee.PermanentStateId},
                        @PermanentPincode = {employee.PermanentPincode},
                        @PermanentThanaId = {employee.PermanentThanaId},
                        @CountryId = {employee.CountryId},
                        @WorkPhone = {employee.WorkPhone},
                        @PersonalPhone = {employee.PersonalPhone},
                        @OfficialEmail = {employee.OfficialEmail},
                        @Nationality = {employee.Nationality},
                        @ExtensionNo = {employee.ExtensionNo},
                        @MobileNo = {employee.MobileNo},
                        @SameAsPresentAddress = {employee.SameAsPresentAddress},

                        -- Salary Report
                        @PrimaryPaymentMode = {employee.PrimaryPaymentMode},
                        @PrimaryBankName = {employee.PrimaryBankName},
                        @PrimaryIFSCCode = {employee.PrimaryIFSCCode},
                        @PrimaryAccountNumber = {employee.PrimaryAccountNumber},
                        @PrimaryBankBranchName = {employee.PrimaryBankBranchName},
                        @WagesTypes = {employee.WagesTypes},
                        @GroupJoiningDate = {employee.GroupJoiningDate},
                        @BusinessSegmentId = {employee.BusinessSegmentId},
                        @EmployeeSalaryReport = {employee.EmployeeSalaryReport},
                        @EmployeePFReport = {employee.EmployeePFReport},
                        @EmployeePTReport = {employee.EmployeePTReport},
                        @EmployeeTaxReport = {employee.EmployeeTaxReport},
                        @EmployeeESIReport = {employee.EmployeeESIReport},
                        @EmployeeNamePrmaryBank = {employee.EmployeeNamePrmaryBank},

                        -- Flags
                        @Overtime = {employee.Overtime},
                        @Latemark = {employee.Latemark},
                        @Earlymark = {employee.Earlymark},
                        @Fullpf = {employee.Fullpf},
                        @Fixsalary = {employee.Fixsalary},
                        @Probation = {employee.Probation},
                        @Trainee = {employee.Trainee},

                        -- Optional Audit Fieldssdjjds
                        @IsDeleted = {employee.IsDeleted},
                        @IsEnabled = {employee.IsEnabled},
                        @IsBlocked = {employee.IsBlocked},
                        @CreatedDate = {employee.CreatedDate},
                        @CreatedBy = {employee.CreatedBy},
                        @DeletedDate = {employee.DeletedDate},
                        @DeletedBy = {employee.DeletedBy}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }



        public async Task<APIResponse> CreateEmployee(vmUpdateEmployee employee)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Action", "CREATE");
                    parameters.Add("@Initial", employee.Initial);
                    parameters.Add("@FirstName", employee.FirstName);
                    parameters.Add("@MiddleName", employee.MiddleName);
                    parameters.Add("@LastName", employee.LastName);
                    parameters.Add("@FullName", employee.FullName);
                    parameters.Add("@EmployeeCode", employee.EmployeeCode);
                    parameters.Add("@AlfaCode", employee.AlfaCode);
                    parameters.Add("@AlfaEmployeeCode", employee.AlfaEmployeeCode);
                    parameters.Add("@DateOfJoining", employee.DateOfJoining);
                    parameters.Add("@BranchId", employee.BranchId);
                    parameters.Add("@GradeId", employee.GradeId);
                    parameters.Add("@ShiftMasterId", employee.ShiftMasterId);
                    parameters.Add("@CTC", employee.CTC);
                    parameters.Add("@DesignationId", employee.DesignationId);
                    parameters.Add("@GrossSalary", employee.GrossSalary);
                    parameters.Add("@CategoryId", employee.CategoryId);
                    parameters.Add("@BasicSalary", employee.BasicSalary);
                    parameters.Add("@DepartmentId", employee.DepartmentId);
                    parameters.Add("@EmployeeTypeId", employee.EmployeeTypeId);
                    parameters.Add("@DateOfBirth", employee.DateOfBirth);
                    parameters.Add("@LoginAlias", employee.LoginAlias);
                    parameters.Add("@Password", employee.Password);
                    parameters.Add("@ReportingManagerId", employee.ReportingManagerId);
                    parameters.Add("@SubBranch", employee.SubBranch);
                    parameters.Add("@EnrollNo", employee.EnrollNo);
                    parameters.Add("@CompanyId", employee.CompanyId);
                    parameters.Add("@Overtime", employee.Overtime);
                    parameters.Add("@Latemark", employee.Latemark);
                    parameters.Add("@Earlymark", employee.Earlymark);
                    parameters.Add("@Fullpf", employee.Fullpf);
                    parameters.Add("@Pt", employee.Pt);
                    parameters.Add("@Fixsalary", employee.Fixsalary);
                    parameters.Add("@Probation", employee.Probation);
                    parameters.Add("@Trainee", employee.Trainee);
                    parameters.Add("@IsPFApplicable", employee.IsPFApplicable);
                    parameters.Add("@WeekOffDetailsId", employee.WeekOffDetailsId);
                    parameters.Add("@IsPermissionPunchInOut", employee.IsPermissionPunchInOut);
                    parameters.Add("@Gender", employee.Gender);
                    parameters.Add("@PersonalEmailId", employee.PersonalEmailId);
                    parameters.Add("@FatherName", employee.FatherName);
                    parameters.Add("@MotherName", employee.MotherName);
                    parameters.Add("@BloodGroup", employee.BloodGroup);
                    parameters.Add("@Height", employee.Height);
                    parameters.Add("@MaritalStatus", employee.MaritalStatus);
                    parameters.Add("@MarriageDate", employee.MarriageDate);
                    parameters.Add("@MarkIdentification", employee.MarkIdentification);
                    parameters.Add("@Religion", employee.Religion);
                    parameters.Add("@Caste", employee.Caste);
                    parameters.Add("@CastCategory", employee.CastCategory);
                    parameters.Add("@AadharCardNo", employee.AadharCardNo);
                    parameters.Add("@PANNo", employee.PANNo);
                    parameters.Add("@Dispensary", employee.Dispensary);
                    parameters.Add("@DoctorName", employee.DoctorName);
                    parameters.Add("@DispensaryAddress", employee.DispensaryAddress);
                    parameters.Add("@UANNumber", employee.UANNumber);
                    parameters.Add("@DrivingLicense", employee.DrivingLicense);
                    parameters.Add("@DrivingLicenseExpiry", employee.DrivingLicenseExpiry);
                    parameters.Add("@RationCardType", employee.RationCardType);
                    parameters.Add("@RationCardNo", employee.RationCardNo);
                    parameters.Add("@ProbationCompletionPeriod", employee.ProbationCompletionPeriod);
                    parameters.Add("@ProbationPeriodType", employee.ProbationPeriodType);
                    parameters.Add("@ManagerProbationId", employee.ManagerProbationId);
                    parameters.Add("@ConfirmDate", employee.ConfirmDate);
                    parameters.Add("@RetirementDate", employee.RetirementDate);
                    parameters.Add("@OfferDate", employee.OfferDate);
                    parameters.Add("@TraineeCompletionPeriod", employee.TraineeCompletionPeriod);
                    parameters.Add("@TraineePeriodType", employee.TraineePeriodType);
                    parameters.Add("@NoOfChildren", employee.NoOfChildren);
                    parameters.Add("@ESICNo", employee.ESICNo);
                    parameters.Add("@PFNo", employee.PFNo);
                    parameters.Add("@PresentAddress", employee.PresentAddress);
                    parameters.Add("@PresentTehsil", employee.PresentTehsil);
                    parameters.Add("@PresentDistrict", employee.PresentDistrict);
                    parameters.Add("@PresentCity", employee.PresentCity);
                    parameters.Add("@PresentStateId", employee.PresentStateId);
                    parameters.Add("@PresentPincode", employee.PresentPincode);
                    parameters.Add("@PresentThanaId", employee.PresentThanaId);
                    parameters.Add("@PermanentAddress", employee.PermanentAddress);
                    parameters.Add("@PermanentTehsil", employee.PermanentTehsil);
                    parameters.Add("@PermanentDistrict", employee.PermanentDistrict);
                    parameters.Add("@PermanentCity", employee.PermanentCity);
                    parameters.Add("@PermanentStateId", employee.PermanentStateId);
                    parameters.Add("@PermanentPincode", employee.PermanentPincode);
                    parameters.Add("@PermanentThanaId", employee.PermanentThanaId);
                    parameters.Add("@CountryId", employee.CountryId);
                    parameters.Add("@WorkPhone", employee.WorkPhone);
                    parameters.Add("@PersonalPhone", employee.PersonalPhone);
                    parameters.Add("@OfficialEmail", employee.OfficialEmail);
                    parameters.Add("@Nationality", employee.Nationality);
                    parameters.Add("@ExtensionNo", employee.ExtensionNo);
                    parameters.Add("@MobileNo", employee.MobileNo);
                    parameters.Add("@SameAsPresentAddress", employee.SameAsPresentAddress);
                    parameters.Add("@PrimaryPaymentMode", employee.PrimaryPaymentMode);
                    parameters.Add("@PrimaryBankName", employee.PrimaryBankName);
                    parameters.Add("@PrimaryIFSCCode", employee.PrimaryIFSCCode);
                    parameters.Add("@PrimaryAccountNumber", employee.PrimaryAccountNumber);
                    parameters.Add("@PrimaryBankBranchName", employee.PrimaryBankBranchName);
                    parameters.Add("@WagesTypes", employee.WagesTypes);
                    parameters.Add("@GroupJoiningDate", employee.GroupJoiningDate);
                    parameters.Add("@BusinessSegmentId", employee.BusinessSegmentId);
                    parameters.Add("@EmployeeSalaryReport", employee.EmployeeSalaryReport);
                    parameters.Add("@EmployeePFReport", employee.EmployeePFReport);
                    parameters.Add("@EmployeePTReport", employee.EmployeePTReport);
                    parameters.Add("@EmployeeTaxReport", employee.EmployeeTaxReport);
                    parameters.Add("@EmployeeESIReport", employee.EmployeeESIReport);
                    parameters.Add("@EmployeeNamePrmaryBank", employee.EmployeeNamePrmaryBank);
                    parameters.Add("@CreatedBy", employee.CreatedBy);

                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);
                    parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("usp_ManageEmployee", parameters, commandType: CommandType.StoredProcedure);

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = parameters.Get<int?>("@Id");
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred: {ex.Message}";
                response.Data = null;
            }
            return response;
        }

        public async Task<APIResponse> UpdateBasicInfo(vmUpdateEmployee employee)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Action", "UPDATE_BASIC");
                    parameters.Add("@Id", employee.Id);
                    parameters.Add("@Initial", employee.Initial);
                    parameters.Add("@FirstName", employee.FirstName);
                    parameters.Add("@MiddleName", employee.MiddleName);
                    parameters.Add("@LastName", employee.LastName);
                    parameters.Add("@FullName", employee.FullName);
                    parameters.Add("@EmployeeCode", employee.EmployeeCode);
                    parameters.Add("@AlfaCode", employee.AlfaCode);
                    parameters.Add("@AlfaEmployeeCode", employee.AlfaEmployeeCode);
                    parameters.Add("@DateOfJoining", employee.DateOfJoining);
                    parameters.Add("@BranchId", employee.BranchId);
                    parameters.Add("@GradeId", employee.GradeId);
                    parameters.Add("@ShiftMasterId", employee.ShiftMasterId);
                    parameters.Add("@CTC", employee.CTC);
                    parameters.Add("@DesignationId", employee.DesignationId);
                    parameters.Add("@GrossSalary", employee.GrossSalary);
                    parameters.Add("@CategoryId", employee.CategoryId);
                    parameters.Add("@BasicSalary", employee.BasicSalary);
                    parameters.Add("@DepartmentId", employee.DepartmentId);
                    parameters.Add("@EmployeeTypeId", employee.EmployeeTypeId);
                    parameters.Add("@DateOfBirth", employee.DateOfBirth);
                    parameters.Add("@RoleId", employee.RoleId);
                    parameters.Add("@LoginAlias", employee.LoginAlias);
                    parameters.Add("@ReportingManagerId", employee.ReportingManagerId);
                    parameters.Add("@SubBranch", employee.SubBranch);
                    parameters.Add("@EnrollNo", employee.EnrollNo);
                    parameters.Add("@CompanyId", employee.CompanyId);
                    parameters.Add("@Overtime", employee.Overtime);
                    parameters.Add("@Latemark", employee.Latemark);
                    parameters.Add("@Earlymark", employee.Earlymark);
                    parameters.Add("@Fullpf", employee.Fullpf);
                    parameters.Add("@Pt", employee.Pt);
                    parameters.Add("@Fixsalary", employee.Fixsalary);
                    parameters.Add("@Probation", employee.Probation);
                    parameters.Add("@Trainee", employee.Trainee);
                    parameters.Add("@IsPFApplicable", employee.IsPFApplicable);
                    parameters.Add("@WeekOffDetailsId", employee.WeekOffDetailsId);
                    parameters.Add("@IsPermissionPunchInOut", employee.IsPermissionPunchInOut);
                    parameters.Add("@UpdatedBy", employee.UpdatedBy);

                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync("usp_ManageEmployee", parameters, commandType: CommandType.StoredProcedure);

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = employee.Id;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred: {ex.Message}";
                response.Data = null;
            }
            return response;
        }

        public async Task<APIResponse> UpdatePersonalInfo(vmUpdateEmployee employee)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Action", "UPDATE_PERSONAL");
                    parameters.Add("@Id", employee.Id);
                    parameters.Add("@Gender", employee.Gender);
                    parameters.Add("@PersonalEmailId", employee.PersonalEmailId);
                    parameters.Add("@FatherName", employee.FatherName);
                    parameters.Add("@MotherName", employee.MotherName);
                    parameters.Add("@BloodGroup", employee.BloodGroup);
                    parameters.Add("@Height", employee.Height);
                    parameters.Add("@MaritalStatus", employee.MaritalStatus);
                    parameters.Add("@MarriageDate", employee.MarriageDate);
                    parameters.Add("@MarkIdentification", employee.MarkIdentification);
                    parameters.Add("@Religion", employee.Religion);
                    parameters.Add("@Caste", employee.Caste);
                    parameters.Add("@CastCategory", employee.CastCategory);
                    parameters.Add("@AadharCardNo", employee.AadharCardNo);
                    parameters.Add("@PANNo", employee.PANNo);
                    parameters.Add("@Dispensary", employee.Dispensary);
                    parameters.Add("@DoctorName", employee.DoctorName);
                    parameters.Add("@DispensaryAddress", employee.DispensaryAddress);
                    parameters.Add("@UANNumber", employee.UANNumber);
                    parameters.Add("@DrivingLicense", employee.DrivingLicense);
                    parameters.Add("@DrivingLicenseExpiry", employee.DrivingLicenseExpiry);
                    parameters.Add("@RationCardType", employee.RationCardType);
                    parameters.Add("@RationCardNo", employee.RationCardNo);
                    parameters.Add("@ProbationCompletionPeriod", employee.ProbationCompletionPeriod);
                    parameters.Add("@ProbationPeriodType", employee.ProbationPeriodType);
                    parameters.Add("@ManagerProbationId", employee.ManagerProbationId);
                    parameters.Add("@ConfirmDate", employee.ConfirmDate);
                    parameters.Add("@RetirementDate", employee.RetirementDate);
                    parameters.Add("@OfferDate", employee.OfferDate);
                    parameters.Add("@TraineeCompletionPeriod", employee.TraineeCompletionPeriod);
                    parameters.Add("@TraineePeriodType", employee.TraineePeriodType);
                    parameters.Add("@NoOfChildren", employee.NoOfChildren);
                    parameters.Add("@ESICNo", employee.ESICNo);
                    parameters.Add("@PFNo", employee.PFNo);
                    parameters.Add("@UpdatedBy", employee.UpdatedBy);

                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync("usp_ManageEmployee", parameters, commandType: CommandType.StoredProcedure);

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = employee.Id;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred: {ex.Message}";
                response.Data = null;
            }
            return response;
        }

        public async Task<APIResponse> UpdateContactInfo(vmUpdateEmployee employee)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Action", "UPDATE_CONTACT");
                    parameters.Add("@Id", employee.Id);
                    parameters.Add("@PresentAddress", employee.PresentAddress);
                    parameters.Add("@PresentTehsil", employee.PresentTehsil);
                    parameters.Add("@PresentDistrict", employee.PresentDistrict);
                    parameters.Add("@PresentCity", employee.PresentCity);
                    parameters.Add("@PresentStateId", employee.PresentStateId);
                    parameters.Add("@PresentPincode", employee.PresentPincode);
                    parameters.Add("@PresentThanaId", employee.PresentThanaId);
                    parameters.Add("@PermanentAddress", employee.PermanentAddress);
                    parameters.Add("@PermanentTehsil", employee.PermanentTehsil);
                    parameters.Add("@PermanentDistrict", employee.PermanentDistrict);
                    parameters.Add("@PermanentCity", employee.PermanentCity);
                    parameters.Add("@PermanentStateId", employee.PermanentStateId);
                    parameters.Add("@PermanentPincode", employee.PermanentPincode);
                    parameters.Add("@PermanentThanaId", employee.PermanentThanaId);
                    parameters.Add("@CountryId", employee.CountryId);
                    parameters.Add("@WorkPhone", employee.WorkPhone);
                    parameters.Add("@PersonalPhone", employee.PersonalPhone);
                    parameters.Add("@OfficialEmail", employee.OfficialEmail);
                    parameters.Add("@Nationality", employee.Nationality);
                    parameters.Add("@ExtensionNo", employee.ExtensionNo);
                    parameters.Add("@MobileNo", employee.MobileNo);
                    parameters.Add("@SameAsPresentAddress", employee.SameAsPresentAddress);
                    parameters.Add("@UpdatedBy", employee.UpdatedBy);

                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync("usp_ManageEmployee", parameters, commandType: CommandType.StoredProcedure);

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = employee.Id;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred: {ex.Message}";
                response.Data = null;
            }
            return response;
        }

        public async Task<APIResponse> UpdateSalaryInfo(vmUpdateEmployee employee)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Action", "UPDATE_SALARY");
                    parameters.Add("@Id", employee.Id);
                    parameters.Add("@PrimaryPaymentMode", employee.PrimaryPaymentMode);
                    parameters.Add("@PrimaryBankName", employee.PrimaryBankName);
                    parameters.Add("@PrimaryIFSCCode", employee.PrimaryIFSCCode);
                    parameters.Add("@PrimaryAccountNumber", employee.PrimaryAccountNumber);
                    parameters.Add("@PrimaryBankBranchName", employee.PrimaryBankBranchName);
                    parameters.Add("@WagesTypes", employee.WagesTypes);
                    parameters.Add("@GroupJoiningDate", employee.GroupJoiningDate);
                    parameters.Add("@BusinessSegmentId", employee.BusinessSegmentId);
                    parameters.Add("@EmployeeSalaryReport", employee.EmployeeSalaryReport);
                    parameters.Add("@EmployeePFReport", employee.EmployeePFReport);
                    parameters.Add("@EmployeePTReport", employee.EmployeePTReport);
                    parameters.Add("@EmployeeTaxReport", employee.EmployeeTaxReport);
                    parameters.Add("@EmployeeESIReport", employee.EmployeeESIReport);
                    parameters.Add("@EmployeeNamePrmaryBank", employee.EmployeeNamePrmaryBank);
                    parameters.Add("@UpdatedBy", employee.UpdatedBy);

                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync("usp_ManageEmployee", parameters, commandType: CommandType.StoredProcedure);

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = employee.Id;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred: {ex.Message}";
                response.Data = null;
            }
            return response;
        }

        public async Task<APIResponse> DeleteEmployee(DeleteRecordVM deleteRecord)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Action", "DELETE");
                    parameters.Add("@Id", deleteRecord.Id);
                    parameters.Add("@DeletedBy", deleteRecord.DeletedBy);

                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync("usp_ManageEmployee", parameters, commandType: CommandType.StoredProcedure);

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = deleteRecord.Id;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"An error occurred: {ex.Message}";
                response.Data = null;
            }
            return response;
        }


        //public async Task<VMCommonResult> DeleteEmployee(DeleteRecordVM deleteRecord)
        //{
        //    try
        //    {
        //        var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
        //            EXEC ManageEmployee
        //                @Action = {"DELETE"},
        //                @Id = {deleteRecord.emp_id},
        //                @DeletedDate = {deleteRecord.DeletedDate},
        //                @DeletedBy = {deleteRecord.DeletedBy}
        //        ").ToListAsync();

        //        return result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
        //    }
        //    catch (Exception)
        //    {
        //        return new VMCommonResult { Id = 0 };
        //    }
        //}


        public async Task<List<vmGetAllEmployee_DropDown>> GetAllEmployee_DropDown(int companyId, string BranchId)
        {
            try
            {
                return await _db.Set<vmGetAllEmployee_DropDown>()
                                .FromSqlInterpolated($"EXEC USP_GetAllEmployee_DropDown @companyId={companyId} , @BranchIds={BranchId}")
                                .ToListAsync();
            }
            catch (Exception)
            {
                return new List<vmGetAllEmployee_DropDown>();
            }
        }

        public async Task<List<vmGetAllEmployee>> GetAllEmployee(int companyId)
        {
            try
            {
                return await _db.Set<vmGetAllEmployee>()
                                .FromSqlInterpolated($"EXEC GetAllEmployee @companyId={companyId}")
                                .ToListAsync();
            }
            catch (Exception)
            {
                return new List<vmGetAllEmployee>();
            }
        }
        public async Task<List<vmUpdateEmployee>> GetAllEmployeeForUpdate(int companyId)
        {
            try
            {
                return await _db.Set<vmUpdateEmployee>()
                                .FromSqlInterpolated($"EXEC GetAllEmployeeForUpdate @companyId={companyId}")
                                .ToListAsync();
            }
            catch (Exception)
            {
                return new List<vmUpdateEmployee>();
            }
        }

        public async Task<List<vmGetAllEmployee>> GetAllEmployeeActiveOrLeft(bool IsLeft, int companyId)
        {
            try
            {
                return await _db.Set<vmGetAllEmployee>()
                                .FromSqlInterpolated($"EXEC GetAllEmployeeActiveOrLeft  @IsLeft = {IsLeft},@companyId={companyId}")
                                .ToListAsync();
            }
            catch (Exception)
            {
                return new List<vmGetAllEmployee>();
            }
        }

        public async Task<vmGetEmployeeById?> GetEmployeeById(int Id)
        {
            try
            {
                var result = await _db.Set<vmGetEmployeeById>()
                                      .FromSqlInterpolated($"EXEC GetEmployeeById @Id = {Id}")
                                      .ToListAsync();

                return result.FirstOrDefault() ?? null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<VMCommonResult> UpdateEmployeeProfileAndSignature(vmUpdateEmployeeProfile model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC UpdateEmployeeProfileAndSignature
                        @Id = {model.EmployeeId},
                        @EmployeeProfileUrl = {model.EmployeeProfileUrl},
                        @EmployeeSignatureUrl = {model.EmployeeSignatureUrl}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<vmGetNextEmployeeCode?> GetNextEmployeeCode(int companyId)
        {
            try
            {
                var result = await _db.Set<vmGetNextEmployeeCode>()
                    .FromSqlInterpolated($"EXEC GetNextEmployeeCode @CompanyId = {companyId}")
                    .ToListAsync();

                return result.FirstOrDefault() ?? null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<VMGetExistEmployeeCode?> GetExistEmployeeCode(vmCommonParameters vmCommonParameters)
        {
            try
            {
                var result = await _db.Set<VMGetExistEmployeeCode>()
                    .FromSqlInterpolated($"EXEC GetExistEmployeeCode @CompanyId = {vmCommonParameters.CompanyId},@EmployeeCode = {vmCommonParameters.EmployeeCode}")
                    .ToListAsync();

                return result.FirstOrDefault() ?? null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<VMCommonResult> UpdateLastLogin(int empid, int compId)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC sp_UpdateLastLogin 
                       
                        @Empid = {empid},
                        @Compid = {compId}
                ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<List<EmployeePersonalInformationVM>> EmployeePersonalInformation(int empid, int compId)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@compid", compId),
                    new SqlParameter("@empid", empid)

                };

                var result = await _db.Set<EmployeePersonalInformationVM>()
                    .FromSqlRaw("EXEC EmployeePersonalInformation @compid ,@empid", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<EmployeePersonalInformationVM>();
            }


        }

        public async Task<vmUserLogin?> UserLogin(vmLogin login)
        {
            try
            {
                var result = await _db.Set<vmUserLogin>()
                                .FromSqlInterpolated($"EXEC SP_UserLogin @Email={login.Email},@Password={login.Password}")
                                .ToListAsync();
                return result.FirstOrDefault() ?? null;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<APIResponse> GetRecordsForAdd(int CompanyId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var multi = await connection.QueryMultipleAsync(
                        "GetRecordsForAdd",
                        new { CompanyId },
                        commandType: CommandType.StoredProcedure))
                    {
                        var result = new EmployeeUpdateViewModel
                        {
                            Branches = (await multi.ReadAsync<BranchViewModel>()).AsList(),
                            Grades = (await multi.ReadAsync<GradeViewModel>()).AsList(),
                            Shifts = (await multi.ReadAsync<ShiftViewModel>()).AsList(),
                            Designations = (await multi.ReadAsync<DesignationViewModel>()).AsList(),
                            Categories = (await multi.ReadAsync<CategoryViewModel>()).AsList(),
                            Departments = (await multi.ReadAsync<DepartmentViewModel>()).AsList(),
                            EmployeeTypes = (await multi.ReadAsync<EmployeeTypeViewModel>()).AsList(),
                            Roles = (await multi.ReadAsync<RoleViewModel>()).AsList(),
                            ReportingEmployees = (await multi.ReadAsync<ReportingEmployeeViewModel>()).AsList()
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
        public async Task<APIResponse> GetRecordsForUpdate(CommonParameter param)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var multi = await connection.QueryMultipleAsync(
                        "GetRecordsForUpdate",
                        new { param.CompanyId,param.EmployeeId },
                        commandType: CommandType.StoredProcedure))
                    {
                        var result = new EmployeeUpdateViewModel
                        {
                            Employees = (await multi.ReadAsync<EmployeeDetailViewModel>()).FirstOrDefault(),
                            Branches = (await multi.ReadAsync<BranchViewModel>()).AsList(),
                            Grades = (await multi.ReadAsync<GradeViewModel>()).AsList(),
                            Shifts = (await multi.ReadAsync<ShiftViewModel>()).AsList(),
                            Designations = (await multi.ReadAsync<DesignationViewModel>()).AsList(),
                            Categories = (await multi.ReadAsync<CategoryViewModel>()).AsList(),
                            Departments = (await multi.ReadAsync<DepartmentViewModel>()).AsList(),
                            EmployeeTypes = (await multi.ReadAsync<EmployeeTypeViewModel>()).AsList(),
                            Roles = (await multi.ReadAsync<RoleViewModel>()).AsList(),
                            ReportingEmployees = (await multi.ReadAsync<ReportingEmployeeViewModel>()).AsList()
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
        public async Task<APIResponse> GetReportingList()
        {
            try
            {
                var ReportingEmployees = new List<ReportingEmployeeViewModel>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var multi = await connection.QueryMultipleAsync(
                        "GetReportingList",
                        commandType: CommandType.StoredProcedure))
                    {

                        ReportingEmployees = (await multi.ReadAsync<ReportingEmployeeViewModel>()).AsList();
                    };


                    return new APIResponse { Data = ReportingEmployees, ResponseMessage = "Fetched successfully!", isSuccess = true };

                }
            }
            catch (Exception)
            {
                return new APIResponse { ResponseMessage = "Some thing Went wrong!", isSuccess = false };
            }
        }
        public async Task<APIResponse> GetEmployeeListByBranchId(int BranchId)
        {
            try
            {
                var EmployeesList = new List<vmGetEmployeeListByBranchId>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var multi = await connection.QueryMultipleAsync(
                        "GetEmployeeListByBranchId",
                          new { BranchId },
                        commandType: CommandType.StoredProcedure))
                    {

                        EmployeesList = (await multi.ReadAsync<vmGetEmployeeListByBranchId>()).AsList();
                    };


                    return new APIResponse { Data = EmployeesList, ResponseMessage = "Fetched successfully!", isSuccess = true };

                }
            }
            catch (Exception)
            {
                return new APIResponse { ResponseMessage = "Some thing Went wrong!", isSuccess = false };
            }
        }
    

    
        public async Task<APIResponse> GetUpdateEmployeeById(int id, string? action = null)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                        "usp_GetEmployeeById",
                        new { Id = id, Action = action },
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null)
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "Employee not found.";
                        return response;
                    }

                    response.isSuccess = true;
                    response.ResponseMessage = "Success!";
                    response.Data = result; // Return dynamic object directly
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }
        public async Task<APIResponse> GetGradeBySalaryRange(vmEmployeeSalary salaryPara)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                        "USP_GetGradeBySalaryRange",
                        new { CompanyId= salaryPara.CompanyId, GrossSalary = salaryPara.GrossSalary, BasicSalary = salaryPara.BasicSalary, IsPFApplicable=salaryPara.IsPFApplicable },
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null)
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "Grade not found.";
                        return response;
                    }

                    response.isSuccess = true;
                    response.ResponseMessage = "Success!";
                    response.Data = result; // Return dynamic object directly
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }
        public async Task<APIResponse> GetEmplyeeDetailsById(int EmployeeId)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                        "GetEmplyeeDetailsById",
                        new { EmployeeId = EmployeeId},
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null)
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "Record not found.";
                        return response;
                    }

                    response.isSuccess = true;
                    response.ResponseMessage = "Success!";
                    response.Data = result; // Return dynamic object directly
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

    }
}
