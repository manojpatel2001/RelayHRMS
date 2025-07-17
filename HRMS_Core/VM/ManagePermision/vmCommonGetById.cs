using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ManagePermision
{
    public class vmCommonGetById
    {
        public int? Id { get; set; } = 0;
        public string? Title { get; set; } = "";
        public string? Ids { get; set; } = null;
        public bool? IsDeleted { get; set; } = false;
        public bool? IsEnabled { get; set; } = true;
        public bool? IsBlocked { get; set; } = false;
    }
}
