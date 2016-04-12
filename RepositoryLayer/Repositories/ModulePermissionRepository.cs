using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain;
using CoreEntities.CustomModels;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using AppInterfaces.Infrastructure;
using AppInterfaces.Interfaces;

namespace RepositoryLayer.Repositories
{
    public class ModulePermissionRepository : BaseRepository<ModulePermission>, IModulePermissionRepository
    {

        #region Initialize

        public ModulePermissionRepository(IAppUnitOfWork uow) : base(uow)
        {
        }
        #endregion

        #region Public Methods
        public List<ScreenPermissionList> GetAllScreenPermission(int moduleID, int roleId)
        {
            DbDataReader reader;
            Context.Database.Initialize(force: false);
            Context.Database.Connection.Open();
            var dbCmd = Context.Database.Connection.CreateCommand();
            dbCmd.CommandText = "ssp_GetModulePermission";
            dbCmd.CommandType = CommandType.StoredProcedure;
            dbCmd.CommandTimeout = 60 * 5;
            dbCmd.Parameters.AddRange(new SqlParameter[] {
                         new SqlParameter("@ModuleID", moduleID),
                         new SqlParameter("@RoleId", roleId)
                    });
            reader = dbCmd.ExecuteReader();
            return ((IObjectContextAdapter)Context).ObjectContext.Translate<ScreenPermissionList>(reader).ToList();
        }

        public ResponseModel CreateFullPermission(ModulePermission modulepermission)
        {
            ResponseModel result = new ResponseModel();

            if (!Context.Set<ModulePermission>().Any(x => x.RoleId == modulepermission.RoleId && x.ModuleId == modulepermission.ModuleId && x.IsDeleted == false))
            {
                modulepermission.CreatedBy = 0;//TODO
                modulepermission.CreatedDate = DateTime.UtcNow;
                modulepermission.IsDeleted = false;

                Context.Set<ModulePermission>().Add(modulepermission);
                Context.SaveChanges();

                result.ResponseStatus = true;
                result.ResponseText = "Created Successfully.";
            }
            else
            {
                result.ResponseStatus = false;
                result.ResponseText = "Sorry Duplicate Records exists.";
            }

            return result;
        }

        public List<ModulePermission> GetModulePermission(int moduleID, int roleId)
        {
            return GetAll(x => !x.IsDeleted && x.ModuleId == moduleID && x.RoleId == roleId).ToList();
        }

        public bool DeleteModulePermission(int moduleID, int roleId)
        {
            bool status = false;

            ModulePermission modulePermission = Get(x => x.ModuleId == moduleID && x.RoleId == roleId && x.IsDeleted == false);
            if (modulePermission != null)
            {
                //Attach(modulePermission);
                modulePermission.IsDeleted = true;
                modulePermission.DeletedBy = 1;
                modulePermission.DeletedDate = DateTime.UtcNow;
                Context.SaveChanges();
                status = true;
                return status;
            }
            return status;
        }
        #endregion
    }
}
