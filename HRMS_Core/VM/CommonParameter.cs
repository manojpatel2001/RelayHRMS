using HRMS_Core.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM
{
    public class CommonParameter
    {
        public int? CompanyId { get; set; }
        public int? DepartmentId { get; set; }
        public int? EmployeeId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
