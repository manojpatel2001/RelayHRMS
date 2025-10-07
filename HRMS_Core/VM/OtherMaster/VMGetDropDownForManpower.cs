using HRMS_Core.VM.UpdateEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.OtherMaster
{
 
    public class VMGetDropDownForManpower
    {
             public List<DepartmentViewModel> Departments { get; set; }
        public List<DesignationViewModel> Designations { get; set; }  
        public List<ReportingEmployeeViewModel> ReportingEmployees { get; set; }
    }
}
