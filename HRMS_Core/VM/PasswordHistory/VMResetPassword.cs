namespace HRMS_Core.VM.PasswordHistory
{
    public class VMResetPassword
    {
        public int? UserId { get; set; }
        public int? EmployeeId { get; set; }
        public string? NewPassword { get; set; }
    }
}
