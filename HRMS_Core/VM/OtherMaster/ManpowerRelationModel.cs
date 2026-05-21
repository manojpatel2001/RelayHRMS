using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.OtherMaster
{
    public class ManpowerRelationModel
    {
        public int? ManpowerRelationId { get; set; }
        public string? Name { get; set; }
        public int? ManpowerRequisitionId { get; set; }
        public int? RelationShipId { get; set; }
        public string? MobileNo { get; set; }
        public bool? IsEnabled { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
    }

}
