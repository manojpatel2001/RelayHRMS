using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.DbContext
{
    public class HRMSRoleIdentity : IdentityRole<int>
    {
        public string? Description {  get; set; }
        public string? Slug {  get; set; }
        public bool IsDeleted {  get; set; }=false;
        public bool IsEnabled { get; set; } = true;
        public HRMSRoleIdentity() : base() { }

        public HRMSRoleIdentity(string roleName) : base(roleName) { }
    }
}
