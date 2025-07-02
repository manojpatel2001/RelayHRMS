using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ManagePermision
{
    public class UserDetailsDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Designation { get; set; }
        public string? ProfileUrl { get; set; }
        public string? Email { get; set; }

        // ⚠️ Be careful exposing Password or PasswordHash in DTOs.
        public string? Password { get; set; }
        public string? PasswordHash { get; set; }
        public string? RoleSlug { get; set; }

        public string? RoleName { get; set; }

        // Permissions can be a list of strings or a complex object
        public List<string>? Permissions { get; set; }
        public string? Company {  get; set; }
    }

}
