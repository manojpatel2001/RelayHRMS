using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.ProfileManage
{
    public class vmPersonalInfo
    {
        public int? EmployeeId { get; set; }
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
        public int? ProbationCompletionPeriod { get; set; }
        public string? ProbationPeriodType { get; set; }
        public int? ManagerProbationId { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public DateTime? RetirementDate { get; set; }
        public DateTime? OfferDate { get; set; }
        public int? TraineeCompletionPeriod { get; set; }
        public string? TraineePeriodType { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PFNo { get; set; }
        public string? ESICNo { get; set; }
        public string? NoOfChildren { get; set; }
    }
}
