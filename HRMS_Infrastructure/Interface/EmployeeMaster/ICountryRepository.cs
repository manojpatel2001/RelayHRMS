using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.EmployeeMaster
{
    public interface ICountryRepository:IRepository<Country>
    {
        Task<List<Country>> GetAllCountries();
        Task<Country?> GetCountryById(int countryId);
        Task<VMCommonResult> CreateCountry(Country country);
        Task<VMCommonResult> UpdateCountry(Country country);
        Task<VMCommonResult> DeleteCountry(DeleteRecordVM deleteRecord);
    }
}
