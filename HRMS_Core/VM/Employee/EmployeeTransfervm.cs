using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class GetEmployeeTransfervm
    {
        public int TransferId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public int TransferBranchId { get; set; }
        public int CurrentBranchId { get; set; }
        public int EmployeeId { get; set; }
        public string? CurrentBranchName { get; set; }
        public string? TransferBranchName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string? Reason { get; set; }
        public string? DesignationName { get; set; }
        public string? DepartmentName { get; set; }
        public string? ReportingManagerName { get; set; }
    }
}
