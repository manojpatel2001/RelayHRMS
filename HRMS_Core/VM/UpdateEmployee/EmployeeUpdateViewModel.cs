namespace HRMS_Core.VM.UpdateEmployee
{
    public class EmployeeUpdateViewModel
    {
        public List<EmployeeDetailViewModel> Employees { get; set; }
        public List<BranchViewModel> Branches { get; set; }
        public List<GradeViewModel> Grades { get; set; }
        public List<ShiftViewModel> Shifts { get; set; }
        public List<DesignationViewModel> Designations { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public List<DepartmentViewModel> Departments { get; set; }
        public List<EmployeeTypeViewModel> EmployeeTypes { get; set; }
        public List<RoleViewModel> Roles { get; set; }
        public List<ReportingEmployeeViewModel> ReportingEmployees { get; set; }
    }

}
