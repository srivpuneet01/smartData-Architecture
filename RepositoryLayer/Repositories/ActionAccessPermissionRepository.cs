using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using AppInterfaces.Infrastructure;
using CoreEntities.Domain;
using AppInterfaces.Interfaces;

namespace RepositoryLayer.Repositories
{
    public class ActionAccessPermissionRepository : BaseRepository<ActionAccessPermission>, IActionAccessPermissionRepository
    {
        #region ctor
        public ActionAccessPermissionRepository(IAppUnitOfWork uow)
            : base(uow)
        {
        }
        #endregion

        #region  Public Methods
        public void UpdateLoginAuthentication(string userId)
        {
            var objLoginAuthentication = Context.Set<LoginAuthentication>()
                   .Where(x => x.TokenId == Guid.Parse(userId) && x.Active == false).FirstOrDefault();
            objLoginAuthentication.Active = true;
            Context.SaveChanges();
        }
        public List<ScreenPermissionList> GetScreenPermissionByUserIdAndScreenId(string userId, string screenName)
        {
            DbDataReader _reader;
            Context.Database.Initialize(force: false);
            Context.Database.Connection.Open();
            var _dbCmd = Context.Database.Connection.CreateCommand();
            _dbCmd.CommandText = "SSp_GetScreenPermissionByUserIdAndScreenId";
            _dbCmd.CommandType = CommandType.StoredProcedure;
            _dbCmd.CommandTimeout = 60 * 5;
            _dbCmd.Parameters.AddRange(new SqlParameter[] {
                        new SqlParameter("@UserId", userId),
                        new SqlParameter("@ScreenName", screenName)
                    //     new SqlParameter("@CurrentPage", objEducationContentSearchArea.CurrentPage)
                    });
            _reader = _dbCmd.ExecuteReader();
            return ((IObjectContextAdapter)Context).ObjectContext.Translate<ScreenPermissionList>(_reader).ToList();
        }
        public void UpdateLastActivityTime(string tokenId)
        {
            var loginAuthentication = Context.Set<LoginAuthentication>().FirstOrDefault(u => u.TokenId == Guid.Parse(tokenId));
            loginAuthentication.ActivityTime = DateTime.UtcNow;
            Context.SaveChanges();
        }
        #endregion

    }
}
