using MvcProject1.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject1.BLL.RepositoryInterfaces
{
    public interface IDepartmentRepository: ICrudGenericRepository<Department>
    {
        
    }
}
