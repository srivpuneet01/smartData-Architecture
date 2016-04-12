using Core.Domain;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using AppInterfaces.Infrastructure;
using AppInterfaces.Interfaces;

namespace RepositoryLayer.Repositories
{
    public class PagePermissionRepository : BaseRepository<LogInfo>, IPagePermissionRepository
    {
        #region Initialize

        public PagePermissionRepository(IAppUnitOfWork uow)
            : base(uow)
        {
        }
        #endregion

        #region  Public Methods
        public string GetAllowPermission(string tokenId, string screenName, string actionName)
        {
            DbDataReader reader;
            Context.Database.Initialize(force: false);
            Context.Database.Connection.Open();
            var dbCmd = Context.Database.Connection.CreateCommand();
            dbCmd.CommandText = "ssp_GetAllowPermission";
            dbCmd.CommandType = CommandType.StoredProcedure;
            dbCmd.CommandTimeout = 60 * 5;
            dbCmd.Parameters.AddRange(new SqlParameter[] {
                       new SqlParameter("@TokenId", tokenId),
                       new SqlParameter("@ScreenName", screenName),
                       new SqlParameter("@ActionName", actionName)
                  });
            reader = dbCmd.ExecuteReader();

            while (reader.Read())
            {
                tokenId = reader["IsAllowPermission"].ToString();
            }

            return tokenId;
        }
        #endregion
    }
}
