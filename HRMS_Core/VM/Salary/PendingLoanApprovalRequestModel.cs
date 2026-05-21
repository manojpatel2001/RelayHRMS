using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class PendingLoanApprovalRequestModel
    {
        public int ApprovalRequestId { get; set; }
        public string RequestTitle { get; set; }
        public int RequestStatusId { get; set; }
        public string RequestStatus { get; set; }
        public int ApprovalRequestLevelId { get; set; }
        public int LevelNo { get; set; }
        public int LevelStatusId { get; set; }
        public string LevelStatus { get; set; }
        public int ApproverEmployeeId { get; set; }
        public int RequesterEmployeeId { get; set; }
        public DateTime? EscalationDueOn { get; set; }
        public DateTime? AssignedOn { get; set; }
        public string ApproverName { get; set; }
        public string RequesterName { get; set; }
        public int CurrentLevelSequence { get; set; }
    }
}
