using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class VMCompOffDetails
    {
        public int? Comp_Off_Detailsid { get; set; }
        public int? Cmp_Id { get; set; }
        public int? Emp_Id { get; set; }
        public int? Rep_Person_Id { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public DateTime? Extra_Work_Day { get; set; }
        public string? Extra_Work_Hours { get; set; }
        public string? Application_Status { get; set; }
        public string? Comp_Off_Type { get; set; }

    }
}
