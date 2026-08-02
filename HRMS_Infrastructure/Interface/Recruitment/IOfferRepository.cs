using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Utility;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    public interface IOfferRepository
    {
        Task<APIResponse> GetAllOffers(int? companyId);
        Task<APIResponse> GetOfferById(int offerId);
        Task<APIResponse> GetOfferByCandidateApplicationId(int candidateApplicationId);
        Task<APIResponse> CreateOffer(Offer model);
        Task<APIResponse> UpdateOffer(Offer model);
        Task<APIResponse> DeleteOffer(DeleteRecordVM model);
        Task<APIResponse> SetOfferStatus(int offerId, string status, int? updatedBy);
        Task<APIResponse> SetOfferLetterUrl(int offerId, string offerLetterUrl, int? updatedBy);
        Task<APIResponse> RecordCandidateResponse(int offerId, string status, string? acceptedByCandidateName, string? declineReason, int? updatedBy);
    }
}
