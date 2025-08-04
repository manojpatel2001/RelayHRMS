using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;

namespace HRMS_Infrastructure.Interface.EmployeeMaster
{
    public interface IBankDetailsRepository
    {
        Task<VMCommonResult> CreateBankDetail(BankDetails model);
        Task<VMCommonResult> UpdateBankDetail(BankDetails model);
        Task<VMCommonResult> DeleteBankDetail(DeleteRecordVM deleteRecord);
        Task<BankDetails?> GetBankDetailById(vmCommonGetById vmCommonGetById);
        Task<List<BankDetails>> GetAllBankDetails(vmCommonGetById vmCommonGetById);
    }
}
