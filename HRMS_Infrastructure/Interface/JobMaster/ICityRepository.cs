using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.JobMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.JobMaster
{
    public interface ICityRepository : IRepository<City>
    {
        Task<List<vmGetAllCity>> GetAllCity();
        Task<vmGetAllCity?> GetByCityId(int cityId);
        Task<VMCommonResult> CreateCity(City city);
        Task<VMCommonResult> UpdateCity(City city);
        Task<VMCommonResult> DeleteCity(DeleteRecordVM deleteRecordVM);

    }
}
