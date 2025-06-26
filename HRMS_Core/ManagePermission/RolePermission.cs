using HRMS_Core.ControlPanel.CompanyInformation;
using HRMS_Core.DbContext;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.ManagePermission
{
    [Table("RolePermission")]
    public class RolePermission:BaseModel
    {
        [Key]
        public int RolePermissionId { get; set; }
        public int? RoleId { get; set; }
        
        public int? PermissionId { get; set; }
        
        public int? CompanyId { get; set; }
    
    }
}
