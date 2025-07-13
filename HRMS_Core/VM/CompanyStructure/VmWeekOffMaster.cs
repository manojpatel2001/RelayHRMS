using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.CompanyStructure
{
    public  class VmWeekOffMaster:BaseModel 
    {
        
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public List<VmWeekOff> WeekOff { get; set; }
   
    }
    public  class VmWeekOff
    {
         public int? WeekOffDetailsId { get; set; }   
        public string? WeekOffName { get; set; }    
        public string? WeekOffDay { get; set; }    
    }
}
