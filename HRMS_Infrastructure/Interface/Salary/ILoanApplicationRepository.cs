using HRMS_Core.Loan;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.importData;
using HRMS_Core.VM.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Salary
{
    public interface ILoanApplicationRepository:IRepository<LoanApplicationViewModel>
    {
        Task<SP_Response> CreateLoanApplication(LoanApplicationViewModel model);
        Task<SP_Response> UpdateLoanApplication(LoanApplicationViewModel model);
        Task<SP_Response> DeleteLoanApplication(DeleteRecordVM deleteRecord);
        Task<List<LoanMaster>> GetLoanNamesForDropdown();
        Task<List<GetLoanApplicationViewModel>> GetLoanApplication(int CompanyId);
        Task<LoanApplicationViewModel> GetLoanDetailsById(int LoanId);
    }
}
