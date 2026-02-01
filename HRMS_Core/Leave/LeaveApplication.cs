using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Leave
{
    [Table("LeaveApplication")]
    public class LeaveApplication : BaseModel
    {
        [Key]
        public int? LeaveApplicationid { get; set; }
        public int? EmplooyeId { get; set; }
        public int? CompId { get; set; }
        public int? ReportingManagerId { get; set; }
        public int? LeaveType { get; set; }
        public string? ApplicationType { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public DateTime? FromDate { get; set; }
        public decimal? No_Of_Date { get; set; }
        public DateTime? Todate { get; set; }
        public string? Reason { get; set; }
        public int? Responsibleperson { get; set; }
        public bool? Cancel_Weekoff { get; set; }
        public string? Send_Intimate { get; set; }
        public string? LeaveStatus { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? Day { get; set; }
        public string? ApprovedBy { get; set; }
        public TimeSpan? FromHour { get; set; }
        public TimeSpan? ToHour { get; set; }
    }
}
