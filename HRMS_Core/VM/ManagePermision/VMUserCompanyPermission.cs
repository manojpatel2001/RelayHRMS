using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ManagePermision
{
    public class VMUserCompanyPermission
    {
        public int UserCompanyPermissionId { get; set; }
        public int? EmployeeId { get; set; }
        public int? CompanyId { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsEnabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }

}
