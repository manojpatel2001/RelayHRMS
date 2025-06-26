using HRMS_Core.DbContext;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.ManagePermission
{
    [Table("UserPermission")]
    public class UserPermission:BaseModel
    {
        [Key]
        public int UserPermissionId { get; set; }
        public int? PermissionId { get; set; }
        public int? EmployeeId { get; set; }
        public bool IsAllowed { get; set; } = true;

    }
}
