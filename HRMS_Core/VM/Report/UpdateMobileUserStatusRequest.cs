using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class UpdateMobileUserStatusRequest
    {
        public string? Action { get; set; }  
        public string? EmpId { get; set; }     
        public string? UpdatedBy { get; set; }
    }
}
