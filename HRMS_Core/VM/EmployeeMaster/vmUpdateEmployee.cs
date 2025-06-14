using HRMS_Core.EmployeeMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.EmployeeMaster
{
    public class vmUpdateEmployee
    {
        public vmEmployeeData? vmEmployeeData {  get; set; }
        public EmployeePersonalInfo? EmployeePersonalInfo {  get; set; }
        public EmployeeContact? EmployeeContact {  get; set; }
    }
}
