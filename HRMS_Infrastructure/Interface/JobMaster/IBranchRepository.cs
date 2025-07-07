using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.importData;
using HRMS_Core.VM.JobMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.JobMaster
{
    public interface IBranchRepository : IRepository<Branch>
    {
        Task<bool> UpdateBranch(Branch branch);
        Task<Branch> SoftDelete(DeleteRecordVM DeleteRecord);

        Task<List<BranchUserStatsModel>> GetBranchWiseEmpCount();
    }
}
