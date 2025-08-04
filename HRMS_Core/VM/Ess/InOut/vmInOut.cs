using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Ess.InOut
{
    public class vmInOut
    {
        public int? CompanyId {  get; set; }
        public int? EmployeeId {  get; set; }
        public string? Mode {  get; set; }
        public DateTime? PunchDateTime {  get; set; }
        public string? CreatedBy {  get; set; }
    }
}
