using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.OtherMaster
{
    public class GeoLocation
    {
        public int GeoLocationId {  get; set; }
        public int? BranchId {  get; set; }
        public string? GeoLocationName {  get; set; }
        public decimal? Latitude {  get; set; }
        public decimal? Longitude {  get; set; }
        public int? Meter {  get; set; }
        public int? CompanyId { get; set; }
        public bool? IsEnabled { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
