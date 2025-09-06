using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Probations
{
    public class ProbationEmployeeVM
    {
        public int? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? ProbationEndDate { get; set; }
        public string? ReviewType { get; set; }
        public int? ExtendedInDays { get; set; }
        public DateTime? EvaluationDate { get; set; }
        public string? BranchName { get; set; }
    }
}
