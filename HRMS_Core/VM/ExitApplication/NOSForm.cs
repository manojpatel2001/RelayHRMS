using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ExitApplication
{
    public class NOSForm
    {
        public int NOSID { get; set; }
        public int ExitApplicationID { get; set; }
        public string? ItemName { get; set; }
        public bool IsHandedOver { get; set; }
        public string? HandoverTo { get; set; }
        public string? Remarks { get; set; }
        public byte[]? DocumentProof { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    }
}
