using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeProfileLanguageRepository
    {
        Task<SP_Response> AddEmpProfileLanguge (VmLanguage model);
        Task<SP_Response> UpdateEmpProfileLanguge(VmLanguage model);
        Task<SP_Response> DeleteEmpProfileLanguge(VmLanguage model);

        Task<List<VmLanguage>> GetAllEmpProfileLanguage();

        //// Get By Id (Optional but recommended)
        //Task<VmLanguage?> GetEmpProfileLanguageById(int languageId);

    }
}
