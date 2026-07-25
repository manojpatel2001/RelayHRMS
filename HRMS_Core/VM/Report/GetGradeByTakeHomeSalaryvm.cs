using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class GetGradeByTakeHomeSalaryvm
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public decimal SalaryRange { get; set; }
        public string Description { get; set; }
    }
}
