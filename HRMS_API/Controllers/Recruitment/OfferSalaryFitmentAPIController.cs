using HRMS_API.Services;
using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferSalaryFitmentAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SalaryFitmentService _salaryFitmentService;

        public OfferSalaryFitmentAPIController(IUnitOfWork unitOfWork, SalaryFitmentService salaryFitmentService)
        {
            _unitOfWork = unitOfWork;
            _salaryFitmentService = salaryFitmentService;
        }

        [HttpGet("GetFitmentByCandidateApplicationId/{candidateApplicationId}")]
        public async Task<APIResponse> GetFitmentByCandidateApplicationId(int candidateApplicationId)
        {
            try { return await _unitOfWork.OfferSalaryFitmentRepository.GetFitmentByCandidateApplicationId(candidateApplicationId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve salary fitment. Please try again later." }; }
        }

        [HttpGet("GetBreakupByFitmentId/{offerSalaryFitmentId}")]
        public async Task<APIResponse> GetBreakupByFitmentId(int offerSalaryFitmentId)
        {
            try { return await _unitOfWork.OfferSalaryFitmentRepository.GetBreakupByFitmentId(offerSalaryFitmentId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve salary breakup. Please try again later." }; }
        }

        // Pure calculation preview — does not persist. Lets the UI show a live breakup as
        // the recruiter adjusts FinalCTC/template before saving.
        [HttpPost("PreviewBreakup")]
        public async Task<APIResponse> PreviewBreakup([FromBody] PreviewBreakupRequest model)
        {
            try
            {
                if (model == null || model.SalaryStructureTemplateId == 0 || model.FinalCTC <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Template and Final CTC are required." };

                var componentsResponse = await _unitOfWork.SalaryStructureRepository.GetComponentsByTemplateId(model.SalaryStructureTemplateId);
                var components = componentsResponse?.Data as List<SalaryStructureComponent> ?? new List<SalaryStructureComponent>();

                var breakup = _salaryFitmentService.ComputeBreakup(components, model.FinalCTC);
                return new APIResponse { isSuccess = true, ResponseMessage = "Success!", Data = breakup };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to compute breakup. Please try again later.", Data = ex.Message };
            }
        }

        // Computes and persists the fitment + breakup for an offer in one call.
        [HttpPost("SaveFitment")]
        public async Task<APIResponse> SaveFitment([FromBody] SaveFitmentRequest model)
        {
            try
            {
                if (model?.Fitment == null || model.Fitment.CandidateApplicationId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Candidate application is required." };

                if (model.Fitment.SalaryStructureTemplateId.HasValue && model.Fitment.FinalCTC.HasValue)
                {
                    var positionMinMax = model.MinBudget ?? 0;
                    var positionMaxBudget = model.MaxBudget ?? decimal.MaxValue;
                    model.Fitment.BudgetValidationStatus =
                        model.Fitment.FinalCTC.Value >= positionMinMax && model.Fitment.FinalCTC.Value <= positionMaxBudget
                            ? "WithinBudget" : "ExceedsBudget";
                }

                model.Fitment.CreatedDate = DateTime.UtcNow;
                var fitmentResult = model.Fitment.OfferSalaryFitmentId > 0
                    ? await _unitOfWork.OfferSalaryFitmentRepository.UpdateFitment(model.Fitment)
                    : await _unitOfWork.OfferSalaryFitmentRepository.CreateFitment(model.Fitment);

                if (!fitmentResult.isSuccess) return fitmentResult;

                int fitmentId = model.Fitment.OfferSalaryFitmentId > 0 ? model.Fitment.OfferSalaryFitmentId : Convert.ToInt32(fitmentResult.Data);

                if (model.Fitment.SalaryStructureTemplateId.HasValue && model.Fitment.FinalCTC.HasValue)
                {
                    var componentsResponse = await _unitOfWork.SalaryStructureRepository.GetComponentsByTemplateId(model.Fitment.SalaryStructureTemplateId.Value);
                    var components = componentsResponse?.Data as List<SalaryStructureComponent> ?? new List<SalaryStructureComponent>();
                    var breakup = _salaryFitmentService.ComputeBreakup(components, model.Fitment.FinalCTC.Value);
                    await _unitOfWork.OfferSalaryFitmentRepository.ReplaceBreakupComponents(fitmentId, breakup, model.Fitment.CreatedBy);
                }

                return new APIResponse { isSuccess = true, ResponseMessage = "Salary fitment saved successfully!", Data = fitmentId };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to save salary fitment. Please try again later.", Data = ex.Message };
            }
        }
    }

    public class PreviewBreakupRequest
    {
        public int SalaryStructureTemplateId { get; set; }
        public decimal FinalCTC { get; set; }
    }

    public class SaveFitmentRequest
    {
        public OfferSalaryFitment Fitment { get; set; } = new();
        public decimal? MinBudget { get; set; }
        public decimal? MaxBudget { get; set; }
    }
}
