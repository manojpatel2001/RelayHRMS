using HRMS_Core.Master.JobMaster;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.CompanyStructure
{
    public class vmCreateHoliayMaster
    {
        public int HolidayMasterId { get; set; }
        public string? HolidayName { get; set; }
        public string? State { get; set; }
        public int BranchId { get; set; }
        public List<BrancheAndLimit> ?Branches{ get; set; }
        
        public bool MultipleHoliday { get; set; } = false;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string? MessageText { get; set; }
        public string? Holidaycategory { get; set; }

        public bool RepeatAnnually { get; set; } = false;
        public bool HalfDay { get; set; } = false;
        public bool PresentCompulsory { get; set; } = false;
        public bool SMS { get; set; } = false;
        public bool OptionalHoliday { get; set; } = false;
        public string? ApprovalMaxLimit { get; set; } = "0";
        public bool? IsDeleted { get; set; } = false;
        public bool? IsEnabled { get; set; } = true;
        public bool? IsBlocked { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }

    public class BrancheAndLimit{
        public int BranchId { get; set; }
        public string ApprovalMaxLimit { get; set; }

    }
}
