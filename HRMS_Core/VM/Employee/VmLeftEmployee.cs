using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class VmLeftEmployee
    {

        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? BranchName { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public int? LeftID { get; set; }
        public int? CmpID { get; set; }
        public int? EmpID { get; set; }
        public int? BranchId { get; set; }
        public DateTime? LeftDate { get; set; }
        public string? LeftReason { get; set; }
        public DateTime? RegAcceptDate { get; set; }
        public bool IsTerminate { get; set; }
        public bool UniformReturn { get; set; }
        public bool ExitInterview { get; set; }
        public string? NoticePeriod { get; set; }
        public bool IsDeath { get; set; }
        public DateTime? RegDate { get; set; }
        public bool IsFnFApplicable { get; set; }
        public int? RptManagerID { get; set; }
        public bool IsRetire { get; set; }
        public int? RequestAprID { get; set; }
        public string? LeftReasonValue { get; set; }
        public string? LeftReasonText { get; set; }
        public int? Res_Id { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public string? ReasonType { get; set; }

    }
}
