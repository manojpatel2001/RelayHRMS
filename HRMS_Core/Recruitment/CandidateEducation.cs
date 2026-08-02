using System;

namespace HRMS_Core.Recruitment
{
    public class CandidateEducation
    {
        public int CandidateEducationId { get; set; }
        public int CandidateId { get; set; }
        public string? Degree { get; set; }
        public string? Specialization { get; set; }
        public string? Institution { get; set; }
        public string? University { get; set; }
        public int? YearOfPassing { get; set; }
        public string? PercentageOrCGPA { get; set; }
        public string? EducationLevel { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
