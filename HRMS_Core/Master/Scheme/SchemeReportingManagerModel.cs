using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.Scheme
{
    public class SchemeReportingManagerModel
    {
        public int? Id { get; set; }
        public int? SchemeId { get; set; }
        public int? LevelNo { get; set; }
        public int? CompanyId { get; set; }
        public int? ReportingManagerId { get; set; }
        public int? DesignationId { get; set; }
        public bool? IsReportingManager { get; set; }
        public bool? IsNotMandatory { get; set; }
        public bool? IsBranchManager { get; set; }
        public bool? IsReportingToReportingManager { get; set; }

        public bool? IsHR { get; set; }
        public bool? IsHOD { get; set; }
        public bool? IsProbationManager { get; set; }
        public bool? IsEnabled { get; set; }
        public int? EscalationDays { get; set; }
        public int? NextLevelId { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public string? OperationType { get; set; } // "Create", "Update", "Delete"
    }
    public class SchemeDropdownDetails
    {
        public List<CompanyModel>? Companies { get; set; }
        public List<SchemeTypeModel>? SchemeTypes { get; set; }
        public List<DesignationModel>?Designations { get; set; }
    }

    public class CompanyModel
    {
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
    }

    public class SchemeTypeModel
    {
        public int? SchemeTypeId { get; set; }
        public string? SchemeType { get; set; }
    }

    public class DesignationModel
    {
        public int? DesignationId { get; set; }
        public string? DesignationName { get; set; }
    }

    public class SchemeModel
    {
        public int? SchemeId { get; set; }
        public string? SchemeName { get; set; }
    }

    public class EmployeeModel
    {
        public int? EmployeeId { get; set; }
        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
    }

}
