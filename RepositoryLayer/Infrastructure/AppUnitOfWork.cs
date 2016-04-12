using AppInterfaces.Infrastructure;
using AppInterfaces.Repository;
using RepositoryLayer.Repositories;
using System.Data.Entity;
using System;

namespace RepositoryInfrastructure
{
    public sealed partial class AppUnitOfWork
        : UnitOfWork, IAppUnitOfWork
    {
        public AppUnitOfWork(IEntitiesContext context) : base(context)
        {
        }
    }
}