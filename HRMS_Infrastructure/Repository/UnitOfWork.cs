using HRMS_Core.DbContext;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Interface.ApprovalManagement;
using HRMS_Infrastructure.Interface.CommanReason;
using HRMS_Infrastructure.Interface.CompanyInformation;
using HRMS_Infrastructure.Interface.CompanyStructure;
using HRMS_Infrastructure.Interface.EmailService;
using HRMS_Infrastructure.Interface.EmegencyContacts;
using HRMS_Infrastructure.Interface.Employee;
using HRMS_Infrastructure.Interface.EmployeeMaster;
using HRMS_Infrastructure.Interface.ExportData;
using HRMS_Infrastructure.Interface.JobMaster;
using HRMS_Infrastructure.Interface.Leave;
using HRMS_Infrastructure.Interface.ManagePermissions;
using HRMS_Infrastructure.Interface.ManageProfile;
using HRMS_Infrastructure.Interface.NewFolder;
using HRMS_Infrastructure.Interface.Notifications;
using HRMS_Infrastructure.Interface.OtherMaster;
using HRMS_Infrastructure.Interface.PrivilegeSetting;
using HRMS_Infrastructure.Interface.Probations;
using HRMS_Infrastructure.Interface.Report;
using HRMS_Infrastructure.Interface.Salary;
using HRMS_Infrastructure.Interface.SuperAdmin;
using HRMS_Infrastructure.Repository.ApprovalManagement;
using HRMS_Infrastructure.Repository.CompanyInformation;
using HRMS_Infrastructure.Repository.CompanyStructure;
using HRMS_Infrastructure.Repository.EmailService;
using HRMS_Infrastructure.Repository.EmergencyContacts;
using HRMS_Infrastructure.Repository.Employee;
using HRMS_Infrastructure.Repository.EmployeeMaster;
using HRMS_Infrastructure.Repository.ExportData;
using HRMS_Infrastructure.Repository.JobMaster;
using HRMS_Infrastructure.Repository.Leave;
using HRMS_Infrastructure.Repository.ManagePermissions;
using HRMS_Infrastructure.Repository.ManageProfile;
using HRMS_Infrastructure.Repository.NewFolder;
using HRMS_Infrastructure.Repository.Notifications;
using HRMS_Infrastructure.Repository.OtherMaster;
using HRMS_Infrastructure.Repository.PrivilegeSetting;
using HRMS_Infrastructure.Repository.Probations;
using HRMS_Infrastructure.Repository.Report;
using HRMS_Infrastructure.Repository.Salary;
using HRMS_Infrastructure.Repository.SuperAdmin;
using HRMS_Infrastructure.Repository.TicketManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HRMSDbContext _dbContext;

        public UnitOfWork(HRMSDbContext dbContext)
        {
            _dbContext = dbContext;
            BranchRepository = new BranchRepository(_dbContext);
            CityRepository = new CityRepository(_dbContext);
            DepartmentRepository = new DepartmentRepository(_dbContext);
            DesignationRepository = new DesignationRepository(_dbContext);
            GradeRepository = new GradeRepository(_dbContext);
            ReasonRepository = new ReasonRepository(_dbContext);
            StateRepository = new StateRepository(_dbContext);
            ShiftBreakRepository = new ShiftBreakRepository(_dbContext);
            ShiftMasterRepository = new ShiftMasterRepository(_dbContext);
            OrganizationPolicyRepository = new OrganizationPolicyRepository(_dbContext);
            TicketTypeRepository = new TicketTypeRepository(_dbContext);
            TicketPriorityRepository = new TicketPriorityRepository(_dbContext);
            BankMasterRepository = new BankMasterRepository(_dbContext);
            WeekOffMasterRepository = new WeekOffMasterRepository(_dbContext);
            HolidayMasterRepository = new HolidayMasterRepository(_dbContext);
            WarningMasterRepository = new WarningMasterRepository(_dbContext);
            LevelWiseCardMappingRepository = new LevelWiseCardMappingRepository(_dbContext);
            CityCategoryRepository = new CityCategoryRepository(_dbContext);
            CompanyDetailsRepository = new CompanyDetailsRepository(_dbContext);
            DirectorDetailsRepository = new DirectorDetailsRepository(_dbContext);
            EmployeeManageRepository = new EmployeeManageRepository(_dbContext);
            EmployeePersonalInfoRepository = new EmployeePersonalInfoRepository(_dbContext);
            EmployeeContactRepository = new EmployeeContactRepository(_dbContext);
            CountryRepository = new CountryRepository(_dbContext);
            ThanaRepository = new ThanaRepository(_dbContext);
            PageMasterRepository = new PageMasterRepository(_dbContext);
            ModuleDetailsRepository = new ModuleDetailsRepository(_dbContext);
            PagePanelRepository = new PagePanelRepository(_dbContext);
            CategoryRepository = new CategoryRepository(_dbContext);
            EmployeeTypeRepository = new EmployeeTypeRepository(_dbContext);
            PrivilegeMasterRepository = new PrivilegeMasterRepository(_dbContext);
            PrivilegeDetailsRepository = new PrivilegeDetailsRepository(_dbContext);

            EmployeeInOutRepository = new EmployeeInOutRepository(_dbContext);

            EarningRepository = new EarningRepository(_dbContext); 
            DeductionRepository = new DeductionRepository(_dbContext);
            PermissionRepository = new PermissionRepository(_dbContext);
            RolePermissionRepository = new RolePermissionRepository(_dbContext);
            RoleRepository = new RoleRepository(_dbContext);
            HRMSUserRoleRepository = new HRMSUserRoleRepository(_dbContext);
            SuperAdminDetailsRepository = new SuperAdminDetailsRepository(_dbContext); 
            RoleRepository = new RoleRepository(_dbContext); 
            EmpAttendanceRepository = new EmpAttendanceRepository(_dbContext);

            CompOffDetailsRepository = new CompOffDetailsRepository(_dbContext);
            LeaveMasterRepository = new LeaveMasterRepository(_dbContext);
            LeaveDetailsRepository = new LeaveDetailsRepository(_dbContext);
            LeaveOpeningRepository = new LeaveOpeningRepository(_dbContext);

            LeaveApplicationRepository = new LeaveApplicationRepository(_dbContext);

            EmployeeSalaryAllowanceRepository = new EmployeeSalaryAllowanceRepository(_dbContext);
            UserCompanyPermissionsRepository = new UserCompanyPermissionsRepository(_dbContext);
            EmergencyContactRepository = new EmergencyContactRepository(_dbContext);
            RelationShipRepository = new RelationShipRepository(_dbContext);
            BusinessSegmentRepository = new BusinessSegmentRepository(_dbContext);


            PasswordHistory = new PasswordHistoryRepository(_dbContext);
            EmployeeReport = new EmployeeReportRepository(_dbContext);
            AttendanceRegularizationRepository = new AttendanceRegularizationRepository(_dbContext);

            AttachmentDetailsRepository = new AttachmentDetailsRepository(_dbContext);
            ContractDetailsRepository = new ContractDetailsRepository(_dbContext);
            ProjectDetailsRepository = new ProjectDetailsRepository(_dbContext);
            ReportingManagerDetailsRepository = new ReportingManagerDetailsRepository(_dbContext);

            EmployeeDashboardRepository = new EmployeeDashboardRepository(_dbContext);
            MonthlySalaryDetailsRepository =new MonthlySalaryDetailsRepository(_dbContext);

            employeeDirectory = new EmployeeDirecotryRepository(_dbContext);
            myProfileRepository = new MyProfileRepository(_dbContext);
            employeeDirectory = new EmployeeDirecotryRepository(_dbContext); 
            leftEmployeeRepository = new LeftEmployeeRepository(_dbContext);
            UserPermissionRepository = new UserPermissionRepository(_dbContext);
            NotificationRemainderRepository = new NotificationRemainderRepository(_dbContext);
            TicketStatusRepository = new TicketStatusRepository(_dbContext);
            TicketApplicationRepository = new TicketApplicationRepository(_dbContext);

            employeeProfileSkillRepository = new EmployeeProfileSkillRepository(_dbContext);
            employeeProfileEducationRepository = new EmployeeProfileEducationRepository(_dbContext);
            employeeProfileExperienceRepository = new EmployeeProfileExperienceRepository(_dbContext);
            employeeProfileLanguageRepository = new EmployeeProfileLanguageRepository(_dbContext);
            TicketFollowUpRepository = new TicketFollowUpRepository(_dbContext);
            ManageProfileRepository = new ManageProfileRepository(_dbContext);
            SkillMasterRepository = new SkillMasterRepository(_dbContext);
            ProbationPerformanceRepository = new ProbationPerformanceRepository(_dbContext);
            GeoLocationRepository = new GeoLocationRepository(_dbContext);
            EmployeeIncrementRespository = new EmployeeIncrementRespository(_dbContext);
            ExportDataRepository = new ExportDataRepository(_dbContext);
            AddeventRepository = new AddEventRepository(_dbContext);
            SchemeMasterRepository = new SchemeMasterRepository(_dbContext);
            SchemeTypeRepository = new SchemeTypeRepository(_dbContext);

            ManpowerRequisitionRepository = new ManpowerRequisitionRepository(_dbContext);

            importDataRepository = new ImportDataRepository(_dbContext);
            ReportRepository = new ReportRepository (_dbContext);
            EmailReportRepository = new EmailReportRepository(_dbContext);
            EmailLoggerRepository = new EmailLoggerRepository(_dbContext);
            LeaveTransactionRepository = new LeaveTransactionRepository(_dbContext);

            ManpowerRelationRepository = new ManpowerRelationRepository(_dbContext);
            ManpowerAttachmentRepository = new ManpowerAttachmentRepository(_dbContext);
            EmployeeHolidayMarkingRepository = new EmployeeHolidayMarkingRepository(_dbContext);
            AttendanceLockRepository = new AttendanceLockRepository(_dbContext);
            EmployeeBankDetailsRepository = new EmployeeBankDetailsRepository(_dbContext);
            ApprovalManagementRepository = new ApprovalManagementRepository(_dbContext);

            ApprovalMasterRepository = new ApprovalMasterRepository(_dbContext);

            LeaveCancellationRepository = new LeaveCancellationRepository(_dbContext);
            CommonReasonsRepository = new CommonReasonsRepository(_dbContext);
            LoanApplicationRepository = new LoanApplicationRepository(_dbContext);


        }

        public IBranchRepository BranchRepository { get; set; }

        public ICityRepository CityRepository { get; set; }

        public IDepartmentRepository DepartmentRepository { get; set; }

        public IDesignationRepository DesignationRepository { get; set; }

        public IGradeRepository GradeRepository { get; set; }

        public IReasonRepository ReasonRepository { get; set; }

        public IStateRepository StateRepository { get; set; }

        public IShiftBreakRepository ShiftBreakRepository { get; set; }

        public IShiftMasterRepository ShiftMasterRepository { get; set; }
        public IOrganizationPolicyRepository OrganizationPolicyRepository { get; set; }
        public ITicketTypeRepository TicketTypeRepository { get; set; }
        public ITicketPriorityRepository TicketPriorityRepository { get; set; }
        public IBankMasterRepository BankMasterRepository { get; set; }
        public IWeekOffMasterRepository WeekOffMasterRepository { get; set; }
        public IHolidayMasterRepository HolidayMasterRepository { get; set; }
        public IWarningMasterRepository WarningMasterRepository { get; set; }
        public ILevelWiseCardMappingRepository LevelWiseCardMappingRepository { get; set; }
        public ICityCategoryRepository CityCategoryRepository { get; set; }
        public ICompanyDetailsRepository CompanyDetailsRepository { get; set; }
        public IDirectorDetailsRepository DirectorDetailsRepository { get; set; }
        public IEmployeeManageRepository EmployeeManageRepository { get; set; }
        public IEmployeePersonalInfoRepository EmployeePersonalInfoRepository { get; set; }
        public IEmployeeContactRepository EmployeeContactRepository { get; set; }
        public ICountryRepository CountryRepository { get; set; }
        public IThanaRepository ThanaRepository { get; set; }
        public IPageMasterRepository PageMasterRepository { get; set; }
        public IModuleDetailsRepository ModuleDetailsRepository { get; set; }
        public IPagePanelRepository PagePanelRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public IEmployeeTypeRepository EmployeeTypeRepository { get; set; }
        public IPrivilegeMasterRepository PrivilegeMasterRepository { get; set; }
        public IPrivilegeDetailsRepository PrivilegeDetailsRepository { get; set; }
        public IEarningRepository EarningRepository { get; set; }

        public IDeductionRepository DeductionRepository { get; set; }

        public IEmployeeInOutRepository EmployeeInOutRepository { get; set; }
        public IPermissionRepository PermissionRepository { get; set; }
        public IRolePermissionRepository RolePermissionRepository { get; set; }
        public IRoleRepository RoleRepository { get; set; }
        public IHRMSUserRoleRepository HRMSUserRoleRepository { get; set; }
        public ISuperAdminDetailsRepository SuperAdminDetailsRepository { get; set; }

        public IEmpAttendanceRepository EmpAttendanceRepository { get; set; }

        public ILeaveDetailsRepository LeaveDetailsRepository { get; set; }

        public ILeaveMasterRepository LeaveMasterRepository { get; set; }

        public ICompOffDetailsRepository CompOffDetailsRepository { get; set; }

        public ILeaveOpeningRepository LeaveOpeningRepository {  get; set; }
        public IEmployeeSalaryAllowanceRepository EmployeeSalaryAllowanceRepository {  get; set; }

        public IPasswordHistory PasswordHistory {  get; set; }

        public ILeaveApplicationRepository LeaveApplicationRepository { get; set; }
        public IUserCompanyPermissionsRepository UserCompanyPermissionsRepository { get; set; }
        public IEmergencyContactRepository EmergencyContactRepository { get; set; }
        public IRelationShipRepository RelationShipRepository { get; set; }
        public IBusinessSegmentRepository BusinessSegmentRepository { get; set; }

        public IEmployeeReport EmployeeReport { get; set; }

        public IAttendanceRegularizationRepository AttendanceRegularizationRepository { get; set; }
        public IAttachmentDetailsRepository AttachmentDetailsRepository { get; set; }
        public IContractDetailsRepository ContractDetailsRepository { get; set; }
        public IProjectDetailsRepository ProjectDetailsRepository { get; set; }
        public IReportingManagerDetailsRepository ReportingManagerDetailsRepository { get; set; }

        public IEmployeeDashboardRepository EmployeeDashboardRepository { get; set; }

        public IMonthlySalaryDetailsRepository MonthlySalaryDetailsRepository { get; set; }

        public IMyProfileRepository myProfileRepository { get; set; }

        public IEmployeeDirectory employeeDirectory { get; set; }

        public ILeftEmployeeRepository leftEmployeeRepository { get; set; }
        public IUserPermissionRepository UserPermissionRepository { get; set; }
        public INotificationRemainderRepository NotificationRemainderRepository { get; set; }
        public ITicketStatusRepository TicketStatusRepository { get; set; }
        public ITicketApplicationRepository TicketApplicationRepository { get; set; }


        public IEmployeeProfileSkillRepository employeeProfileSkillRepository { get; set; }
        public IEmployeeProfileEducationRepository employeeProfileEducationRepository { get; set; }
        public IEmployeeProfileExperienceRepository employeeProfileExperienceRepository { get; set; }
        public IEmployeeProfileLanguageRepository employeeProfileLanguageRepository { get; set; }
        public ITicketFollowUpRepository TicketFollowUpRepository { get; set; }
        public IManageProfileRepository ManageProfileRepository { get; set; }
        public ISkillMasterRepository SkillMasterRepository { get; set; }
        public IProbationPerformanceRepository ProbationPerformanceRepository { get; set; }
        public IGeoLocationRepository GeoLocationRepository { get; set; }
        public IEmployeeIncrementRespository EmployeeIncrementRespository { get; set; }
        public IExportDataRepository ExportDataRepository { get; set; }
        public IAddeventRepository AddeventRepository { get; set; }

        public ISchemeMasterRepository SchemeMasterRepository { get; set; }
        public ISchemeTypeRepository SchemeTypeRepository { get; set; }
        public IManpowerRequisitionRepository ManpowerRequisitionRepository { get; set; }

        public IImportDataRepository importDataRepository { get; set; }

        public IReportRepository ReportRepository { get; set; }
        public IEmailReportRepository EmailReportRepository { get; set; }
        public IEmailLoggerRepository EmailLoggerRepository { get; set; }
        public ILeaveTransactionRepository LeaveTransactionRepository { get; set; }
        public IManpowerRelationRepository ManpowerRelationRepository { get; set; }
        public IManpowerAttachmentRepository ManpowerAttachmentRepository { get; set; }

        public IEmployeeHolidayMarkingRepository EmployeeHolidayMarkingRepository { get; set; }

        public IAttendanceLockRepository AttendanceLockRepository { get; set; }
        public IEmployeeBankDetailsRepository EmployeeBankDetailsRepository { get; set; }
        public IApprovalManagementRepository ApprovalManagementRepository { get; set; }
        public IApprovalMasterRepository ApprovalMasterRepository { get; set; }

        public ILeaveCancellationRepository LeaveCancellationRepository { get; set; }

        public ICommonReasonsRepository CommonReasonsRepository { get; set; }

        public ILoanApplicationRepository LoanApplicationRepository { get; set; }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
