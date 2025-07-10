using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface.Employee;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HRMS_Infrastructure.Repository.Employee
{
    public class EmployeeInOutRepository : Repository<EmployeeInOutRecord>, IEmployeeInOut
    {
        private readonly HRMSDbContext _db;

        public EmployeeInOutRepository(HRMSDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<AttendanceInOutReportVM>> AttendancefirstInOutReport(int empid, string Month, string Year)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@empid", empid),
                    new SqlParameter("@month", Month),
                    new SqlParameter("@year", Year)
                };

                var result = await _db.Set<AttendanceInOutReportVM>()
                    .FromSqlRaw("EXEC AttendanceReport_First_In_Out_Sp @empid, @month, @year", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<AttendanceInOutReportVM>();
            }
        }

        public async Task<List<AttendanceInOutReportVM>> AttendanceMultipleInOutReport(int empid, string Month, string Year)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@empid", empid),
                    new SqlParameter("@month", Month),
                    new SqlParameter("@year", Year)
                };

                var result = await _db.Set<AttendanceInOutReportVM>()
                    .FromSqlRaw("EXEC AttendanceReport_Multiple_In_Out_Sp @empid, @month, @year", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<AttendanceInOutReportVM>();
            }
        }

        public async Task<VMCommonResult> CreateEmpInOut(EmployeeInOutRecord Record)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                    EXEC InsertEmployeeInOutRecord_sp
                        @Emp_Id = {Record.Emp_Id},
                        @Com_Id = {Record.Com_Id},
                        @For_Date = {Record.For_Date},
                        @In_Time = {Record.In_Time},
                        @Out_Time = {Record.Out_Time},
                        @Duration = {Record.Duration},
                        @Reason = {Record.Reason},
                        @Ip_adrress = {Record.Ip_adrress},
                        @In_Date_Time = {Record.In_Date_Time},
                        @Out_Date_Time = {Record.Out_Date_Time},
                        @Skip_Count = {Record.Skip_Count},
                        @Late_Calc_Not_App = {Record.Late_Calc_Not_App},
                        @Chk_By_Superior = {Record.Chk_By_Superior},
                        @Sup_Comment = {Record.Sup_Comment},
                        @Half_Full_day = {Record.Half_Full_day},
                        @Is_Cancel_Late_In = {Record.Is_Cancel_Late_In},
                        @Is_Cancel_Early_Out = {Record.Is_Cancel_Early_Out},
                        @Is_Default_In = {Record.Is_Default_In},
                        @Is_Default_Out = {Record.Is_Default_Out},
                        @Cmp_prp_in_flag = {Record.Cmp_prp_in_flag},
                        @Cmp_prp_out_flag = {Record.Cmp_prp_out_flag},
                        @is_Cmp_purpose = {Record.is_Cmp_purpose},
                        @App_Date = {Record.App_Date},
                        @Apr_Date = {Record.Apr_Date},
                        @System_date = {Record.System_date},
                        @Other_Reason = {Record.Other_Reason},
                        @ManualEntryFlag = {Record.ManualEntryFlag},
                        @statusFlag = {Record.statusFlag},
                        @In_Admin_Time = {Record.In_Admin_Time},
                        @Out_Admin_Time = {Record.Out_Admin_Time},
                        @IsDeleted = {Record.IsDeleted},
                        @IsEnabled = {Record.IsEnabled},
                        @IsBlocked = {Record.IsBlocked},
                        @CreatedDate = {Record.CreatedDate},
                        @CreatedBy = {Record.CreatedBy},
                        @UpdatedDate = {Record.UpdatedDate},
                        @UpdatedBy = {Record.UpdatedBy},
                        @DeletedDate = {Record.DeletedDate},
                        @DeletedBy = {Record.DeletedBy}
                ").ToListAsync();

                return result.FirstOrDefault() ?? new VMCommonResult { Id = null };
            }
            catch (Exception)
            {
                return new VMCommonResult { Id = null };
            }
        }

        public async Task<List<VMInOutRecord>> GetInOutRecord(int empid, string month, string year)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@empid", empid),
                    new SqlParameter("@month", month),
                    new SqlParameter("@year", year)
                };

                var result = await _db.Set<VMInOutRecord>()
                    .FromSqlRaw("EXEC First_In_Out_Sp @empid, @month, @year", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<VMInOutRecord>();
            }
        }

        public async Task<List<VMInOutRecord>> GetMultipleInOutRecordAsync(int empid, string Month, string Year)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@empid", empid),
                    new SqlParameter("@month", Month),
                    new SqlParameter("@year", Year)
                };

                var result = await _db.Set<VMInOutRecord>()
                    .FromSqlRaw("EXEC Multiple_In_Out_Sp @empid, @month, @year", parameters)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return new List<VMInOutRecord>();
            }
        }

        public async Task<bool> UpdateEmployeeOutTimeAsync(int empId, DateTime forDate, DateTime outTime, string updatedBy)
        {
            try
            {
                string sql = @"
                    EXEC SPUpdateOutTimeEmployee 
                        @Emp_Id = {0}, 
                        @For_Date = {1}, 
                        @Out_Time = {2}, 
                        @UpdatedBy = {3}";

                int rowsAffected = await _db.Database.ExecuteSqlRawAsync(sql, empId, forDate, outTime, updatedBy);
                return rowsAffected > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
