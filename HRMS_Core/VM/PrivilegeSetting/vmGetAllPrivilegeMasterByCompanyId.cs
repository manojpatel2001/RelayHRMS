using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.PrivilegeSetting
{
    public class vmGetAllPrivilegeMasterByCompanyId
    {
        public int? PrivilegeMasterId { get; set; }
        public string? PrivilegeName { get; set; }
        public int? CompanyId { get; set; }
        public string? BranchId_Multi { get; set; }
        public string? DepartmentId_Multi { get; set; }
        public string? VerticalId_Multi { get; set; }
        public string? PrivilegeType { get; set; }
        public string? BranchNames { get; set; }
        public string? DepartmentNames { get; set; }
        public string? PagePanelName { get; set; }
    }
}
