using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Salary
{
    public interface ISalaryPayableDaysOverrideRepo :IRepository<SalaryPayableDaysOverrideResponseDto>
    {
        Task<SP_Response> CreateSalaryPayableDaysOverride(SalaryPayableDaysOverrideResponseDto dto);
        Task<SP_Response> UpdateSalaryPayableDaysOverride(SalaryPayableDaysOverrideResponseDto dto);
        Task<SP_Response> DeleteSalaryPayableDaysOverride(DeleteRecordVM deleteRecord);
        Task<List<SalaryPayableDaysOverridevm>> GetSalaryPayableDaysOverride(SalaryPayableDaysPara Para);
    }
}
