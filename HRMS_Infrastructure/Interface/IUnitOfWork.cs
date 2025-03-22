using HRMS_Infrastructure.Interface.JobMaster;
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

        void Commit();
        Task CommitAsync();
    }
}
