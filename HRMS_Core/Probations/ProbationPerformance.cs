using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Probations
{
    public class ProbationPerformance
    {
        public int ProbationPerformanceId {  get; set; }
        public int? EmployeeId {  get; set; }
        public string? ReviewType {  get; set; }
        public DateTime? EvaluationDate {  get; set; }
        public string? Period {  get; set; }
        public int? EmployeeType {  get; set; }
        public string? MajorStrengths {  get; set; }
        public string? MajorWeaknesses {  get; set; }
        public string? RemarksOfAppraiser {  get; set; }
        public string? RemarksOfAppraisalReviewer {  get; set; }
        public string? ScoreRange  {  get; set; }
        public string? DocumentUrl  {  get; set; }
        public IFormFile? DocumentFile  {  get; set; }
        public int? ExtendedInDays {  get; set; }
        public DateTime? ProbationExtendedDate {  get; set; }

        //base
        public bool? IsDeleted { get; set; } = false;
        public bool? IsEnabled { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }
}
