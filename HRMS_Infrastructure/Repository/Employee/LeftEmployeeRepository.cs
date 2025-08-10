using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.ManagePermision;
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
          @CmpID = {model.CmpID},
          @EmpID = {model.EmpID},
          @BranchId = {model.BranchId},
          @LeftDate = {model.LeftDate},
          @LeftReason = {model.LeftReason},
          @RegAcceptDate = {model.RegAcceptDate},
          @IsTerminate = {model.IsTerminate},
          @UniformReturn = {model.UniformReturn},
          @ExitInterview = {model.ExitInterview},
          @NoticePeriod = {model.NoticePeriod},
          @IsDeath = {model.IsDeath},
          @RegDate = {model.RegDate},
          @IsFnFApplicable = {model.IsFnFApplicable},
          @RptManagerID = {model.RptManagerID},
          @IsRetire = {model.IsRetire},
          @RequestAprID = {model.RequestAprID},
          @LeftReasonValue = {model.LeftReasonValue},
          @LeftReasonText = {model.LeftReasonText},
          @Res_Id = {model.Res_Id},
          @IsDeleted = {model.IsDeleted},
          @IsEnabled = {model.IsEnabled},
          @CreatedBy = {model.CreatedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteLeftEmployee(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC sp_LeftEmployee_CRUD
                    @Operation = {"DELETE"},
                    @LeftID = {deleteRecord.Id},
                    @DeletedBy = {deleteRecord.DeletedBy}
            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }


        }

        public async Task<List<VmLeftEmployee>> GetAllLeftEmployee()
        {
            try
            {
                var result = await _db.Set<VmLeftEmployee>()
                    .FromSqlInterpolated($@"EXEC sp_GetLeftEmployeeList")
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {       
                return new List<VmLeftEmployee>(); 
            }
        }


        public async Task<VMCommonResult> UpdateLeftEmployee(LeftEmployee model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
            EXEC sp_LeftEmployee_CRUD
          @Operation = {"UPDATE"},
          @LeftID = {model.LeftID},
          @CmpID = {model.CmpID},
          @EmpID = {model.EmpID},
          @BranchId = {model.BranchId},
          @LeftDate = {model.LeftDate},
          @LeftReason = {model.LeftReason},
          @RegAcceptDate = {model.RegAcceptDate},
          @IsTerminate = {model.IsTerminate},
          @UniformReturn = {model.UniformReturn},
          @ExitInterview = {model.ExitInterview},
          @NoticePeriod = {model.NoticePeriod},
          @IsDeath = {model.IsDeath},
          @RegDate = {model.RegDate},
          @IsFnFApplicable = {model.IsFnFApplicable},
          @RptManagerID = {model.RptManagerID},
          @IsRetire = {model.IsRetire},
          @RequestAprID = {model.RequestAprID},
          @LeftReasonValue = {model.LeftReasonValue},
          @LeftReasonText = {model.LeftReasonText},
          @Res_Id = {model.Res_Id},
          @IsDeleted = {model.IsDeleted},
          @IsEnabled = {model.IsEnabled},
          @UpdatedBy ={model.UpdatedBy}

            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }


        public async Task<LeftEmployee?> GetLeftEmpById(vmCommonGetById filter)
        {
            try
            {
                var result = await _db.Set<LeftEmployee>().FromSqlInterpolated($@"
                 EXEC sp_LeftEmployee_CRUD
                    @Operation = {"GET"},
                    @Id = {filter.Id}
                   
            ").ToListAsync();

                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
    }
}
