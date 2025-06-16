using HRMS_Core.PrivilegeSetting;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.PrivilegeSetting
{
    public interface IPagePanelRepository:IRepository<PagePanel>
    {
        Task<VMCommonResult> CreatePagePanel(PagePanel pagePanel);
        Task<VMCommonResult> UpdatePagePanel(PagePanel pagePanel);
        Task<VMCommonResult> DeletePagePanel(DeleteRecordVM pagePanel);
        Task<PagePanel?> GetPagePanelById(int pagePanelId);
        Task<List<PagePanel>> GetAllPagePanels();
    }
}
