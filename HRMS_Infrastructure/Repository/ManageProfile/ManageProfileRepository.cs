using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.ProfileManage;
using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Infrastructure.Interface.ManageProfile;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace HRMS_Infrastructure.Repository.ManageProfile
{
    public class ManageProfileRepository: IManageProfileRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;

        public ManageProfileRepository(HRMSDbContext db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;


        }

        // Fetch Personal Info details
        public async Task<vmPersonalInfo?> GetPersonalInfo(int employeeId)
        {
            try
            {
                var result = await _db.Set<vmPersonalInfo>()
                    .FromSqlInterpolated($"EXEC GetProfileDetails @Action = {"PersionalInfo"}, @EmployeeId = {employeeId}")
                    .ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        // Fetch Contact details
        public async Task<vmContactDetails?> GetContactDetails(int employeeId)
        {
            try
            {
                var result = await _db.Set<vmContactDetails>()
                    .FromSqlInterpolated($"EXEC GetProfileDetails @Action = {"Contact"}, @EmployeeId = {employeeId}")
                    .ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        // Fetch Salary details
        public async Task<vmSalaryDetails?> GetSalaryDetails(int employeeId)
        {
            try
            {
                var result = await _db.Set<vmSalaryDetails>()
                    .FromSqlInterpolated($"EXEC GetProfileDetails @Action = {"Salary"}, @EmployeeId = {employeeId}")
                    .ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        // Update Personal Info details
        public async Task<SP_Response> UpdatePersonalInfo(vmPersonalInfo vmPersonalInfo)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                        EXEC UpdateProfile
                            @Action = {"PersonalInfo"},
                            @DepartmentId = {vmPersonalInfo.DepartmentId},
                            @ShiftId = {vmPersonalInfo.ShiftId},
                            @UpdatedBy = {vmPersonalInfo.UpdatedBy},
                            @EmployeeId = {vmPersonalInfo.EmployeeId},
                            @Gender = {vmPersonalInfo.Gender},
                            @PersonalEmailId = {vmPersonalInfo.PersonalEmailId},
                            @FatherName = {vmPersonalInfo.FatherName},
                            @MotherName = {vmPersonalInfo.MotherName},
                            @BloodGroup = {vmPersonalInfo.BloodGroup},
                            @Height = {vmPersonalInfo.Height},
                            @MaritalStatus = {vmPersonalInfo.MaritalStatus},
                            @MarriageDate = {vmPersonalInfo.MarriageDate},
                            @MarkIdentification = {vmPersonalInfo.MarkIdentification},
                            @Religion = {vmPersonalInfo.Religion},
                            @Caste = {vmPersonalInfo.Caste},
                            @CastCategory = {vmPersonalInfo.CastCategory},
                            @AadharCardNo = {vmPersonalInfo.AadharCardNo},
                            @PANNo = {vmPersonalInfo.PANNo},
                            @Dispensary = {vmPersonalInfo.Dispensary},
                            @DoctorName = {vmPersonalInfo.DoctorName},
                            @DispensaryAddress = {vmPersonalInfo.DispensaryAddress},
                            @UANNumber = {vmPersonalInfo.UANNumber},
                            @DrivingLicense = {vmPersonalInfo.DrivingLicense},
                            @DrivingLicenseExpiry = {vmPersonalInfo.DrivingLicenseExpiry},
                            @RationCardType = {vmPersonalInfo.RationCardType},
                            @RationCardNo = {vmPersonalInfo.RationCardNo},
                            @ProbationCompletionPeriod = {vmPersonalInfo.ProbationCompletionPeriod},
                            @ProbationPeriodType = {vmPersonalInfo.ProbationPeriodType},
                            @ManagerProbationId = {vmPersonalInfo.ManagerProbationId},
                            @ConfirmDate = {vmPersonalInfo.ConfirmDate},
                            @RetirementDate = {vmPersonalInfo.RetirementDate},
                            @OfferDate = {vmPersonalInfo.OfferDate},
                            @TraineeCompletionPeriod = {vmPersonalInfo.TraineeCompletionPeriod},
                            @TraineePeriodType = {vmPersonalInfo.TraineePeriodType},
                            @PFNo = {vmPersonalInfo.PFNo},
                            @ESICNo = {vmPersonalInfo.ESICNo},
                            @NoOfChildren = {vmPersonalInfo.NoOfChildren}
                    ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        // Update Contact details
        public async Task<SP_Response> UpdateContactDetails(vmContactDetails vmContactDetails)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                        EXEC UpdateProfile
                            @Action = {"Contact"},
                            @EmployeeId = {vmContactDetails.EmployeeId},
                            @PresentAddress = {vmContactDetails.PresentAddress},
                            @PresentTehsil = {vmContactDetails.PresentTehsil},
                            @PresentDistrict = {vmContactDetails.PresentDistrict},
                            @PresentCity = {vmContactDetails.PresentCity},
                            @PresentStateId = {vmContactDetails.PresentStateId},
                            @PresentPincode = {vmContactDetails.PresentPincode},
                            @PresentThanaId = {vmContactDetails.PresentThanaId},
                            @PermanentAddress = {vmContactDetails.PermanentAddress},
                            @PermanentTehsil = {vmContactDetails.PermanentTehsil},
                            @PermanentDistrict = {vmContactDetails.PermanentDistrict},
                            @PermanentCity = {vmContactDetails.PermanentCity},
                            @PermanentStateId = {vmContactDetails.PermanentStateId},
                            @PermanentPincode = {vmContactDetails.PermanentPincode},
                            @PermanentThanaId = {vmContactDetails.PermanentThanaId},
                            @CountryId = {vmContactDetails.CountryId},
                            @WorkPhone = {vmContactDetails.WorkPhone},
                            @PersonalPhone = {vmContactDetails.PersonalPhone},
                            @OfficialEmail = {vmContactDetails.OfficialEmail},
                            @Nationality = {vmContactDetails.Nationality},
                            @ExtensionNo = {vmContactDetails.ExtensionNo},
                            @MobileNo = {vmContactDetails.MobileNo},
                            @SameAsPresentAddress = {vmContactDetails.SameAsPresentAddress}
                    ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        // Update Salary details
        public async Task<SP_Response> UpdateSalaryDetails(vmSalaryDetails vmSalaryDetails)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                        EXEC UpdateProfile
                            @Action = {"Salary"},
                            @EmployeeId = {vmSalaryDetails.EmployeeId},
                            @PrimaryPaymentMode = {vmSalaryDetails.PrimaryPaymentMode},
                            @PrimaryBankName = {vmSalaryDetails.PrimaryBankName},
                            @PrimaryIFSCCode = {vmSalaryDetails.PrimaryIFSCCode},
                            @PrimaryAccountNumber = {vmSalaryDetails.PrimaryAccountNumber},
                            @PrimaryBankBranchName = {vmSalaryDetails.PrimaryBankBranchName},
                            @WagesTypes = {vmSalaryDetails.WagesTypes},
                            @GroupJoiningDate = {vmSalaryDetails.GroupJoiningDate},
                            @BusinessSegmentId = {vmSalaryDetails.BusinessSegmentId},
                            @EmployeeSalaryReport = {vmSalaryDetails.EmployeeSalaryReport},
                            @EmployeePFReport = {vmSalaryDetails.EmployeePFReport},
                            @EmployeePTReport = {vmSalaryDetails.EmployeePTReport},
                            @EmployeeTaxReport = {vmSalaryDetails.EmployeeTaxReport},
                            @EmployeeESIReport = {vmSalaryDetails.EmployeeESIReport},
                            @EmployeeNamePrmaryBank = {vmSalaryDetails.EmployeeNamePrmaryBank}
                    ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<vmGetEmployeeSalaryAllowance?> GetEmployeeSalaryAllowance(int EmployeeId)
        {
            try
            {
                var result = await _db.Set<vmGetEmployeeSalaryAllowance>().FromSqlInterpolated($@"
                EXEC GetEmployeeSalaryAllowance       
                    @EmployeeId = {EmployeeId}
                    
            ").AsNoTracking().ToListAsync();

                return result?.FirstOrDefault() ?? null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<SP_Response> UpdateProfilePic(vmUpdateEmployeeProfile model)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                        EXEC UpdateProfile
                            @Action = {"ProfilePic"},
                            @EmployeeId = {model.EmployeeId},
                            @EmployeeProfileUrl = {model.EmployeeProfileUrl}
                           
                    ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<APIResponse> CheckReportingPerson(vmCommonCheckParameters model)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", model.UserId);
                    parameters.Add("@EmployeeId", model.EmployeeId);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

                    await connection.ExecuteAsync(
                        "CheckReportingPerson",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    response.isSuccess = parameters.Get<bool>("@Success");
                    response.ResponseMessage = parameters.Get<string>("@ResponseMessage");
                    response.Data = null; // You can set this to a relevant object if needed
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
    }
}
