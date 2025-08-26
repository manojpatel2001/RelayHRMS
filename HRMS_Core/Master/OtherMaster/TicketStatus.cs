using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.OtherMaster
{
    public class TicketStatus
    {
        public int TicketStatusId {  get; set; }
        public string? TicketStatusName {  get; set; }
        public bool? IsEnabled { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;
    }
}
