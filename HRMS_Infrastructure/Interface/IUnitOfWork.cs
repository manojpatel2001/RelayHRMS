﻿using HRMS_Infrastructure.Interface.CompanyInformation;
using HRMS_Infrastructure.Interface.CompanyStructure;
using HRMS_Infrastructure.Interface.EmegencyContacts;
using HRMS_Infrastructure.Interface.Employee;
using HRMS_Infrastructure.Interface.EmployeeMaster;
using HRMS_Infrastructure.Interface.JobMaster;
using HRMS_Infrastructure.Interface.Leave;
using HRMS_Infrastructure.Interface.ManagePermissions;
using HRMS_Infrastructure.Interface.Notifications;
using HRMS_Infrastructure.Interface.OtherMaster;
using HRMS_Infrastructure.Interface.PrivilegeSetting;
using HRMS_Infrastructure.Interface.Salary;
using HRMS_Infrastructure.Interface.SuperAdmin;
using HRMS_Infrastructure.Repository.ManagePermissions;
using HRMS_Infrastructure.Repository.OtherMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface
{
    public interface IUnitOfWork 
    {
        IBranchRepository BranchRepository { get; } 
        ICityRepository CityRepository { get; }     
        IDepartmentRepository DepartmentRepository { get; }         
        IDesignationRepository DesignationRepository { get; }   
        IGradeRepository GradeRepository { get; }   
        IReasonRepository ReasonRepository { get; } 
        IStateRepository StateRepository { get; }
        IShiftBreakRepository ShiftBreakRepository { get; }
        IShiftMasterRepository ShiftMasterRepository { get; }
        IOrganizationPolicyRepository OrganizationPolicyRepository { get; }
        ITicketTypeRepository TicketTypeRepository { get; }
        ITicketPriorityRepository TicketPriorityRepository { get; }
        IBankMasterRepository BankMasterRepository { get; }
        IWeekOffMasterRepository WeekOffMasterRepository { get; }
        IHolidayMasterRepository HolidayMasterRepository { get; }
        IWarningMasterRepository WarningMasterRepository { get; }
        ILevelWiseCardMappingRepository LevelWiseCardMappingRepository { get; }
        ICityCategoryRepository CityCategoryRepository { get; }
        ICompanyDetailsRepository CompanyDetailsRepository { get; }
        IDirectorDetailsRepository DirectorDetailsRepository { get; }
        IEmployeeManageRepository EmployeeManageRepository { get; }
        IEmployeePersonalInfoRepository EmployeePersonalInfoRepository { get; }
        IEmployeeContactRepository EmployeeContactRepository { get; }
        ICountryRepository CountryRepository { get; }
        IThanaRepository ThanaRepository { get; }
        IPageMasterRepository PageMasterRepository { get; }
        IModuleDetailsRepository ModuleDetailsRepository { get; }
        IPagePanelRepository PagePanelRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IEmployeeTypeRepository EmployeeTypeRepository { get; }
        IPrivilegeMasterRepository PrivilegeMasterRepository { get; }
        IPrivilegeDetailsRepository PrivilegeDetailsRepository { get; }

        IEmployeeInOutRepository EmployeeInOutRepository { get; }

        IEarningRepository EarningRepository { get; }
        IDeductionRepository DeductionRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IRolePermissionRepository RolePermissionRepository { get; }
        IRoleRepository RoleRepository { get; }

        IHRMSUserRoleRepository HRMSUserRoleRepository { get; }
        ISuperAdminDetailsRepository SuperAdminDetailsRepository { get; }

        IEmpAttendanceRepository EmpAttendanceRepository { get; }
        ILeaveDetailsRepository LeaveDetailsRepository { get; }
        ILeaveMasterRepository LeaveMasterRepository { get; }
        ICompOffDetailsRepository CompOffDetailsRepository { get; }
        ILeaveOpeningRepository LeaveOpeningRepository { get; }

        ILeaveApplicationRepository LeaveApplicationRepository { get; }


        IEmployeeSalaryAllowanceRepository EmployeeSalaryAllowanceRepository { get; }
        IUserCompanyPermissionsRepository UserCompanyPermissionsRepository { get; }
        IEmergencyContactRepository EmergencyContactRepository { get; }
        IBusinessSegmentRepository BusinessSegmentRepository { get; }
        IRelationShipRepository RelationShipRepository { get; }
        IPasswordHistory PasswordHistory { get; }

        IEmployeeReport EmployeeReport { get; }
        IAttendanceRegularizationRepository AttendanceRegularizationRepository { get; }
        IAttachmentDetailsRepository AttachmentDetailsRepository { get; }
        IContractDetailsRepository ContractDetailsRepository { get; }
        IProjectDetailsRepository ProjectDetailsRepository { get; }
        IReportingManagerDetailsRepository ReportingManagerDetailsRepository { get; }

        IEmployeeDashboardRepository EmployeeDashboardRepository { get; }
        IMonthlySalaryDetailsRepository MonthlySalaryDetailsRepository { get; }
        IMyProfileRepository myProfileRepository { get; }
        IEmployeeDirectory employeeDirectory { get; }
        ILeftEmployeeRepository leftEmployeeRepository { get; }
        IUserPermissionRepository UserPermissionRepository { get; }
        INotificationRemainderRepository NotificationRemainderRepository { get; }


        IEmployeeProfileSkillRepository employeeProfileSkillRepository { get; }
        IEmployeeProfileEducationRepository employeeProfileEducationRepository { get; }
        IEmployeeProfileExperienceRepository employeeProfileExperienceRepository { get; }
        IEmployeeProfileLanguageRepository employeeProfileLanguageRepository { get; }

        ITicketStatusRepository TicketStatusRepository { get; }

        void Commit();
        Task CommitAsync();
    }
}
