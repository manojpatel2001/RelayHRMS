using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Probations
{
    public class ProbationEvaluationForm
    {
        public int ProbationEvaluationFormId { get; set; }
        public int EmployeeId { get; set; }
        public string? ProbationEvaluationFormUrl { get; set; }
        public bool IsUploaded { get; set; } = false;
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
