using Mvc.DAL.Data;
using MvcProject1.BLL.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using MvcProject1.DAL.Models;
using System.Linq.Expressions;

namespace MvcProject1.BLL.Repositories
{
    public class CrudGenericRepository<T> : ICrudGenericRepository<T> where T : ModelBase
    {
        private protected readonly AppDbContext _db;

        public CrudGenericRepository(AppDbContext db)
        {
            _db = db;
        }

        // Add a new entity
        public void Add(T entity)
        {
            _db.Set<T>().Add(entity);
           
        }

        // Delete a T by its ID
        public void Delete(int id)
        {
            var entity = _db.Set<T>().Find(id);
            if (entity != null)
            {
                _db.Set<T>().Remove(entity);
               
            }
        }

        // Delete all entities of type T
        public void DeleteAll()
        {
            _db.Set<T>().RemoveRange(_db.Set<T>());
        }

        // Get a T by its ID
        public T? Get(int id)
        {
            // Check if the entity is already tracked locally
            var localEntity = _db.Set<T>().Local.
                FirstOrDefault(e =>  e.Id == id);

            if (localEntity != null)
            {
                return localEntity; // Return the local entity if found
            }

            // If not in local, fetch from the database
            if (typeof(T) == typeof(Employee))
            {
                return _db.Employees.AsNoTracking()
                    .Include(e => e.Department)
                    .FirstOrDefault(e => e.Id == id) as T;
            }

            return _db.Set<T>().AsNoTracking()
                .FirstOrDefault(e => 
                e.Id == id);
        }



        // Get all entities of type T
        public IEnumerable<T> GetAll()
        {
            return _db.Set<T>().AsNoTracking().ToList();
        }

        // Update an existing T
        public void Update(T entity)
        {
            var existingEntity = _db.Set<T>().Local.FirstOrDefault(e => e.Id == entity.Id);
            if (existingEntity != null)
            {
                _db.Entry(existingEntity).State = EntityState.Detached;
            }

            _db.Entry(entity).State = EntityState.Modified;

        }
        public IEnumerable<T> Find(Expression<Func<T,bool>> expression)
        {
            var existingEntity = _db.Set<T>().Where(expression).ToList();
            return existingEntity;
        }

    }
}
