﻿using HRMS_Core.Master.JobMaster;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Salary;
using HRMS_Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Salary
{
    public interface IMonthlySalaryDetailsRepository:IRepository<SalaryDetailViewModel>
    {
        Task<VMCommonResult> CreateSalaryDetails(MonthlySalaryRequestViewModel vm);
        Task<List<SalaryReportDTO>> GetMonthlySalaryData(MonthlySalaryRequestViewModel vm);
        Task<List<SalaryDetailViewModel>> GetSalaryDetails(SalaryDetailsParameterVm vm);
        Task<SalaryDetailForGetById?> GetBySalaryDetailsId(List<int> Ids);
        Task<VMCommonResult> DeleteSalaryDetails(DeleteRecordVModel deleteRecordVM);

    }
}
