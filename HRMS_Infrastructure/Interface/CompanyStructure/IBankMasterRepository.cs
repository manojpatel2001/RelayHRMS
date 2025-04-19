using HRMS_Core.Master.CompanyStructure;
using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.CompanyStructure
{
    public interface IBankMasterRepository:IRepository<BankMaster>
    {
        Task<List<BankMaster>> GetAllBankMaster();
        Task<BankMaster?> GetByBankMasterId(int bankMasterId);
        Task<VMCommonResult> CreateBankMaster(BankMaster bankMaster);
        Task<VMCommonResult> UpdateBankMaster(BankMaster bankMaster);
        Task<VMCommonResult> DeleteBankMaster(DeleteRecordVM deleteRecordVM);
    }
}
