using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class YearlySalarySummaryVM
    {
        public string? SalaryHead { get; set; }
        public string? Jan { get; set; } = "0.00";
        public string? Feb { get; set; } = "0.00";
        public string? Mar { get; set; } = "0.00";
        public string? Apr { get; set; } = "0.00";
        public string? May { get; set; } = "0.00";
        public string? Jun { get; set; } = "0.00";
        public string? Jul { get; set; } = "0.00";
        public string? Aug { get; set; } = "0.00";
        public string? Sep { get; set; } = "0.00";
        public string? Oct { get; set; } = "0.00";
        public string? Nov { get; set; } = "0.00";
        public string? Dec { get; set; } = "0.00";
        public string? Total { get; set; } = "0.00";
    }
}
