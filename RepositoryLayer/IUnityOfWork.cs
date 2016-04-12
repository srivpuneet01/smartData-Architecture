using AppInterfaces.Repository;
using Core;
using Core.Domain;
using Core.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Transactions;

namespace RepositoryLayer
{
    public interface IUnitOfWork : IDisposable
    {
        IEfRepository<T> Repository<T>() where T : class;
        void Commit();
        void Rollback();
    }
}

