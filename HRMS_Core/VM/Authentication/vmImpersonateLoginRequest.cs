namespace HRMS_Core.VM.Authentication
{
    public class vmImpersonateLoginRequest
    {
        public int TargetEmployeeId { get; set; }
        public string? Reason { get; set; }
    }
}
