using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.importData;
using HRMS_Core.VM.JobMaster;
using HRMS_Core.VM.ManagePermision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.JobMaster
{
    public interface IBranchRepository : IRepository<Branch>
    {
        Task<VMCommonResult> CreateBranch(Branch model);
        Task<VMCommonResult> UpdateBranch(Branch model);
        Task<VMCommonResult> DeleteBranch(DeleteRecordVM deleteRecord);
        Task<List<vmGetAllBranches>> GetAllBranches(vmCommonGetById filters);
        Task<Branch?> GetBranchById(vmCommonGetById filter);
        Task<List<vmGetAllCityByStateId>> GetAllCityByStateId(vmCommonGetById filter);
        Task<vmCheckExistBranchCode?> CheckExistBranchCode(vmCommonGetById filter);

        Task<List<BranchUserStatsModel>> GetBranchWiseEmpCount();
        Task<List<vmGetAllBranchesListByCompanyId>> GetAllBranchesListByCompanyId(vmCommonGetById filter);
    }
}
