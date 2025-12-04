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
    public interface IProbationPerformanceRepository
    {
        Task<List<ProbationEmployeeVM>> GetAllProbationEmployees(int ProbationManagerId);
        Task<EmployeeProbationDetailVM?> GetEmployeeForProbationByEmployeeId(int EmployeeId);
        Task<SP_Response> CreateProbationPerformance(ProbationPerformance probationPerformance);
        Task<SP_Response> UpdateProbationPerformance(ProbationPerformance probationPerformance);
        Task<SP_Response> DeleteProbationPerformance(DeleteRecordVM deleteRecord);



    }
}
