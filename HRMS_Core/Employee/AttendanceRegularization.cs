using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Employee
{
    [Table("AttendanceRegularization")]
    public class AttendanceRegularization:BaseModel
    {
        [Key]
        public int AttendanceRegularizationId { get; set; }
        public int? EmpId { get; set; }
        public string? FullName { get; set; }
        public string? BranchName { get; set; }
        public DateTime? ForDate { get; set; }
        public string? ShiftTime { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? Day { get; set; }
        public string? Status { get; set; }
        public string? Reason { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool IsPending { get; set; } = true;  // Default to pending
        public bool IsRejected { get; set; } = false;
        public bool IsLocked { get; set; } = false;
    }
}
