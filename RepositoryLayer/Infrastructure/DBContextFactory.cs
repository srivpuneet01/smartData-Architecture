using AppInterfaces.Infrastructure;
using RepositoryLayer;
using System.Data.Entity;

namespace RepositoryInfrastructure
{
    /// <summary>
    /// Context Factory for project
    /// </summary>
    public sealed class DBContextFactory
        : IContextFactory<DbContext>
    {
        /// <summary>
        /// The connection string
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyProjectContextFactory"/> class.
        /// </summary>
        /// <param name="connectionStringOrName">Name of the connection string or actual connection string.</param>
        public DBContextFactory(string connectionStringOrName)
        {
            _connectionString = connectionStringOrName;
        }

        /// <summary>
        /// Creates new database context.
        /// </summary>
        /// <returns>DbContext: <see cref="MyProjectContext"/></returns>
        public DbContext Create()
        {
            return new AzureDBContext(_connectionString);
        }
    }
}