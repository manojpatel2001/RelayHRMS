using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ExitApplication
{
    public class ExitApplicationUpdateparam
    {
        public int ExitApplicationID { get; set; }
       public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public int UpdatedBy { get; set; }
        public int EmployeeId { get; set; }
    }
}
