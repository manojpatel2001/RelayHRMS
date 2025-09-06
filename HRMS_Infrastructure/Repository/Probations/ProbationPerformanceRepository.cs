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
    public class ProbationPerformanceRepository : IProbationPerformanceRepository
    {
        private readonly HRMSDbContext _db;

        public ProbationPerformanceRepository(HRMSDbContext db)
        {
            _db = db;
        }

        public async Task<List<ProbationEmployeeVM>> GetAllProbationEmployees(int ProbationManagerId)
        {
            try
            {
                return await _db.Set<ProbationEmployeeVM>()
                    .FromSqlInterpolated($"EXEC GetAllProbationEmployees @ProbationManagerId = {ProbationManagerId}")
                    .ToListAsync();
            }
            catch
            {
                return new List<ProbationEmployeeVM>();
            }
        }

        public async Task<EmployeeProbationDetailVM?> GetEmployeeForProbationByEmployeeId(int EmployeeId)
        {
            try
            {
                var result = await _db.Set<EmployeeProbationDetailVM>()
                    .FromSqlInterpolated($"EXEC GetEmployeeForProbationByEmployeeId @EmployeeId = {EmployeeId}")
                    .ToListAsync();
                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task<SP_Response> CreateProbationPerformance(ProbationPerformance probationPerformance)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageProbationPerformance
                        @Action = {"CREATE"},
                        @EmployeeId = {probationPerformance.EmployeeId},
                        @ReviewType = {probationPerformance.ReviewType},
                        @EvaluationDate = {probationPerformance.EvaluationDate},
                        @Period = {probationPerformance.Period},
                        @EmployeeType = {probationPerformance.EmployeeType},
                        @MajorStrengths = {probationPerformance.MajorStrengths},
                        @MajorWeaknesses = {probationPerformance.MajorWeaknesses},
                        @RemarksOfAppraiser = {probationPerformance.RemarksOfAppraiser},
                        @RemarksOfAppraisalReviewer = {probationPerformance.RemarksOfAppraisalReviewer},
                        @ScoreRange = {probationPerformance.ScoreRange},
                        @DocumentUrl = {probationPerformance.DocumentUrl},
                        @ExtendedInDays = {probationPerformance.ExtendedInDays},
                        @ProbationExtendedDate = {probationPerformance.ProbationExtendedDate},
                        @CreatedBy = {probationPerformance.CreatedBy}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> UpdateProbationPerformance(ProbationPerformance probationPerformance)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageProbationPerformance
                        @Action = {"UPDATE"},
                        @ProbationPerformanceId = {probationPerformance.ProbationPerformanceId},
                        @EmployeeId = {probationPerformance.EmployeeId},
                        @ReviewType = {probationPerformance.ReviewType},
                        @EvaluationDate = {probationPerformance.EvaluationDate},
                        @Period = {probationPerformance.Period},
                        @EmployeeType = {probationPerformance.EmployeeType},
                        @MajorStrengths = {probationPerformance.MajorStrengths},
                        @MajorWeaknesses = {probationPerformance.MajorWeaknesses},
                        @RemarksOfAppraiser = {probationPerformance.RemarksOfAppraiser},
                        @RemarksOfAppraisalReviewer = {probationPerformance.RemarksOfAppraisalReviewer},
                        @ScoreRange = {probationPerformance.ScoreRange},
                        @DocumentUrl = {probationPerformance.DocumentUrl},
                        @ExtendedInDays = {probationPerformance.ExtendedInDays},
                        @ProbationExtendedDate = {probationPerformance.ProbationExtendedDate},
                        @UpdatedBy = {probationPerformance.UpdatedBy}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<SP_Response> DeleteProbationPerformance(DeleteRecordVM deleteRecord)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                    EXEC ManageProbationPerformance
                        @Action = {"DELETE"},
                        @ProbationPerformanceId = {deleteRecord.Id},
                        @DeletedBy = {deleteRecord.DeletedBy}
                ").ToListAsync();
                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Something went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Something went wrong!" };
            }
        }
    }
}
