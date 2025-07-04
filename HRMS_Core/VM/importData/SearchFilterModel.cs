using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.importData
{
    public class SearchFilterModel
    {
        public int? Month { get; set; }
        public int? Year { get; set; }
        public string? EmpCode { get; set; }
        public bool IsBetween { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BranchId { get; set; }
    }
}
