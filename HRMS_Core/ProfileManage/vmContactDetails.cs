namespace HRMS_Core.ProfileManage
{
    public class vmContactDetails
    {
        public int? EmployeeId { get; set; }
        public string? PresentAddress { get; set; }
        public string? PresentTehsil { get; set; }
        public string? PresentDistrict { get; set; }
        public string? PresentCity { get; set; }
        public int? PresentStateId { get; set; }
        public string? PresentPincode { get; set; }
        public int? PresentThanaId { get; set; }
        public string? PermanentAddress { get; set; }
        public string? PermanentTehsil { get; set; }
        public string? PermanentDistrict { get; set; }
        public string? PermanentCity { get; set; }
        public int? PermanentStateId { get; set; }
        public string? PermanentPincode { get; set; }
        public int? PermanentThanaId { get; set; }
        public int? CountryId { get; set; }
        public string? WorkPhone { get; set; }
        public string?   PersonalPhone { get; set; }
        public string? OfficialEmail { get; set; }
        public string? Nationality { get; set; }
        public string? ExtensionNo { get; set; }
        public string? MobileNo { get; set; }
        public bool? SameAsPresentAddress { get; set; }
    }
}
