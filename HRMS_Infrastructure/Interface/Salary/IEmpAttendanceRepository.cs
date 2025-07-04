﻿using HRMS_Core.Migrations;
using HRMS_Core.Salary;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Salary
{
    public interface IEmpAttendanceRepository:IRepository<EmpAttendanceImport>
    {
        Task<bool> UpdateEmpAttendance(EmpAttendanceImport empAttendanceImport);
        Task<EmpAttendanceImport> SoftDelete(DeleteRecordVM DeleteRecord);
    }
}
