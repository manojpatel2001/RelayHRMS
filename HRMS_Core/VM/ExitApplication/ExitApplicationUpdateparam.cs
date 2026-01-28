using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ExitApplication
{
    public class ExitApplicationUpdateparam
    {
        public List<int> ExitApplicationID { get; set; }
        public string Status { get; set; }
        public int UpdatedBy { get; set; }
        public int EmployeeId { get; set; }
    }
}
