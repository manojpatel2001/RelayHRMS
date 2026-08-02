using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface IOfferSalaryFitmentRepository
    {
        Task<APIResponse> CreateFitment(OfferSalaryFitment model);
        Task<APIResponse> UpdateFitment(OfferSalaryFitment model);
        Task<APIResponse> DeleteFitment(DeleteRecordVM model);
        Task<APIResponse> GetFitmentByCandidateApplicationId(int candidateApplicationId);
        Task<APIResponse> GetFitmentById(int offerSalaryFitmentId);

        Task<APIResponse> ReplaceBreakupComponents(int offerSalaryFitmentId, List<OfferSalaryBreakupComponent> components, int? createdBy);
        Task<APIResponse> GetBreakupByFitmentId(int offerSalaryFitmentId);
    }
}
