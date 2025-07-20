using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.EmployeeMaster
{
    public interface IContractDetailsRepository
    {

        Task<VMCommonResult> CreateContractDetail(ContractDetails model);
        Task<VMCommonResult> UpdateContractDetail(ContractDetails model);
        Task<VMCommonResult> DeleteContractDetail(DeleteRecordVM deleteRecord);
        Task<ContractDetails?> GetContractDetailById(vmCommonGetById vmCommonGetById);
        Task<List<ContractDetails>> GetAllContractDetails(vmCommonGetById vmCommonGetById);
    }
}
