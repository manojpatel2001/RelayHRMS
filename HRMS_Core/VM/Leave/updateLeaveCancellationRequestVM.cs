using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class updateLeaveCancellationRequestVM
    {
        public int LeaveCancellationId { get; set; }  // Required: ID of the record to update
        public int EmployeeId { get; set; }           // Required: Employee ID
        public bool IsApproved { get; set; }           // Required: Approval flag
        public bool IsRejected { get; set; }           // Required: Rejection flag
        public int UpdatedBy { get; set; }
    }
}
