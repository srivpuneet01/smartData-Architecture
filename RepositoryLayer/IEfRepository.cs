using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer
{
    public interface IEfRepository<T>
    {
        IEnumerable<T> GetAll(Func<T, bool> predicate = null);
        IQueryable<T> AsQuerable();
        T Get(Func<T, bool> predicate);
        void Add(T entity);
        void Attach(T entity);
        void Delete(T entity);

        //IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, IList<Expression<Func<T, object>>> includedProperties = null, IList<ISortCriteria<T>> sortCriterias = null);
        //PaginatedList<T> GetPaged(Expression<Func<T, bool>> filter = null, IList<Expression<Func<T, object>>> includedProperties = null, PagingOptions<T> pagingOptions = null);
        //T Find(Expression<Func<T, bool>> filter, IList<Expression<Func<T, object>>> includedProperties = null);
        //void Remove(Expression<Func<T, bool>> filter);
    }
}
