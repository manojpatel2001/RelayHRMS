using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class LeftEmployeeRepository : Repository<LeftEmployee>, ILeftEmployeeRepository
    {

        private readonly HRMSDbContext _db;
        public LeftEmployeeRepository(HRMSDbContext hRMSDbContext) : base(hRMSDbContext)
        {
            _db = hRMSDbContext;
        }

        public async Task<VMCommonResult> CreateLeftEmployee(LeftEmployee model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
            EXEC sp_LeftEmployee_CRUD
          @Operation = {"CREATE"},
          @LeftID = {model.LeftID},
          @CmpID = {model.CmpID},
          @EmpID = {model.EmpID},
          @BranchId = {model.BranchId},
          @LeftDate = {model.LeftDate},
          @LeftReason = {model.LeftReason},
          @RegAcceptDate = {model.RegAcceptDate},
          @IsTerminate = {model.IsTerminate},
          @UniformReturn = {model.UniformReturn}
          @ExitInterview = {model.ExitInterview}
          @NoticePeriod = {model.NoticePeriod}
          @IsDeath = {model.IsDeath}
          @RegDate = {model.RegDate}
          @IsFnFApplicable = {model.IsFnFApplicable}
          @RptManagerID = {model.RptManagerID}
          @IsRetire = {model.IsRetire}
          @RequestAprID = {model.RequestAprID}
          @LeftReasonValue = {model.LeftReasonValue}
          @LeftReasonText = {model.LeftReasonText}
          @Res_Id = {model.Res_Id}
          @IsDeleted = {model.IsDeleted}
          @IsEnabled = {model.IsEnabled}
          @IsEnabled = {model.IsEnabled}
          @CreatedBy = {model.CreatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public Task<VMCommonResult> DeleteLeftEmployee(DeleteRecordVM deleteRecord)
        {
            throw new NotImplementedException();
        }

        public Task<VMCommonResult> UpdateLeftEmployee(LeftEmployee model)
        {
            throw new NotImplementedException();
        }
    }
}
