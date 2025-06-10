using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.EmployeeMaster
{
    public interface IThanaRepository:IRepository<Thana>
    {
        Task<List<Thana>> GetAllThanas();
        Task<Thana?> GetThanaById(int thanaId);
        Task<VMCommonResult> CreateThana(Thana thana);
        Task<VMCommonResult> UpdateThana(Thana thana);
        Task<VMCommonResult> DeleteThana(DeleteRecordVM deleteRecord);

    }
}
