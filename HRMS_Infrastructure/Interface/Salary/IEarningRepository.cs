using HRMS_Core.Master.JobMaster;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Salary
{
    public interface IEarningRepository : IRepository<Earning>
    {
        Task<bool> UpdateEarning(Earning earning);
        Task<Earning> SoftDelete(DeleteRecordVM DeleteRecord);
    }
}
