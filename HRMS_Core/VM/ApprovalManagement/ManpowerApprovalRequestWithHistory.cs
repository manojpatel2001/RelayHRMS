using HRMS_Core.VM.Probations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ApprovalManagement
{
    public class ManpowerApprovalRequestWithHistory
    {
        public int ApprovalRequestId { get; set; }
        public string RequestTitle { get; set; }
        public int RequestStatusId { get; set; }
        public string RequestStatus { get; set; }
        public int CurrentApprovalRequestLevelId { get; set; }
        public int CurrentLevelNo { get; set; }
        public int CurrentLevelStatusId { get; set; }
        public string CurrentLevelStatus { get; set; }
        public int RequesterEmployeeId { get; set; }
        public string RequesterName { get; set; }
        public int ApproverEmployeeId { get; set; }
        public string ApproverName { get; set; }
        public DateTime? AssignedOn { get; set; }
        public DateTime? EscalationDueOn { get; set; }

        // Manpower Requisition specific fields
        public int? ManpowerRequisitionId { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string RequirementType { get; set; }
        public string EmployeeName { get; set; }
        public string PersonalEmail { get; set; }
        public string ContactNumber { get; set; }
        public int? DesignationId { get; set; }
        public string DesignationName { get; set; }
        public string ExperienceRange { get; set; }
        public string EducationalQualification { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string CategoryOfEmployment { get; set; }
        public decimal? CTC_Monthly { get; set; }
        public decimal? GrossSalary { get; set; }
        public decimal? TakeHomeSalary { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public string SerialNo { get; set; }
        // JSON string containing previous approval levels
        public string PreviousApprovalLevelsJson { get; set; }

        // Parsed previous approval levels
        public List<ApprovalLevelHistory> PreviousApprovalLevels { get; set; }
    }

    public class ManpowerApprovalRequestFilter
    {
        public int? ApproverEmployeeId { get; set; }
        public int StatusId { get; set; } = 7; // Default to Pending
        public int? ApprovalMasterId { get; set; }
    }
}
