using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ApprovalManagement
{
    public class ApprovalMasterViewModel
    {
        public int? ApprovalMasterId { get; set; }
        public string? ApprovalName { get; set; }
        public int? ApprovalTypeId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public string? Action { get; set; } // "INSERT", "UPDATE", "DELETE"
    }

}
