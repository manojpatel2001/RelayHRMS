using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.importData
{
    public  class GetAllEarningData
    {
            public int? EarningId { get; set; }
            public decimal? Basic { get; set; }
            public decimal? HRA { get; set; }
            public decimal? Conveyance { get; set; }
            public decimal? Medical { get; set; }
            public decimal? Deputation { get; set; }

            public int? Month { get; set; }
            public int? Year { get; set; }

            public string? EmployeeCode { get; set; }
            public string? FullName { get; set; }
     

    }
}
