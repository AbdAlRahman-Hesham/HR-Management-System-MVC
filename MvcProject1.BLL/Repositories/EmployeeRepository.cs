using Mvc.DAL.Data;
using MvcProject1.BLL.RepositoryInterfaces;
using MvcProject1.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject1.BLL.Repositories
{
    public class EmployeeRepository : CrudGenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext db) : base(db)
        {
        }
        
    }
}
