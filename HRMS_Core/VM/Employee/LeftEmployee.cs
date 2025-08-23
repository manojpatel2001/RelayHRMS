using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    [Table("LeftEmployee")]
    public class LeftEmployee:BaseModel
    {
        [Key]
        public int LeftID { get; set; }
        public int? CmpID { get; set; }
        public int? EmpID { get; set; }
        public int? BranchId { get; set; }
        public DateTime? LeftDate { get; set; }
        public string? LeftReason { get; set; }
        public DateTime? RegAcceptDate { get; set; }
        public bool? IsTerminate { get; set; } = false;
        public bool? UniformReturn { get; set; } = false;
        public bool? ExitInterview { get; set; } = false;
        public string? NoticePeriod { get; set; }
        public bool? IsDeath { get; set; } = false;
        public DateTime? RegDate { get; set; }
        public bool? IsFnFApplicable { get; set; } = false;
        public int? RptManagerID { get; set; }
        public bool? IsRetire { get; set; } = false;
        public int? RequestAprID { get; set; }
        public int? LeftReasonValue { get; set; }
        public string? LeftReasonText { get; set; }
        public int? Res_Id { get; set; } 
        public bool IsLeft { get; set; } 
        public string? Remark { get; set; } 
    }
}
