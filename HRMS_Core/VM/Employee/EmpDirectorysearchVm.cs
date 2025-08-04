using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class EmpDirectorysearchVm
    {

        public string? BranchId { get; set; }         
        public string? DepartmentId { get; set; }
        public string? DesignationId { get; set; }
        public string? EmpCodeName { get; set; }
        public int? isenable { get; set; }
        public int? isdeleted { get; set; }


    }
}
