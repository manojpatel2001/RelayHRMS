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
   
    public class UserPermission
    {
        public int UserPermissionId { get; set; }
        public string? PermissionIds { get; set; }
        public int? EmployeeId { get; set; }
        public int? CompanyId { get; set; }
       
    }
}
