using AppInterfaces.Infrastructure;
using AppInterfaces.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RepositoryLayer
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected IDbSet<T> _objectSet;
        protected readonly IEntitiesContext _context;
        protected readonly IUnitOfWork _uow;


        #region Ctor
        public BaseRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _context = _uow.Context;
            _objectSet = _context.Set<T>();
        }

        #endregion
        protected IUnitOfWork UOW
        {
            get { return _uow; }
        }
        protected IEntitiesContext Context
        {
            get { return _context; }
        }
        //protected IUnitOfWork _uow { get}
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <typeparam name="T">Db Model</typeparam>
        /// <returns></returns>
        protected TObj GetContext<TObj>() where TObj
            : class
        {
            return (TObj)Convert.ChangeType(_context, Type.GetTypeCode(typeof(TObj)));
        }
        public IQueryable<T> AsQuerable()
        {
            return _objectSet.AsQueryable();
        }
        public IEnumerable<T> GetAll(Func<T, bool> predicate = null)
        {
            if (predicate != null)
            {
                return _objectSet.Where(predicate);
            }
            return _objectSet.AsEnumerable();
        }
        public T Get(Func<T, bool> predicate)
        {
            return _objectSet.FirstOrDefault(predicate);
        }
        public bool Any(Func<T, bool> predicate = null)
        {
            if (predicate != null)
            {
                return _objectSet.Any(predicate);
            }
            return _objectSet.Any();
        }
        public void Add(T entity)
        {
            _objectSet.Add(entity);
        }
        public void Attach(T entity)
        {
            _objectSet.Attach(entity);
        }
        public void Delete(T entity)
        {
            _objectSet.Remove(entity);
        }
        protected void SaveChanges()
        {
           _context.SaveChanges();
        }
    }
}
