using HRMS_Core.ControlPanel.CompanyInformation;
using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyInformation;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Infrastructure.Interface.EmployeeMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.EmployeeMaster
{
    public class EmployeeManageRepository:Repository<HRMSUserIdentity>, IEmployeeManageRepository
    {
        private HRMSDbContext _db;

        public EmployeeManageRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
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

        public async Task<VMCommonResult> DeleteEmployee(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageEmployee
                        @Action = {"DELETE"},
                        @Id = {deleteRecord.emp_id},
                        @DeletedDate = {deleteRecord.DeletedDate},
                        @DeletedBy = {deleteRecord.DeletedBy}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async  Task<List<vmGetAllEmployee>> GetAllEmployee(int companyId)
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
        public async  Task<List<vmUpdateEmployee>> GetAllEmployeeForUpdate(int companyId)
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

        public async Task<List<vmGetAllEmployee>> GetAllEmployeeByIsBlocked(bool IsBlocked, int companyId)
        {
            try
            {
                return await _db.Set<vmGetAllEmployee>()
                                .FromSqlInterpolated($"EXEC GetAllEmployeeByIsBlocked  @IsBlocked = {IsBlocked},@companyId={companyId}")
                                .ToListAsync();
            }
            catch (Exception)
            {
                return new List<vmGetAllEmployee>();
            }
        }

        public async Task<vmGetAllEmployee?> GetEmployeeById(int Id)
        {
            try
            {
                var result = await _db.Set<vmGetAllEmployee>()
                                      .FromSqlInterpolated($"EXEC GetEmployeeById @Id = {Id}")
                                      .ToListAsync();

                return result.FirstOrDefault() ?? null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async  Task<VMCommonResult> UpdateEmployeeProfileAndSignature(vmUpdateEmployeeProfile model)
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

                return result.FirstOrDefault()??null;
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

                return result.FirstOrDefault()??null;
            }
            catch (Exception)
            {
                return null;
            }
        }


    }
}
