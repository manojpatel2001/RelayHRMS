using HRMS_Core.DbContext;
using HRMS_Core.Probations;
using HRMS_Core.VM;
using HRMS_Core.VM.Probations;
using HRMS_Infrastructure.Interface.Probations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Probations
{
    public class ProbationEvaluationFormRepository : IProbationEvaluationFormRepository
    {

        private readonly HRMSDbContext _db;
        private readonly string _connectionString;


        public ProbationEvaluationFormRepository(HRMSDbContext db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;

        }

        public async Task<SP_Response> CreateProbationEvaluationForm(ProbationEvaluationForm model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
            EXEC sp_ProbationEvaluationForm_CRUD
                @Operation      = {"CREATE"},
                @EmployeeId     = {model.EmployeeId},
                @ProbationEvaluationFormUrl = {model.ProbationEvaluationFormUrl},
                @IsUploaded     = {model.IsUploaded},
                @IsEnabled      = {model.IsEnabled},
                @IsDeleted      = {model.IsDeleted},
                @CreatedBy      = {model.CreatedBy}
        ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> UpdateProbationEvaluationForm(ProbationEvaluationForm model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
            EXEC sp_ProbationEvaluationForm_CRUD
                @Operation                  = {"UPDATE"},
                @ProbationEvaluationFormId  = {model.ProbationEvaluationFormId},
                @ProbationEvaluationFormUrl = {model.ProbationEvaluationFormUrl},
                @IsUploaded                 = {model.IsUploaded},
                @IsEnabled                  = {model.IsEnabled},
                @IsDeleted                  = {model.IsDeleted},
                @UpdatedBy                  = {model.UpdatedBy}
        ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> DeleteProbationEvaluationForm(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
            EXEC sp_ProbationEvaluationForm_CRUD
                @Operation                  = {"DELETE"},
                @ProbationEvaluationFormId  = {deleteRecord.Id},
                @DeletedBy                  = {deleteRecord.DeletedBy}
        ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        //    public async Task<List<ProbationEvaluationForm>> GetProbationEvaluationForm(int? probationEvaluationFormId = null, int? employeeId = null)
        //    {
        //        try
        //        {
        //            var result = await _db.Set<ProbationEvaluationForm>().FromSqlInterpolated($@"
        //        EXEC sp_ProbationEvaluationForm_CRUD
        //            @Operation                  = {"GET"},
        //            @ProbationEvaluationFormId  = {probationEvaluationFormId},
        //            @EmployeeId                 = {employeeId}
        //    ").ToListAsync();

        //            return result;
        //        }
        //        catch
        //        {
        //            return new List<ProbationEvaluationForm>();
        //        }
        //    }

        public async Task<List<ProbationEvaluationFormListVM>> GetProbationEvaluationFormList(int loggedInUserId)
        {
            try
            {
                var result = await _db.Set<ProbationEvaluationFormListVM>()
                    .FromSqlInterpolated($@"
                    EXEC USP_GetProbationEvaluationFormList
                        @LoggedInUserId = {loggedInUserId}
                ").ToListAsync();

                return result;
            }
            catch
            {
                return new List<ProbationEvaluationFormListVM>();
            }
        }
    }
}
