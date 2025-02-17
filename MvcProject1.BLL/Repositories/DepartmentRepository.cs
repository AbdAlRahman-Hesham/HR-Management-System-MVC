using Mvc.DAL.Data;
using MvcProject1.BLL.Repositories;
using MvcProject1.BLL.RepositoryInterfaces;
using MvcProject1.DAL.Data;
using MvcProject1.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvc.BLL.Repositories
{
    public class DepartmentRepository : CrudGenericRepository<Department>, IDepartmentRepository
    {
        
        public DepartmentRepository(AppDbContext db) : base(db)
        {
        }
    }
}
