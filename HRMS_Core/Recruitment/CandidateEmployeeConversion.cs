using System;

namespace HRMS_Core.Recruitment
{
    public class CandidateEmployeeConversion
    {
        public int CandidateEmployeeConversionId { get; set; }
        public int CandidateApplicationId { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? AlfaEmployeeCode { get; set; }
        public string? ConversionStatus { get; set; } = "Pending";
        public string? FailureReason { get; set; }
        public DateTime? ConvertedDate { get; set; }
        public int? ConvertedBy { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
