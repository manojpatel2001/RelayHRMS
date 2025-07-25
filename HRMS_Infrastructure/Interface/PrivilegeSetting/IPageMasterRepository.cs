﻿using HRMS_Core.PrivilegeSetting;
using HRMS_Core.VM;
using HRMS_Core.VM.PrivilegeSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.PrivilegeSetting
{
    public interface IPageMasterRepository:IRepository<PageMaster>
    {
        Task<VMCommonResult> CreatePageMaster(PageMaster pageMaster);

        Task<VMCommonResult> UpdatePageMaster(PageMaster pageMaster);

        Task<VMCommonResult> DeletePageMaster(DeleteRecordVM pageMaster);

        Task<PageMaster?> GetPageMasterById(int pageMasterId);

        Task<List<vmPageMaster>> GetAllPageMaster();
        Task<List<vmPageMaster>> GetAllPagesByPagePanel(int pagePanelId);
        Task<List<PanelHierarchyVM>> GetPageHierarchyWithPrivileges(PageVM pageVM);
        Task<List<PageMaster>> GetAllMenuPages();
    }
}
