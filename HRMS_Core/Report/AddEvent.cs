using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Report
{
    [Table("AddEvent")] 
    public class AddEvent:BaseModel

    {
        [Key]
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public DateTime? Date { get; set; }
        public string? EventType { get; set; }
        public string? Repeat { get; set; }
        public bool IsMyevent { get; set; }
        public bool IsShowall { get; set; }
    }
}
