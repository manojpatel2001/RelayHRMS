using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Probations
{
    public class ProbationEvaluationFormListVM
    {
        public int EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public string? EmployeeEmail { get; set; }
        public DateTime? ProbationEndDate { get; set; }
        public string? GradeName { get; set; }
        public string? GradeCategory { get; set; }
        public int? ReportingManagerId { get; set; }
        public int? DepartmentId { get; set; }
        public int? CompanyId { get; set; }
        public string? BranchName { get; set; }
        public string? AlertStatus { get; set; }
        public int? ReceiverUserId { get; set; }
        public bool IsUploaded { get; set; }
        public string? FormUrl { get; set; }
        public int? ProbationEvaluationFormId { get; set; }
    }
}
