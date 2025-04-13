using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.OtherMaster
{
    public class vmGetAllTicketTypes
    {
        public int? TicketTypeId { get; set; }
        public string? TicketTypeName { get; set; }

        public int? DepartmentId { get; set; } 
        public string? DepartmentName { get; set; }

        public string? CreatedBy { get; set; } 
        public DateTime? CreatedDate { get; set; }

        public bool? IsEnabled { get; set; }
    }
}
