using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.PasswordHistory
{
    public class VmPasswordHistory
    {
        public int PasswordHistoryId { get; set; }
        public int EMPID { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
