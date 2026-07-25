using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ExitApplication
{
    public class NOCFormDataResponse
    {
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string Designation { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime ResignationDate { get; set; }
        public DateTime LastWorkingDate { get; set; }
        public string ReasonForResignation { get; set; }
        public int NoticePeriodDays { get; set; }
        public int ShortFallDays { get; set; }
        public List<NOCItem> NocItems { get; set; }
    }


    public class NOCItem
    {
        public string ItemName { get; set; }
        public bool IsHandedOver { get; set; }
        public string HandoverTo { get; set; }
        public string Remarks { get; set; }
    }
}
