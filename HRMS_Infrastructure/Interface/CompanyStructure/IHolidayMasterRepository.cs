using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyStructure;
using HRMS_Core.VM.ManagePermision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.CompanyStructure
{
    public interface IHolidayMasterRepository:IRepository<HolidayMaster>
    {
        Task<VMCommonResult> CreateHoliday(HolidayMaster model);
        Task<VMCommonResult> UpdateHoliday(HolidayMaster model);
        Task<VMCommonResult> DeleteHoliday(DeleteRecordVM deleteRecord);
        Task<List<vmGetAllHolidayMaster>> GetAllHolidayMaster(vmCommonGetById filters);
        Task<vmGetAllHolidayMaster?> GetHolidayMasterById(vmCommonGetById filter);
    }
}
