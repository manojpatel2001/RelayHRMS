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
    public interface IWeekOffMasterRepository:IRepository<WeekOffDetails>
    {
        Task<VMCommonResult> CreateWeekOffDetails(WeekOffDetails weekOffDetails);
        Task<VMCommonResult> UpdateWeekOffDetails(WeekOffDetails weekOffDetails);
        Task<VMCommonResult> DeleteWeekOffDetails(DeleteRecordVM deleteRecordVM);
        Task<List<vmGetAllWeekOffDetails>> GetAllWeekOffDetails(vmCommonParameters vmCommonParameters);
    }
}
