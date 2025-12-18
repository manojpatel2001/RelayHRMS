using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ApprovalManagement
{
    public class EscalationMailModel
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }

        // Previous Approver
        public string PreviousApprover { get; set; }
        public string PreviousApproverCode { get; set; }
        public string PreviousApproverEmail { get; set; }
        public int PreviousLevelNo { get; set; }

        // Next Approver
        public string NextApprover { get; set; }
        public string NextApproverCode { get; set; }
        public string NextApproverEmail { get; set; }
        public int NextLevelNo { get; set; }

        public string CompanyName { get; set; }

        public DateTime? EscalationDate { get; set; }
    }

    public class EscalationReturnPara
    {
        public string? EscalatedData { get; set; }
        public string ResponseMessage { get; set; }
        public bool IsSuccess { get; set; }
    }

    

}
