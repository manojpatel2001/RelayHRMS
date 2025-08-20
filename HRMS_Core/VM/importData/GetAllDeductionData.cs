using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.importData
{
    public class GetAllDeductionData
    {
        public int? DeductionId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public decimal? PF { get; set; }
        public decimal? ESIC { get; set; }
        public decimal? PT { get; set; }
        public decimal? LWF { get; set; }
        public decimal? TDS { get; set; }
        public string? BranchName { get; set; }


    }
}
