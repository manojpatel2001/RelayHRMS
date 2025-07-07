using HRMS_Core.ControlPanel.CompanyInformation;
using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.CompanyInformation;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Infrastructure.Interface.EmployeeMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.EmployeeMaster
{
    public class EmployeeManageRepository:Repository<HRMSUserIdentity>, IEmployeeManageRepository
    {
        private HRMSDbContext _db;

        public EmployeeManageRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        
        public async Task<VMCommonResult> CreateEmployee(vmEmployeeData employee)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageEmployee
                        @Action = {"CREATE"},
                        @FullName = {employee.FullName},
                        @Initial = {employee.Initial},
                        @FirstName = {employee.FirstName},
                        @MiddleName = {employee.MiddleName},
                        @LastName = {employee.LastName},
                        @EmployeeCode = {employee.EmployeeCode},
                        @AlfaEmployeeCode = {employee.AlfaEmployeeCode},
                        @AlfaCode = {employee.AlfaCode},
                        @DateOfJoining = {employee.DateOfJoining},
                        @BranchId = {employee.BranchId},
                        @GradeId = {employee.GradeId},
                        @ShiftMasterId = {employee.ShiftMasterId},
                        @CTC = {employee.CTC},
                        @DesignationId = {employee.DesignationId},
                        @GrossSalary = {employee.GrossSalary},
                        @CategoryId = {employee.CategoryId},
                        @BasicSalary = {employee.BasicSalary},
                        @DepartmentId = {employee.DepartmentId},
                        @EmployeeTypeId = {employee.EmployeeTypeId},
                        @DateOfBirth = {employee.DateOfBirth},
                        @LoginAlias = {employee.LoginAlias},
                        @Password = {employee.Password},
                        @ReportingManagerId = {employee.ReportingManagerId},
                        @SubBranch = {employee.SubBranch},
                        @EnrollNo = {employee.EnrollNo},
                        @CompanyId = {employee.CompanyId},
                        @Overtime = {employee.Overtime},
                        @Latemark = {employee.Latemark},
                        @Earlymark = {employee.Earlymark},
                        @Fullpf = {employee.Fullpf},
                        @Pt = {employee.Pt},
                        @Fixsalary = {employee.Fixsalary},
                        @Probation = {employee.Probation},
                        @Trainee = {employee.Trainee},
                        @EmployeeProfileUrl = {employee.EmployeeProfileUrl},
                        @EmployeeSignatureUrl = {employee.EmployeeSignatureUrl},
                        @IsDeleted = {employee.IsDeleted},
                        @IsEnabled = {employee.IsEnabled},
                        @IsBlocked = {employee.IsBlocked},
                        @CreatedDate = {employee.CreatedDate},
                        @CreatedBy = {employee.CreatedBy}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> UpdateEmployee(vmEmployeeData employee)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageEmployee
                        @Action = {"UPDATE"},
                        @Id = {employee.Id},
                        @FullName = {employee.FullName},
                        @Initial = {employee.Initial},
                        @FirstName = {employee.FirstName},
                        @MiddleName = {employee.MiddleName},
                        @LastName = {employee.LastName},
                        @EmployeeCode = {employee.EmployeeCode},
                         @AlfaEmployeeCode = {employee.AlfaEmployeeCode},
                        @AlfaCode = {employee.AlfaCode},
                        @DateOfJoining = {employee.DateOfJoining},
                        @BranchId = {employee.BranchId},
                        @GradeId = {employee.GradeId},
                        @ShiftMasterId = {employee.ShiftMasterId},
                        @CTC = {employee.CTC},
                        @DesignationId = {employee.DesignationId},
                        @GrossSalary = {employee.GrossSalary},
                        @CategoryId = {employee.CategoryId},
                        @BasicSalary = {employee.BasicSalary},
                        @DepartmentId = {employee.DepartmentId},
                        @EmployeeTypeId = {employee.EmployeeTypeId},
                        @DateOfBirth = {employee.DateOfBirth},
                        @LoginAlias = {employee.LoginAlias},
                        @Password = {employee.Password},
                        @ReportingManagerId = {employee.ReportingManagerId},
                        @SubBranch = {employee.SubBranch},
                        @EnrollNo = {employee.EnrollNo},
                        @CompanyId = {employee.CompanyId},
                        @Overtime = {employee.Overtime},
                        @Latemark = {employee.Latemark},
                        @Earlymark = {employee.Earlymark},
                        @Fullpf = {employee.Fullpf},
                        @Pt = {employee.Pt},
                        @Fixsalary = {employee.Fixsalary},
                        @Probation = {employee.Probation},
                        @Trainee = {employee.Trainee},
                        @EmployeeProfileUrl = {employee.EmployeeProfileUrl},
                        @EmployeeSignatureUrl = {employee.EmployeeSignatureUrl},
                        @UpdatedDate = {employee.UpdatedDate},
                        @UpdatedBy = {employee.UpdatedBy}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<VMCommonResult> DeleteEmployee(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC ManageEmployee
                        @Action = {"DELETE"},
                        @Id = {deleteRecord.emp_id},
                        @DeletedDate = {deleteRecord.DeletedDate},
                        @DeletedBy = {deleteRecord.DeletedBy}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async  Task<List<vmGetAllEmployee>> GetAllEmployee(int companyId)
        {
            try
            {
                return await _db.Set<vmGetAllEmployee>()
                                .FromSqlInterpolated($"EXEC GetAllEmployee @companyId={companyId}")
                                .ToListAsync();
            }
            catch (Exception)
            {
                return new List<vmGetAllEmployee>();
            }
        }

        public async Task<List<vmGetAllEmployee>> GetAllEmployeeByIsBlocked(bool IsBlocked, int companyId)
        {
            try
            {
                return await _db.Set<vmGetAllEmployee>()
                                .FromSqlInterpolated($"EXEC GetAllEmployeeByIsBlocked  @IsBlocked = {IsBlocked},@companyId={companyId}")
                                .ToListAsync();
            }
            catch (Exception)
            {
                return new List<vmGetAllEmployee>();
            }
        }

        public async Task<vmGetAllEmployee?> GetEmployeeById(int Id)
        {
            try
            {
                var result = await _db.Set<vmGetAllEmployee>()
                                      .FromSqlInterpolated($"EXEC GetEmployeeById @Id = {Id}")
                                      .ToListAsync();

                return result.FirstOrDefault() ?? null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async  Task<VMCommonResult> UpdateEmployeeProfileAndSignature(vmUpdateEmployeeProfile model)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC UpdateEmployeeProfileAndSignature
                        @Id = {model.EmployeeId},
                        @EmployeeProfileUrl = {model.EmployeeProfileUrl},
                        @EmployeeSignatureUrl = {model.EmployeeSignatureUrl}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<vmGetNextEmployeeCode?> GetNextEmployeeCode(int companyId)
        {
            try
            {
                var result = await _db.Set<vmGetNextEmployeeCode>()
                    .FromSqlInterpolated($"EXEC GetNextEmployeeCode @CompanyId = {companyId}")
                    .ToListAsync();

                return result.FirstOrDefault()??null;
            }
            catch (Exception)
            {
                return null;
            }
        }


    }
}
