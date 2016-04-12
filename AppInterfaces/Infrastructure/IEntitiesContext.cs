using Core.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace AppInterfaces.Infrastructure
{
    public interface IEntitiesContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        void Dispose();
        int SaveChanges();
        DbChangeTracker ChangeTracker { get; }

        IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new();

        Database Database { get; }
    }
}
