using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.OtherMaster
{
    [Table("SchemeMaster")]
    public class SchemeMaster
    {
        [Key]
        public int SchemeID { get; set; }
        public string?  SchemeName { get; set; }
        public string? Type { get; set; }
        public bool IsDefaultScheme { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public bool? IsEnabled { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }
}
