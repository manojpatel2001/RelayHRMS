using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class LoanApplicationStatusUpdateModel
    {
  
        public int LoanApplicationId { get; set; }

        public int ApprovalRequestLevelId { get; set; }

        public int ApprovalRequestId { get; set; }

        public int ApproverEmployeeId { get; set; }

        public int StatusId { get; set; }
        public string? Status { get; set; } // e.g., "Approved", "Rejected", "Pending"
        public int LevelNo { get; set; }
        public string? Remarks { get; set; }

        public int UpdatedBy { get; set; }

    }

}
