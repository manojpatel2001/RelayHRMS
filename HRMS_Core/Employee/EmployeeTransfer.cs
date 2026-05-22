using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Employee
{
    public class EmployeeTransfer
    {
        public int TransferId { get; set; }
        public int EmployeeId { get; set; }
        public int CurrentBranchId { get; set; }
        public int TransferBranchId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string? Reason { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int TransferReportingPerson { get; set; }
        public int CurrentReportingPerson { get; set; }
    }
}
