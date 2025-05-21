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

        

        public async Task<VMEmpResult> CreateEmployee(vmEmployeeData employee)
        {
            try
            {
                var result = await _db.Set<VMEmpResult>().FromSqlInterpolated($@"
                    EXEC ManageEmployee
                        @Action = {"CREATE"},
                        @FullName = {employee.FullName},
                        @Initial = {employee.Initial},
                        @FirstName = {employee.FirstName},
                        @MiddleName = {employee.MiddleName},
                        @LastName = {employee.LastName},
                        @EmployeeCode = {employee.EmployeeCode},
                        @DateOfJoining = {employee.DateOfJoining},
                        @BranchId = {employee.BranchId},
                        @GradeId = {employee.GradeId},
                        @Shift = {employee.Shift},
                        @CTC = {employee.CTC},
                        @DesignationId = {employee.DesignationId},
                        @GrossSalary = {employee.GrossSalary},
                        @Category = {employee.Category},
                        @BasicSalary = {employee.BasicSalary},
                        @DepartmentId = {employee.DepartmentId},
                        @EmployeeType = {employee.EmployeeType},
                        @DateOfBirth = {employee.DateOfBirth},
                        @UserPrivilege = {employee.UserPrivilege},
                        @LoginAlias = {employee.LoginAlias},
                        @Password = {employee.Password},
                        @ReportingManager = {employee.ReportingManager},
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

                return result.FirstOrDefault() ?? new VMEmpResult { Emp_Id = null };
            }
            catch (Exception)
            {
                return new VMEmpResult { Emp_Id = null };
            }
        }

        public async Task<VMEmpResult> UpdateEmployee(vmEmployeeData employee)
        {
            try
            {
                var result = await _db.Set<VMEmpResult>().FromSqlInterpolated($@"
                    EXEC ManageEmployee
                        @Action = {"UPDATE"},
                        @Id = {employee.Id},
                        @FullName = {employee.FullName},
                        @Initial = {employee.Initial},
                        @FirstName = {employee.FirstName},
                        @MiddleName = {employee.MiddleName},
                        @LastName = {employee.LastName},
                        @EmployeeCode = {employee.EmployeeCode},
                        @DateOfJoining = {employee.DateOfJoining},
                        @BranchId = {employee.BranchId},
                        @GradeId = {employee.GradeId},
                        @Shift = {employee.Shift},
                        @CTC = {employee.CTC},
                        @DesignationId = {employee.DesignationId},
                        @GrossSalary = {employee.GrossSalary},
                        @Category = {employee.Category},
                        @BasicSalary = {employee.BasicSalary},
                        @DepartmentId = {employee.DepartmentId},
                        @EmployeeType = {employee.EmployeeType},
                        @DateOfBirth = {employee.DateOfBirth},
                        @UserPrivilege = {employee.UserPrivilege},
                        @LoginAlias = {employee.LoginAlias},
                        @Password = {employee.Password},
                        @ReportingManager = {employee.ReportingManager},
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

                return result.FirstOrDefault() ?? new VMEmpResult { Emp_Id = null };
            }
            catch (Exception)
            {
                return new VMEmpResult { Emp_Id=null};
            }
        }

        public async Task<VMEmpResult> DeleteEmployee(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<VMEmpResult>().FromSqlInterpolated($@"
                    EXEC ManageEmployee
                        @Action = {"DELETE"},
                        @Id = {deleteRecord.emp_id},
                        @DeletedDate = {deleteRecord.DeletedDate},
                        @DeletedBy = {deleteRecord.DeletedBy}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new VMEmpResult { Emp_Id = null };
            }
            catch (Exception)
            {
                return new VMEmpResult { Emp_Id=null };
            }
        }

        public async  Task<List<vmGetAllEmployee>> GetAllEmployee()
        {
            try
            {
                return await _db.Set<vmGetAllEmployee>()
                                .FromSqlInterpolated($"EXEC GetAllEmployee")
                                .ToListAsync();
            }
            catch (Exception)
            {
                return new List<vmGetAllEmployee>();
            }
        }

        public async Task<HRMSUserIdentity?> GetEmployeeById(string Id)
        {
            try
            {
                var result = await _db.Set<HRMSUserIdentity>()
                                      .FromSqlInterpolated($"EXEC GetEmployeeById @Id = {Id}")
                                      .ToListAsync();

                return result.FirstOrDefault() ?? null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
