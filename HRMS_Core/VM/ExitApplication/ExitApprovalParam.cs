using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ExitApplication
{
    public class ExitApprovalParam
    {
        public int EmployeeID { get; set; }
        public string ? SearchBy { get; set; }
        public string ? SearchFor { get; set; }
        public string ? Status { get; set; }
    }
}
