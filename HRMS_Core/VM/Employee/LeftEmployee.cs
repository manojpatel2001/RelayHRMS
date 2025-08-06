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
        public bool? IsTerminate { get; set; }
        public bool? UniformReturn { get; set; }
        public bool? ExitInterview { get; set; }
        public string? NoticePeriod { get; set; }
        public bool? IsDeath { get; set; }
        public DateTime? RegDate { get; set; }
        public bool? IsFnFApplicable { get; set; }
        public int? RptManagerID { get; set; }
        public bool? IsRetire { get; set; }
        public int? RequestAprID { get; set; }
        public int? LeftReasonValue { get; set; }
        public string? LeftReasonText { get; set; }
        public int? Res_Id { get; set; }
    }
}
