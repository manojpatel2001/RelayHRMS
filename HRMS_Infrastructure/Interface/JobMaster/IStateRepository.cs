using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.JobMaster
{
    public interface IStateRepository : IRepository<State>
    {
        Task<VMCommonResult> CreateState(State model);
        Task<VMCommonResult> UpdateState(State model);
        Task<VMCommonResult> DeleteState(DeleteRecordVM deleteRecord);
        Task<List<State>> GetAllStates(vmCommonGetById filters); 
        Task<State?> GetStateById(vmCommonGetById filter);
    }
}
