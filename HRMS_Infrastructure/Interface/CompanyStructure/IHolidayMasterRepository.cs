using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.CompanyStructure
{
    public interface IHolidayMasterRepository:IRepository<HolidayMaster>
    {
        Task<List<vmGetAllHolidayMaster>> GetAllHolidayMaster();
        Task<VMCommonResult> CreateHolidayMaster(vmCreateHoliayMaster holidayMaster);
        Task<VMCommonResult> UpdateHolidayMaster(vmCreateHoliayMaster holidayMaster);
        Task<VMCommonResult> DeleteHolidayMaster(DeleteRecordVM deleteRecordVM);
    }
}
