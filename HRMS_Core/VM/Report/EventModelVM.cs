using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class EventModelVM
    {

        public int EventId { get; set; }
        public string? EventName { get; set; }
        public DateTime Date { get; set; }
        public string? EventType { get; set; } 
        public string? Repeat { get; set; }   
        public bool IsMyEvent { get; set; }  
        public bool IsEnabled { get; set; }
        public bool IsShowAll { get; set; }
    }
}
