using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class EmployeeSalaryPublish
    {

            public int? EmployeeId { get; set; }
            public int? Id { get; set; }
            public string? EmployeeCode { get; set; }
            public string? FullName { get; set; }
            public string? DesignationName { get; set; }
            public int Month { get; set; }
            public int Year { get; set; }
            public string? Status { get; set; }
        }
    }

