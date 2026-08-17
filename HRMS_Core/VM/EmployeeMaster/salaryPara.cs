namespace HRMS_Core.VM.EmployeeMaster
{
    public class salaryPara
    {
        public decimal? GrossSalary { get; set; }
        public decimal? BasicSalary { get; set; }
        public int? CompanyId { get; set; }
        public int? EmployeeId { get; set; }
        public bool? IsPFApplicable { get; set; } = true;

        // Optional — the Grade/Designation currently selected on the form.
        // Needed for the live preview because during employee CREATE there is
        // no AspNetUsers row yet to resolve these from by EmployeeId, and
        // during EDIT the user may have changed the dropdown without saving
        // yet. When omitted, the resolver falls back to looking these up from
        // the existing employee row (fine for the persisted Create/Update
        // calls, which always run after the employee row itself is saved).
        public int? GradeId { get; set; }
        public int? DesignationId { get; set; }
    }
}
