using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.SuperAdmin
{
    [Table("SuperAdminDetails")]
    public class SuperAdminDetails
    {
        [Key]
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? MobileNumber { get; set; }
        public string? Password { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
    }

}
