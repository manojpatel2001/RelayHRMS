using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.EmployeeMaster
{
    public class EmergencyContact
    {
        public int EmergencyContactId { set; get; }
        public int ? EmployeeId { set; get; }
        public int ? RelationShipId { set; get; }
        public string? Name { set; get; }
        public string? RelationName { set; get; }
        public string ? MobileNo { set; get; }
        public string ? HomePhoneNo { set; get; }
        public string ? WorkPhoneNo { set; get; }
        public bool? IsDeleted { get; set; } = false;
        public bool? IsEnabled { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }
    public class RelationShip
    {
        public int? RelationShipId { set; get; }
        public string? RelationName{ set; get; }
        public bool? IsDeleted { get; set; } = false;
        public bool? IsEnabled { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

    }
}
