using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class EventViewModel
    {
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public DateTime? Date { get; set; }
        public string? EventType { get; set; }  // e.g., "birthday", "anniversary"
        public string? Repeat { get; set; }     // e.g., "daily", "weekly"
        public string? EventCategory { get; set; }
    }
}
