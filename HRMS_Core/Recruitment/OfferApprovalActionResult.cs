namespace HRMS_Core.Recruitment
{
    // Public (not anonymous) so OfferApprovalService can read it across the
    // HRMS_Infrastructure -> HRMS_API assembly boundary. An anonymous type here would be
    // internal by default, and the C# dynamic binder honors accessibility across assemblies
    // — accessing it via `dynamic` in another assembly throws at runtime.
    public class OfferApprovalActionResult
    {
        public bool IsAllLevelsCompleted { get; set; }
        public int? OfferApprovalRequestId { get; set; }
    }
}
