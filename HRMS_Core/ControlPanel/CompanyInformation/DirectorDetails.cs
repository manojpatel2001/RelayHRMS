using HRMS_Core.DbContext;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace HRMS_Core.ControlPanel.CompanyInformation
{
    [Table("DirectorDetails")]
    public class DirectorDetails : BaseModel
    {
        [Key]
        public int DirectorDetailsId { get; set; }
        public string? DirectorName { get; set; }
        public string? DirectorAddress { get; set; }
        public DateTime? DirectorDOB { get; set; }
        public string? DirectorBranch { get; set; }
        public string? DirectorDesignation { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [ValidateNever]
        public CompanyDetails? CompanyDetails { get; set; }

    }
}

