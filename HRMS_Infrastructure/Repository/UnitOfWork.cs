using HRMS_Core.DbContext;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Interface.CompanyStructure;
using HRMS_Infrastructure.Interface.JobMaster;
using HRMS_Infrastructure.Interface.OtherMaster;
using HRMS_Infrastructure.Repository.CompanyStructure;
using HRMS_Infrastructure.Repository.JobMaster;
using HRMS_Infrastructure.Repository.OtherMaster;
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
