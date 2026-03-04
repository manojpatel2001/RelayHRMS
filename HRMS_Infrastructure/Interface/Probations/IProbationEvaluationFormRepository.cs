using HRMS_Core.Probations;
using HRMS_Core.VM;
using HRMS_Core.VM.Probations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Probations
{
    public interface IProbationEvaluationFormRepository
    {
        Task<SP_Response> CreateProbationEvaluationForm(ProbationEvaluationForm model);
        Task<SP_Response> UpdateProbationEvaluationForm(ProbationEvaluationForm model);
        Task<SP_Response> DeleteProbationEvaluationForm(DeleteRecordVM deleteRecord);
        Task<List<ProbationEvaluationFormListVM>> GetProbationEvaluationFormList(int loggedInUserId);

    }
}
