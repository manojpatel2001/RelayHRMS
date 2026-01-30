using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ExitApplication
{
    public class ExitClearanceApprovalVM
    {
        public int ExitApplicationID { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeCode { get; set; }

        public string FullName { get; set; }

        public DateTime? ResignationDate { get; set; }

        public string ReasonForResignation { get; set; }

        public DateTime? LastWorkingDate { get; set; }

        public string ApprovedByReportingPerson { get; set; }

        public bool IsApproved { get; set; }
        public bool IsNOCFormFilled { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
