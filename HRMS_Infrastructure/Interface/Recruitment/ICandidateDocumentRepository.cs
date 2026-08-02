using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Utility;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Recruitment
{
    // Covers both CandidateDocumentType (admin-configurable master) and CandidateDocument
    // (the per-application uploaded files) — grouped since they're always used together.
    public interface ICandidateDocumentRepository
    {
        Task<APIResponse> GetAllDocumentTypes();
        Task<APIResponse> CreateDocumentType(CandidateDocumentType model);
        Task<APIResponse> UpdateDocumentType(CandidateDocumentType model);
        Task<APIResponse> DeleteDocumentType(DeleteRecordVM model);

        Task<APIResponse> GetDocumentsByApplicationId(int candidateApplicationId);
        Task<APIResponse> CreateDocument(CandidateDocument model);
        Task<APIResponse> UpdateDocument(CandidateDocument model);
        Task<APIResponse> DeleteDocument(DeleteRecordVM model);
    }
}
