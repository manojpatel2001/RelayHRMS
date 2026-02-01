using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.JobMaster
{
    public class BranchUserStatsModel
    {
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }

        public int? TotalUsers { get; set; }

        public int? JoinLastMonth { get; set; }

        public int? LeftLastMonth { get; set; }
    }
}
