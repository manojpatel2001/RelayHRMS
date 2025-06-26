using HRMS_Core.DbContext;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.EmployeeMaster
{
    [Table("EmployeePersonalInfo")]
    public class EmployeePersonalInfo:BaseModel
    {
        [Key]
        public int EmployeePersonalInfoId { get; set; }
        public string? Gender { get; set; }
        public string? PersonalEmailId { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public DateTime? DateOfBirth { get; set; }
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
        public string? LinkedInId { get; set; }
        public string? TwitterId { get; set; }
        public string? ProbationCompletionPeriod { get; set; }
        public string? ProbationPeriodType { get; set; }
        public string? ManagerProbation { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public DateTime? RetirementDate { get; set; }
        public DateTime? OfferDate { get; set; }
        public string? TraineeCompletionPeriod { get; set; }
        public string? TraineePeriodType { get; set; }
        public string? CanteenCode { get; set; }
        public string? TallyLedgerName { get; set; }
        public bool? IsMetroCity { get; set; } = false;
        public string? AdultWorkerNo { get; set; }
        public string? PhysicalDisability { get; set; }
        public string? MinimumWageSkillType { get; set; }
        public string? VehicleNo { get; set; }
        public bool? InsuranceNo { get; set; } = false;
        public string?  DressCode { get; set; }
        public string? ShirtSize { get; set; }
        public string? PantSize { get; set; }
        public string? ShoeSize { get; set; }

        public int? EmployeeId { get; set; }
        
    }

}
