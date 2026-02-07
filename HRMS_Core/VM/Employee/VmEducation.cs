using System;

namespace HRMS_Core.VM.Employee
{
    public class VmEducation
    {
        public int EducationId { get; set; }

        public int? EmployeeId { get; set; }   // Nullable if EmployeeId can be optional

        public string? Education { get; set; }

        public string? Specialization { get; set; }

        public int? Year { get; set; }

        public string? ScoreOrClass { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Comments { get; set; }

        public string? DocumentPath { get; set; }

        public bool? IsDeleted { get; set; } = false;
        public bool? IsEnabled { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }
}
