using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.EmployeeMaster
{
    public class vmEmployeeSalary
    {
        public int? Id { get; set; }
        public int? EmployeeId { get; set; }
        public int? CompanyId { get; set; }
        public decimal? GrossSalary { get; set; }
    }
}
