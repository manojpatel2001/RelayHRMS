using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.importData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Salary
{
    public interface IDeductionRepository :IRepository<Deduction>
    {
        Task<bool> UpdateDeduction(Deduction deduction);
        Task<Deduction> SoftDelete(DeleteRecordVM DeleteRecord);
        Task<List<GetAllDeductionData>> GetDeductionDataAsync(SearchFilterModel searchFilter);

    }
}
