using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.DbContext
{
    [Table("HRMSUserRoles")]
    public class HRMSUserRole 
    {
        [Key]
        public int UserRoleId { get; set; }
        public int? RoleId { get; set; }
        public int? EmployeeId { get; set; }
        public int? CompanyId {  get; set; }
        public bool IsEnabled {  get; set; }=true;
        public bool IsDeleted {  get; set; }=false;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
