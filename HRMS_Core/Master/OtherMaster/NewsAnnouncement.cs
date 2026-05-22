using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.OtherMaster
{
    public class NewsAnnouncement
    {
        public int NewsID { get; set; }
        public int CmpID { get; set; }
        public string? NewsTitle { get; set; }
        public string? NewsDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsVisible { get; set; }
        public bool IsThought { get; set; }
        public bool IsPop { get; set; }
        public bool IsLoginNotification { get; set; }
        public string? BranchWiseNewsAnnoun { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
    }
}
