using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class GetCountDirectOrIndirectEmployeesVM
    {

        public int? DirectMember { get; set; }
        public int? IndirectMember { get; set; }
    }
}
