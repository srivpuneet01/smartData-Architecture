using System;
using System.Collections.Generic;
using System.Linq;

namespace AppInterfaces.Repository
{
    public interface IBaseRepository<T>
    {
        IEnumerable<T> GetAll(Func<T, bool> predicate = null);
        IQueryable<T> AsQuerable();
        T Get(Func<T, bool> predicate);

        bool Any(Func<T, bool> predicate = null);
        void Add(T entity);
        void Attach(T entity);
        void Delete(T entity);
    }
}
