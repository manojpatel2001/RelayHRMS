using HRMS_Core.ControlPanel.CompanyInformation;
using HRMS_Core.VM.CompanyInformation;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.CompanyInformation
{
    public interface ICompanyDetailsRepository : IRepository<CompanyDetails>
    {
        Task<List<vmGetAllCompanyDetails>> GetAllCompanyDetails();
        Task<List<vmGetAllCompanyDetailsList>> GetAllCompanyDetailsList();
        Task<vmGetAllCompanyDetails?> GetByCompanyId(int companyId);
        Task<List<vmGetAllCompanyDetails>> GetCompanyListByCompanyId(int companyId);
        Task<VMCommonResult> CreateCompanyDetails(CompanyDetails companyDetails);
        Task<VMCommonResult> UpdateCompanyDetails(CompanyDetails companyDetails);
        Task<VMCommonResult> DeleteCompanyDetails(DeleteRecordVM deleteRecordVM);
        Task<VMCommonResult> UpdateCompanyLogo(vmChangeCompanyLogo vmChangeCompanyLogo);
        Task<VMCommonResult> UpdateLetterHead(vmUploadHeader vmUploadHeader);
        Task<VMCommonResult> UpdateDigitalSignature(vmDigitalSignature vmDigitalSignature);
    }

}
