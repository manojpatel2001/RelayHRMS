using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.UpdateEmployee
{

    public class EmployeeDetailViewModel
    {
       public int? Id { get; set; }
        public string? Initial { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? AlfaEmployeeCode { get; set; }
        public string? AlfaCode { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public int? BranchId { get; set; }

        public int? GradeId { get; set; }

        public int? ShiftMasterId { get; set; }

        public string? CTC { get; set; }
        public int? DesignationId { get; set; }

        public decimal? GrossSalary { get; set; }

        public int? CategoryId { get; set; }
        public decimal? BasicSalary { get; set; }
        public int? DepartmentId { get; set; }

        public int? EmployeeTypeId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? UserPrivilege { get; set; }
        public int? RoleId { get; set; }

        public string? LoginAlias { get; set; }
        public string? Password { get; set; }
        public int? ReportingManagerId { get; set; }
        public string? SubBranch { get; set; }
        public string? EnrollNo { get; set; }
        public int? CompanyId { get; set; }

        public bool? Overtime { get; set; } = false;
        public bool? Latemark { get; set; } = false;
        public bool? Earlymark { get; set; } = false;
        public bool? Fullpf { get; set; } = false;
        public bool? Pt { get; set; } = false;
        public bool? Fixsalary { get; set; } = false;
        public bool? Probation { get; set; } = false;
        public bool? Trainee { get; set; } = false;
        public int? WeekOffDetailsId { get; set; }
        public bool? IsPermissionPunchInOut { get; set; } = false;
        public string? CreatedByName { get; set; }
        public bool? IsPFApplicable { get; set; } = true;
        public string? PFNo { get; set; }
        public string? ESICNo { get; set; }
        public string? NoOfChildren { get; set; }


        //base model
        public bool? IsDeleted { get; set; } = false;
        public bool? IsEnabled { get; set; } = true;
        public bool? IsBlocked { get; set; } = false;
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public string? EmployeeProfileUrl { get; set; }
        public string? EmployeeSignatureUrl { get; set; }
        //PersionalInfo
        public string? Gender { get; set; }
        public string? PersonalEmailId { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? BloodGroup { get; set; }
        public string? Height { get; set; }
        public string? MaritalStatus { get; set; }
        public DateTime? MarriageDate { get; set; }
        public string? MarkIdentification { get; set; }
        public string? Religion { get; set; }
        public string? Caste { get; set; }
        public string? CastCategory { get; set; }
        public string? AadharCardNo { get; set; }
        public string? PANNo { get; set; }
        public string? Dispensary { get; set; }
        public string? DoctorName { get; set; }
        public string? DispensaryAddress { get; set; }
        public string? UANNumber { get; set; }
        public string? DrivingLicense { get; set; }
        public DateTime? DrivingLicenseExpiry { get; set; }
        public string? RationCardType { get; set; }
        public string? RationCardNo { get; set; }
        public decimal? ProbationCompletionPeriod { get; set; }
        public string? ProbationPeriodType { get; set; }
        //foreignKey
        public int? ManagerProbationId { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public DateTime? RetirementDate { get; set; }
        public DateTime? OfferDate { get; set; }
        public decimal? TraineeCompletionPeriod { get; set; }
        public string? TraineePeriodType { get; set; }




        //employee contact
        public string? PresentAddress { get; set; }
        public string? PresentTehsil { get; set; }
        public string? PresentDistrict { get; set; }
        public string? PresentCity { get; set; }
        public int? PresentStateId { get; set; }
        public string? PresentPincode { get; set; }
        //forein key ThanaId
        public int? PresentThanaId { get; set; }

        // Permanent Address Details
        public string? PermanentAddress { get; set; }
        public string? PermanentTehsil { get; set; }
        public string? PermanentDistrict { get; set; }
        public string? PermanentCity { get; set; }
        public int? PermanentStateId { get; set; }
        public string? PermanentPincode { get; set; }
        public int? PermanentThanaId { get; set; }
      
        // Contact Information
        public int? CountryId { get; set; }
        public string? WorkPhone { get; set; }
        public string? PersonalPhone { get; set; }
        public string? OfficialEmail { get; set; }
        public string? Nationality { get; set; }
        public string? ExtensionNo { get; set; }
        public string? MobileNo { get; set; }

        // Checkbox for same as present address
        public bool? SameAsPresentAddress { get; set; } = false;


        //salary report
        public string? PrimaryPaymentMode { get; set; }
        public string? PrimaryBankName { get; set; }
        public string? PrimaryIFSCCode{ get; set; }
        public string? PrimaryAccountNumber{ get; set; }
        public string? PrimaryBankBranchName{ get; set; }
        public string? WagesTypes{ get; set; }
        public DateTime? GroupJoiningDate{ get; set; }
        //foreinkey BusinessSegment
        public int? BusinessSegmentId{ get; set; }
        public string? EmployeeSalaryReport{ get; set; }
        public string? EmployeePFReport{ get; set; }
        public string? EmployeePTReport{ get; set; }
        public string? EmployeeTaxReport{ get; set; }
        public string? EmployeeESIReport{ get; set; }
        public string? EmployeeNamePrmaryBank{ get; set; }
    }

}
