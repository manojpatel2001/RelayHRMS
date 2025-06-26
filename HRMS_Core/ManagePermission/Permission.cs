using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.ManagePermission
{
    [Table("Permission")]
    public class Permission:BaseModel
    {
        [Key]
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Slug { get; set; }
        public string? Description { get; set; }
        public string? PermissionUrl { get; set; }
        public string? GroupName { get; set; }

    }
}
