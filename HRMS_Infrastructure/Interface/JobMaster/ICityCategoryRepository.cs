using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.JobMaster
{
    public interface ICityCategoryRepository : IRepository<CityCategory>
    {
        Task<List<CityCategory>> GetAllCityCategory();
        Task<CityCategory?> GetByCityCategoryId(int categoryId);
        Task<VMCommonResult> CreateCityCategory(CityCategory category);
        Task<VMCommonResult> UpdateCityCategory(CityCategory category);
        Task<VMCommonResult> DeleteCityCategory(DeleteRecordVM deleteRecordVM);
    }

}
