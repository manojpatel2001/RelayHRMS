using HRMS_Core.Recruitment;
using HRMS_Core.Services;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_API.Services
{
    // Converts an accepted Candidate into a real Employee Master record. Never modifies
    // the untracked usp_ManageEmployee proc — it only calls the existing, unmodified
    // EmployeeManageRepository/EmployeeSalaryAllowanceRepository methods and works around
    // CreateEmployee's missing-new-Id gap with a follow-up lookup by EmployeeCode.
    public class EmployeeConversionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService _emailService;

        public EmployeeConversionService(IUnitOfWork unitOfWork, EmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<(bool Success, string Message, string? EmployeeCode)> ConvertCandidateToEmployeeAsync(int candidateApplicationId, int actionBy)
        {
            var checklistResponse = await _unitOfWork.CandidateOnboardingChecklistRepository.GetByCandidateApplicationId(candidateApplicationId);
            if (checklistResponse?.Data is not CandidateOnboardingChecklist checklist ||
                !(checklist.OfferAcceptedConfirmed && checklist.DocumentsComplete && checklist.BGVComplete &&
                  checklist.SalaryApproved && checklist.JoiningApproved && checklist.EmployeeCreationApproved))
            {
                return (false, "The onboarding checklist must be fully completed before converting to an employee.", null);
            }

            var applicationResponse = await _unitOfWork.CandidateApplicationRepository.GetCandidateApplicationById(candidateApplicationId);
            if (applicationResponse?.Data is not CandidateApplication application)
                return (false, "Candidate application not found.", null);

            var candidateResponse = await _unitOfWork.CandidateRepository.GetCandidateById(application.CandidateId);
            if (candidateResponse?.Data is not Candidate candidate)
                return (false, "Candidate not found.", null);

            var offerResponse = await _unitOfWork.OfferRepository.GetOfferByCandidateApplicationId(candidateApplicationId);
            if (offerResponse?.Data is not Offer offer || !string.Equals(offer.OfferStatus, "Accepted", StringComparison.OrdinalIgnoreCase))
                return (false, "Offer must be accepted before converting to an employee.", null);

            var positionResponse = await _unitOfWork.JobPositionRepository.GetJobPositionById(offer.JobPositionId);
            if (positionResponse?.Data is not JobPositionMaster position)
                return (false, "Job position not found.", null);

            var (grossSalary, basicSalary, isPFApplicable) = await ResolveSalaryAsync(offer);

            var codeInfo = await _unitOfWork.EmployeeManageRepository.GetNextEmployeeCode(position.CompanyId ?? 0);
            if (codeInfo == null)
                return (false, "Unable to generate an employee code for this company.", null);

            // New hires get the standard self-service role — resolved by slug rather than a
            // hardcoded id, since role ids aren't guaranteed stable across environments.
            var allRoles = await _unitOfWork.RoleRepository.GetAllRoles();
            var essRole = allRoles?.FirstOrDefault(r => string.Equals(r.Slug, "ess", StringComparison.OrdinalIgnoreCase) && r.IsEnabled && !r.IsDeleted);
            if (essRole == null)
                return (false, "Unable to resolve the default employee role (ESS) for the new hire.", null);

            int digits = int.TryParse(codeInfo.DigitsForEmployeeCode, out var parsedDigits) ? parsedDigits : 4;
            string employeeCode = codeInfo.NextEmployeeCode.ToString().PadLeft(digits, '0');
            string alfaCode = codeInfo.CompanyCode ?? string.Empty;
            string alfaEmployeeCode = alfaCode + employeeCode;
            string domainName = codeInfo.DomainName ?? string.Empty;
            string loginAliasBase = !string.IsNullOrWhiteSpace(candidate.Email) ? candidate.Email.Split('@')[0] : (candidate.FirstName ?? "user").ToLowerInvariant();
            string loginAlias = loginAliasBase + domainName;
            string temporaryPassword = GenerateTemporaryPassword();

            var employeeVm = new vmUpdateEmployee
            {
                FirstName = candidate.FirstName,
                MiddleName = candidate.MiddleName,
                LastName = candidate.LastName,
                FullName = candidate.FullName,
                EmployeeCode = employeeCode,
                AlfaCode = alfaCode,
                AlfaEmployeeCode = alfaEmployeeCode,
                DateOfJoining = offer.JoiningDate,
                BranchId = offer.BranchId ?? position.BranchId,
                GradeId = offer.GradeId ?? position.GradeId,
                CTC = grossSalary.ToString("0.##"),
                DesignationId = offer.DesignationId ?? position.DesignationId,
                GrossSalary = grossSalary,
                BasicSalary = basicSalary,
                DepartmentId = offer.DepartmentId ?? position.DepartmentId,
                DateOfBirth = candidate.DateOfBirth,
                LoginAlias = loginAlias,
                Password = temporaryPassword,
                ReportingManagerId = position.ReportingManagerId,
                CompanyId = position.CompanyId,
                RoleId = essRole.Id,
                IsPFApplicable = isPFApplicable,
                IsGMP = false,
                Gender = candidate.Gender,
                PersonalEmailId = candidate.Email,
                MobileNo = candidate.Phone,
                PresentAddress = candidate.CurrentAddress,
                PresentCity = candidate.CurrentCity,
                IsEnabled = true,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = actionBy.ToString()
            };

            var createResult = await _unitOfWork.EmployeeManageRepository.CreateEmployee(employeeVm);
            if (!createResult.isSuccess)
            {
                await RecordConversionAsync(candidateApplicationId, null, employeeCode, alfaEmployeeCode, "Failed", createResult.ResponseMessage, actionBy);
                return (false, createResult.ResponseMessage ?? "Failed to create employee.", null);
            }

            // CreateEmployee doesn't return the new Id — look it up by the code we just composed.
            var allForUpdate = await _unitOfWork.EmployeeManageRepository.GetAllEmployeeForUpdate(position.CompanyId ?? 0);
            var createdEmployee = allForUpdate?.FirstOrDefault(e => e.AlfaEmployeeCode == alfaEmployeeCode || e.EmployeeCode == employeeCode);
            int? newEmployeeId = createdEmployee?.Id;

            if (newEmployeeId.HasValue)
            {
                await _unitOfWork.EmployeeSalaryAllowanceRepository.CreateEmployeeSalaryAllowance(new vmEmployeeSalary
                {
                    EmployeeId = newEmployeeId.Value,
                    CompanyId = position.CompanyId,
                    GrossSalary = grossSalary,
                    BasicSalary = basicSalary,
                    IsPFApplicable = isPFApplicable
                });
            }

            await RecordConversionAsync(candidateApplicationId, newEmployeeId, employeeCode, alfaEmployeeCode, "Completed", null, actionBy);
            await AdvancePipelineToJoinedAsync(candidateApplicationId, position.CompanyId, actionBy);
            await SendWelcomeEmailAsync(candidate, alfaEmployeeCode, loginAlias, temporaryPassword);

            return (true, "Candidate converted to employee successfully!", alfaEmployeeCode);
        }

        private async Task<(decimal GrossSalary, decimal BasicSalary, bool IsPFApplicable)> ResolveSalaryAsync(Offer offer)
        {
            decimal grossSalary = offer.OfferedCTC ?? 0;
            decimal basicSalary = 0;
            bool isPFApplicable = false;

            if (offer.OfferSalaryFitmentId.HasValue)
            {
                var breakupResponse = await _unitOfWork.OfferSalaryFitmentRepository.GetBreakupByFitmentId(offer.OfferSalaryFitmentId.Value);
                if (breakupResponse?.Data is List<OfferSalaryBreakupComponent> breakup)
                {
                    var basicComponent = breakup.FirstOrDefault(c => string.Equals(c.ComponentName, "Basic", StringComparison.OrdinalIgnoreCase));
                    if (basicComponent != null) basicSalary = basicComponent.Amount;
                    isPFApplicable = breakup.Any(c => c.ComponentName != null && c.ComponentName.Contains("PF", StringComparison.OrdinalIgnoreCase));
                }
            }

            return (grossSalary, basicSalary, isPFApplicable);
        }

        private async Task RecordConversionAsync(int candidateApplicationId, int? employeeId, string employeeCode, string alfaEmployeeCode, string status, string? failureReason, int actionBy)
        {
            var existingResponse = await _unitOfWork.CandidateEmployeeConversionRepository.GetByCandidateApplicationId(candidateApplicationId);
            var existing = existingResponse?.Data as CandidateEmployeeConversion;

            var model = new CandidateEmployeeConversion
            {
                CandidateEmployeeConversionId = existing?.CandidateEmployeeConversionId ?? 0,
                CandidateApplicationId = candidateApplicationId,
                EmployeeId = employeeId,
                EmployeeCode = employeeCode,
                AlfaEmployeeCode = alfaEmployeeCode,
                ConversionStatus = status,
                FailureReason = failureReason,
                CreatedBy = actionBy,
                UpdatedBy = actionBy
            };

            if (existing != null)
                await _unitOfWork.CandidateEmployeeConversionRepository.UpdateConversion(model);
            else
                await _unitOfWork.CandidateEmployeeConversionRepository.CreateConversion(model);
        }

        private async Task AdvancePipelineToJoinedAsync(int candidateApplicationId, int? companyId, int actionBy)
        {
            var stagesResponse = await _unitOfWork.CandidatePipelineRepository.GetAllPipelineStages(companyId);
            if (stagesResponse?.Data is List<CandidatePipelineStage> stages)
            {
                var joinedStage = stages.FirstOrDefault(s => string.Equals(s.StageName, "Joined", StringComparison.OrdinalIgnoreCase));
                if (joinedStage != null)
                    await _unitOfWork.CandidatePipelineRepository.MoveCandidateToStage(candidateApplicationId, joinedStage.CandidatePipelineStageId, "Employee record created.", actionBy);
            }
        }

        private async Task SendWelcomeEmailAsync(Candidate candidate, string alfaEmployeeCode, string loginAlias, string temporaryPassword)
        {
            if (string.IsNullOrWhiteSpace(candidate.Email)) return;

            var emailRequest = new EmailRequest
            {
                ToEmails = new List<string> { candidate.Email },
                Subject = "Welcome to Relay Express",
                TemplateName = "WelcomeEmployeeEmailTemplate.html",
                Placeholders = new Dictionary<string, string>
                {
                    { "EmployeeName", candidate.FullName ?? candidate.FirstName ?? "Employee" },
                    { "EmployeeCode", alfaEmployeeCode },
                    { "LoginAlias", loginAlias },
                    { "TemporaryPassword", temporaryPassword },
                    { "CompanyName", "Relay Express" }
                }
            };

            await _emailService.SendEmailAsync(emailRequest);
        }

        private static string GenerateTemporaryPassword()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789!@#";
            var bytes = RandomNumberGenerator.GetBytes(12);
            var builder = new StringBuilder();
            foreach (var b in bytes) builder.Append(chars[b % chars.Length]);
            return builder.ToString();
        }
    }
}
