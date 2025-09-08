using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class SearchVmForCompoffAdmin
    {

        public string? SearchType { get; set; } 
        public string? SearchFor { get; set; }
        public int? CompId { get; set; }
        public int? BranchId { get; set; }
        public string? ApplicationStatus { get; set; }
    }
}
