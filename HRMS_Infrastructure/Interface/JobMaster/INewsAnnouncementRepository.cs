using HRMS_Core.Employee;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.JobMaster
{
    public interface INewsAnnouncementRepository: IRepository<NewsAnnouncement>
    {
        Task<SP_Response> CreateNewsAnnouncement(NewsAnnouncement model);
        Task<SP_Response> UpdateNewsAnnouncement(NewsAnnouncement model);
        Task<SP_Response> DeleteNewsAnnouncement(DeleteRecordVM deleteRecord);
        Task<List<NewsAnnouncement>> GetNewsAnnouncement(int? CompanyId);
    }
}
