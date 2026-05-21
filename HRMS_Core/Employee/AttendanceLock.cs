using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Employee
{
    public class AttendanceLock
    {
        public int AttendanceLockId { get; set; }
        public int EmpId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsLocked { get; set; } 
        public bool IsEnabled { get; set; } 
        public bool IsDeleted { get; set; } 
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime CreatedDate { get; set; } 
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
