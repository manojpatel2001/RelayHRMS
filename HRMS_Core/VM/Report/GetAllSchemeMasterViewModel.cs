
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class GetAllSchemeMasterViewModel
    {
        public int SchemeID { get; set; }
        public string? SchemeName { get; set; }
        public string? SchemeType { get; set; }
        public bool? IsDefaultScheme { get; set; }
        public bool? IsDeleted { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
    }
}
