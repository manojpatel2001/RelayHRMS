using System;

namespace HRMS_Core.Employee
{
    public class EmployeeTraining
    {
        public int EmployeeTrainingId { get; set; }
        public int EmployeeId { get; set; }
        public string TrainingName { get; set; } = string.Empty;
        public string? TrainingType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Provider { get; set; }
        public string Status { get; set; } = "Scheduled";
        public string? Score { get; set; }
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
