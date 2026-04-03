using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class BranchWiseJoiningCountVM
    {
        public string BranchName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public int LastMonth { get; set; }
        public int LastQuarter { get; set; }
        public int LastSixMonths { get; set; }
        public int LastYear { get; set; }
    }
}
