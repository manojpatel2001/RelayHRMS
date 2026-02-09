using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class NewsAnnouncementDto
    {
        public int NewsID { get; set; }
        public int CmpID { get; set; }
        public string? NewsTitle { get; set; }
        public string? NewsDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsVisible { get; set; }
        public string? BranchWiseNewsAnnoun { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
