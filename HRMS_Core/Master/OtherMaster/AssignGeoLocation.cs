namespace HRMS_Core.Master.OtherMaster
{
    public class AssignGeoLocation
    {
        public int AssignGeoLocationId { get; set; }
        public string? GeoLocationIds { get; set; }
        public string? EmployeeIds { get; set; }
       
        public bool? IsEnabled { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
       
    }
}
