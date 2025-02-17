using Mvc.BLL.Repositories;
using Mvc.DAL.Data;
using MvcProject1.BLL.Repositories;
using MvcProject1.BLL.RepositoryInterfaces;


namespace MvcProject1.BLL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext db;

        public Lazy<IEmployeeRepository> EmployeeRepository { get; set; }
        public Lazy<IDepartmentRepository> DepartmentRepository { get; set; }
        public UnitOfWork(AppDbContext _appDbContext) {
            EmployeeRepository = new Lazy<IEmployeeRepository>
                ( ()=>new EmployeeRepository(_appDbContext));
            DepartmentRepository =new Lazy<IDepartmentRepository>
                (()=> new DepartmentRepository(_appDbContext));
            db = _appDbContext;
        }
        public void Dispose()
        {
            db.Dispose();
        }

        public void saveChange()
        {
            db.SaveChanges();
        }
    }
    public interface IUnitOfWork:IDisposable
    {
        public Lazy<IEmployeeRepository> EmployeeRepository { get; set; }
        public Lazy<IDepartmentRepository> DepartmentRepository { get; set; }
        public void saveChange();
    }
}
