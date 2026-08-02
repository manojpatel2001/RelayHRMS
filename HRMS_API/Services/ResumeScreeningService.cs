using HRMS_Core.Recruitment;
using HRMS_Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS_API.Services
{
    // Rule-based resume screening: keyword/skill overlap, experience-range match, and
    // education-level match against the linked Job Position, weighted into an overall
    // score + Strong/Moderate/Weak recommendation. No external AI/LLM dependency —
    // per the confirmed Phase 1 scope decision.
    public class ResumeScreeningService
    {
        private const string EngineVersion = "RuleBased-v1";

        private static readonly Dictionary<string, int> EducationLevelRank = new(StringComparer.OrdinalIgnoreCase)
        {
            { "10th", 1 }, { "SSC", 1 },
            { "12th", 2 }, { "HSC", 2 }, { "Diploma", 2 },
            { "Graduate", 3 }, { "Bachelor", 3 }, { "UG", 3 },
            { "Post Graduate", 4 }, { "Master", 4 }, { "PG", 4 },
            { "Doctorate", 5 }, { "PhD", 5 }
        };

        private readonly IUnitOfWork _unitOfWork;

        public ResumeScreeningService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ScreenCandidateApplicationAsync(int candidateApplicationId, int? actionBy = null)
        {
            var applicationResponse = await _unitOfWork.CandidateApplicationRepository.GetCandidateApplicationById(candidateApplicationId);
            if (applicationResponse?.Data is not CandidateApplication application) return;

            var positionResponse = await _unitOfWork.JobPositionRepository.GetJobPositionById(application.JobPositionId);
            if (positionResponse?.Data is not JobPositionMaster position) return;

            var candidateResponse = await _unitOfWork.CandidateRepository.GetCandidateById(application.CandidateId);
            var candidate = candidateResponse?.Data as Candidate;

            var skillsResponse = await _unitOfWork.CandidateProfileRepository.GetSkillByCandidateId(application.CandidateId);
            var candidateSkills = (skillsResponse?.Data as List<CandidateSkill>) ?? new List<CandidateSkill>();

            var educationResponse = await _unitOfWork.CandidateProfileRepository.GetEducationByCandidateId(application.CandidateId);
            var candidateEducation = (educationResponse?.Data as List<CandidateEducation>) ?? new List<CandidateEducation>();

            var (skillScore, matched, missing) = ScoreSkills(position.RequiredSkills, position.PreferredSkills, candidateSkills);
            var experienceScore = ScoreExperience(position.MinExperienceYears, position.MaxExperienceYears, candidate?.TotalExperienceYears);
            var educationScore = ScoreEducation(position.MinEducationLevel, candidateEducation);

            // Weighted: skills matter most for a role fit, then experience, then education.
            var overallScore = Math.Round((skillScore * 0.5m) + (experienceScore * 0.3m) + (educationScore * 0.2m), 2);
            var recommendation = overallScore >= 75 ? "Strong Match" : overallScore >= 50 ? "Moderate Match" : "Weak Match";

            bool isDuplicate = false;
            int? duplicateOfCandidateId = null;
            string? duplicateMatchReason = null;
            if (candidate != null)
            {
                var dupResponse = await _unitOfWork.CandidateRepository.CheckDuplicateCandidate(candidate.Email, candidate.Phone, candidate.ResumeFileHash);
                if (dupResponse?.Data is IEnumerable<dynamic> dupRows)
                {
                    var other = dupRows.Cast<dynamic>().FirstOrDefault(r => (int)r.CandidateId != candidate.CandidateId);
                    if (other != null)
                    {
                        isDuplicate = true;
                        duplicateOfCandidateId = (int)other.CandidateId;
                        duplicateMatchReason = (string)other.DuplicateMatchReason;
                    }
                }
            }

            var result = new CandidateResumeScreeningResult
            {
                CandidateApplicationId = candidateApplicationId,
                JobPositionId = application.JobPositionId,
                SkillMatchScore = skillScore,
                ExperienceMatchScore = experienceScore,
                EducationMatchScore = educationScore,
                OverallScore = overallScore,
                Recommendation = recommendation,
                MatchedSkills = matched,
                MissingSkills = missing,
                IsDuplicate = isDuplicate,
                DuplicateOfCandidateId = duplicateOfCandidateId,
                DuplicateMatchReason = duplicateMatchReason,
                ScreeningEngineVersion = EngineVersion,
                CreatedBy = actionBy,
                UpdatedBy = actionBy
            };

            await _unitOfWork.CandidateResumeScreeningResultRepository.CreateOrUpdateScreeningResult(result);
        }

        private static (decimal score, string matched, string missing) ScoreSkills(string? requiredSkillsCsv, string? preferredSkillsCsv, List<CandidateSkill> candidateSkills)
        {
            var required = SplitSkills(requiredSkillsCsv);
            var preferred = SplitSkills(preferredSkillsCsv);
            var candidateSkillNames = candidateSkills
                .Select(s => s.SkillName?.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s!.ToLowerInvariant())
                .ToHashSet();

            if (required.Count == 0 && preferred.Count == 0)
                return (0m, string.Empty, string.Empty);

            var matchedRequired = required.Where(r => candidateSkillNames.Contains(r)).ToList();
            var matchedPreferred = preferred.Where(p => candidateSkillNames.Contains(p)).ToList();
            var missingRequired = required.Except(matchedRequired).ToList();

            // Required skills weigh 80% of the skill score, preferred skills the remaining 20%.
            decimal requiredScore = required.Count > 0 ? (decimal)matchedRequired.Count / required.Count * 80m : 80m;
            decimal preferredScore = preferred.Count > 0 ? (decimal)matchedPreferred.Count / preferred.Count * 20m : 20m;
            var score = Math.Round(requiredScore + preferredScore, 2);

            var matchedAll = matchedRequired.Concat(matchedPreferred).Distinct();
            return (score, string.Join(", ", matchedAll), string.Join(", ", missingRequired));
        }

        private static HashSet<string> SplitSkills(string? csv) =>
            (csv ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(s => s.ToLowerInvariant())
                .ToHashSet();

        private static decimal ScoreExperience(decimal? minYears, decimal? maxYears, decimal? candidateYears)
        {
            if (minYears == null && maxYears == null) return 100m;
            if (candidateYears == null) return 0m;

            var min = minYears ?? 0m;
            var max = maxYears ?? (min + 5m);

            if (candidateYears >= min && candidateYears <= max) return 100m;

            // Linear taper for being outside the range (under-qualified or over-qualified).
            var distance = candidateYears < min ? min - candidateYears.Value : candidateYears.Value - max;
            var penalty = Math.Min(distance * 20m, 100m);
            return Math.Max(0m, 100m - penalty);
        }

        private static decimal ScoreEducation(string? minEducationLevel, List<CandidateEducation> candidateEducation)
        {
            if (string.IsNullOrWhiteSpace(minEducationLevel)) return 100m;
            if (!EducationLevelRank.TryGetValue(minEducationLevel.Trim(), out var requiredRank)) return 100m;

            var candidateMaxRank = candidateEducation
                .Select(e => EducationLevelRank.TryGetValue((e.EducationLevel ?? string.Empty).Trim(), out var rank) ? rank : 0)
                .DefaultIfEmpty(0)
                .Max();

            return candidateMaxRank >= requiredRank ? 100m : 50m;
        }
    }
}
