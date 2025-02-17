using MvcProject1.DAL.Models;
using System.Linq.Expressions;


namespace MvcProject1.BLL.RepositoryInterfaces
{
    public interface ICrudGenericRepository<T> where T : ModelBase
    {
        public void Add(T entity);

        public void Delete(int id);

        public void DeleteAll();

        public T Get(int id);

        public IEnumerable<T> GetAll();

        public void Update(T entity);

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression);

    }
}
