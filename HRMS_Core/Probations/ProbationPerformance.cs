using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Probations
{

    public class ProbationPerformance
    {
        public int? ProbationPerformanceId { get; set; }
        public int? EmployeeId { get; set; }
        public int ProbationStatusId { get; set; }
        public int? ProbationEvaluationPeriodId { get; set; }
        public int Rating { get; set; } = 0;
        public DateTime? ProbationEvaluationDate { get; set; }
        public string? RemarksOfApprover { get; set; }
        public int? EmployeeTypeId { get; set; }
        public int? Level { get; set; }
        

        public DateTime? ProbationEndDate { get; set; }
        public DateTime? EscalationDate { get; set; }
        public int ApprovalRequestId { get; set; } = 0;
        public int ApprovalRequestLevelId { get; set; } = 0;
        public int? CreatedBy { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? ApproverName { get; set; }
        public string? ApproverCode { get; set; }
        public int? EvaluationDays { get; set; }
        public string? CompanyName { get; set; }

    }

   
}
