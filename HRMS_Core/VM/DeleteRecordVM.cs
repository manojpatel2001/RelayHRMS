using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM
{
    public class DeleteRecordVM
    {
        public int Id { get; set; }
        public string? emp_id { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public bool? IsEnabled { get; set; } = true;
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }
}
