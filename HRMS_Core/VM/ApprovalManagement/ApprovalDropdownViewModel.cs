using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ApprovalManagement
{
    public class ApprovalDropdownViewModel
    {
        public List<CompanyViewModel>? Companies { get; set; }
        public List<ApprovalTypeViewModel>? ApprovalTypes { get; set; }
        public List<DesignationViewModel>? Designations { get; set; }
        public List<DepartmentViewModel>? Departments { get; set; }
    }
    public class CompanyViewModel
    {
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
    }

    public class DesignationViewModel
    {
        public int? DesignationId { get; set; }
        public string? DesignationName { get; set; }
    }


    public class DepartmentViewModel
    {
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
    }
    public class ApprovalTypeViewModel
    {
        public int? ApprovalTypeId { get; set; }
        public string? ApprovalTypeName { get; set; }
    }

}
