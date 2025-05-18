using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.EmployeeMaster
{
    public class EmployeePersonalDetails
    {
        // Personal Information Fields
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

        // Required Documents Fields
        public string? AadharCardNo { get; set; }
        public string? PanNo { get; set; }
        public string? Dispensary { get; set; }
        public string? DoctorName { get; set; }
        public string? DispensaryAddress { get; set; }
        public string? UanNumber { get; set; }
        public string? DrivingLicense { get; set; }
        public DateTime? DrivingLicenseExpiry { get; set; }
        public string? RationCardType { get; set; }
        public string? RationCardNo { get; set; }

        // Social Address Fields
        public string? LinkedInId { get; set; }
        public string? TwitterId { get; set; }

        // Training and Probation Fields
        public string? ProbationCompletionPeriod { get; set; }
        public string? ManagerProbation { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public DateTime? RetirementDate { get; set; }
        public DateTime? OfferDate { get; set; }
        public string? TraineeCompletionPeriod { get; set; }

        // Other Details Fields
        public string? CanteenCode { get; set; }
        public string? TallyLedgerName { get; set; }
        public string? IsMetroCity { get; set; }
        public int? AdultWorkerNo { get; set; }
        public string? PhysicalDisability { get; set; }
        public string? MinimumWageSkillType { get; set; }
        public string? VehicleNo { get; set; }
        public string? InsuranceNo { get; set; }

        // Uniform Details Fields
        public string? DressCode { get; set; }
        public string? ShirtSize { get; set; }
        public string? PantSize { get; set; }
        public string? ShoeSize { get; set; }
    }

}
