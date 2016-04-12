using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using System.Web.Mvc;
using CoreEntities.CustomModels;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using AppInterfaces.Interfaces;
using AppInterfaces.Infrastructure;

namespace RepositoryLayer.Repositories
{
    public class ScreenPermissionRepository : BaseRepository<ScreenPermission>, IScreenPermissionRepository
    {
        #region ctor
        public ScreenPermissionRepository(IAppUnitOfWork uow) : base(uow)
        {
        }
        #endregion

        #region  Public Methods

        public List<ScreenPermission> GetAllScreenPermission()
        {

            var enumList = Enum.GetValues(typeof(ModelType)).OfType<ModelType>().ToList();
            List<ActionType> actionTypeList = new List<ActionType>();

            int j = 0;
            foreach (var i in enumList)
            {
                j++;
                ActionType actionTypeObject = new ActionType();
                actionTypeObject.ActionTypeId = j;
                actionTypeObject.TypeName = i.ToString();
                actionTypeList.Add(actionTypeObject);
            }

            var query = from o in Context.Set<Screen>().AsEnumerable()
                        join c in Context.Set<ScreenAction>().AsEnumerable() on o.screenId equals c.FK_screenId
                        where (o.IsDeleted == false && c.IsDeleted == false)
                        orderby o.screenId, c.ActionType
                        select new ScreenPermission()
                        {

                            ScreenId = o.screenId,
                            ScreenName = o.ScreenName,
                            ActionId = c.ActionId,
                            ActionName = c.ActionName,
                            ActionType = c.ActionType
                        };
            return query.ToList();

        }

        public List<Screen> GetAllScreen()
        {
            return Context.Set<Screen>().Where(x => x.IsDeleted == false).ToList();
        }
        public void AddScreenPermission(string screenActinSet, int roleId, int moduleID)
        {
            List<ScreenPermission> ScreenPermissionList = new List<ScreenPermission>();
            string[] screenActions = screenActinSet.Split('/');
            foreach (string screenAction in screenActions)
            {
                ScreenPermission screenpermission = new ScreenPermission();
                string[] arr = screenAction.Split(',');
                if (arr.Length > 1)
                {
                    var actionId = int.Parse(arr[1]);
                    var screenId = int.Parse(arr[0]);

                    if (!Context.Set<ScreenPermission>().Any(x => x.ActionId == actionId && x.ScreenId == screenId && x.IsDeleted == false && x.RoleId == roleId && x.ModuleID == moduleID))
                    {
                        screenpermission.ScreenId = Convert.ToInt32(arr[0]);
                        screenpermission.ActionId = Convert.ToInt32(arr[1]);
                        screenpermission.ActionName = (arr[2].ToString());
                        screenpermission.CreatedBy = 0;
                        screenpermission.CreatedDate = DateTime.UtcNow;
                        screenpermission.IsDeleted = false;
                        screenpermission.ScreenName = "";
                        screenpermission.RoleId = roleId;
                        screenpermission.ModuleID = moduleID;
                        ScreenPermissionList.Add(screenpermission);
                    }
                }
            }
            Context.Set<ScreenPermission>().AddRange(ScreenPermissionList);
            SaveChanges();
        }
        public bool DeleteScreenPermission(string str, int roleId, int moduleID)
        {
            bool res = false;
            List<ScreenPermission> ScreenPermissionList = new List<ScreenPermission>();
            string[] words = str.Split('/');
            foreach (string word in words)
            {
                string[] arr = word.Split(',');
                if (arr.Length > 1)
                {
                    var SID = int.Parse(arr[0]);
                    var AID = int.Parse(arr[1]);

                    ScreenPermission ScreenPermission = Context.Set<ScreenPermission>().FirstOrDefault(x => x.ScreenId == SID && x.ActionId == AID && x.IsDeleted == false && x.RoleId == roleId && x.ModuleID == moduleID);
                    if (ScreenPermission != null)
                    {
                        //  uow.Repository<ScreenPermission>().Attach(ScreenPermission);
                        ScreenPermission.IsDeleted = true;
                        ScreenPermission.DeletedBy = 1;
                        ScreenPermission.DeletedDate = DateTime.UtcNow;
                        SaveChanges();
                        res = true;
                    }
                }
            }
            return res;
        }

        public List<ScreenPermission> GetPermissionList(int roleId, int moduleID)
        {
            return Context.Set<ScreenPermission>().Where(x => x.IsDeleted == false && x.RoleId == roleId && x.ModuleID == moduleID).ToList();
        }
        public List<Roles> GetAllRoles()
        {
            return Context.Set<Roles>().Where(x => x.IsDeleted == false).ToList();
        }
        public ScreenPermission GetDropDown()
        {
            IEnumerable<SelectListItem> ModuleList = (from x in Context.Set<Module>() where x.IsDeleted == false select new SelectListItem() { Text = x.ModuleName.ToString(), Value = x.ModuleID.ToString() }).ToList();
            IEnumerable<SelectListItem> RoleList = (from x in Context.Set<Roles>() where x.IsDeleted == false && x.Active select new SelectListItem() { Text = x.RoleName, Value = x.RoleId.ToString() }).ToList();
            ScreenPermission screenPermissionObject = new ScreenPermission();
            screenPermissionObject.ModuleList = ModuleList;
            screenPermissionObject.RoleList = RoleList;
            return screenPermissionObject;
        }
        public List<ScreenPermissionList> GetAllScreenPermissionl()
        {
            DbDataReader reader;
            Context.Database.Initialize(force: false);
            Context.Database.Connection.Open();
            var dbCmd = Context.Database.Connection.CreateCommand();
            dbCmd.CommandText = "ssp_GetScreenPermission";
            dbCmd.CommandType = CommandType.StoredProcedure;
            dbCmd.CommandTimeout = 60 * 5;
            reader = dbCmd.ExecuteReader();
            return ((IObjectContextAdapter)Context).ObjectContext.Translate<ScreenPermissionList>(reader).ToList();
        }

        public string GetAuthorizeToken(int UserID)
        {

            string tokenID = "";
            DbDataReader reader = null;
            try
            {
                Context.Database.Initialize(force: false);
                Context.Database.Connection.Open();
                var dbCmd = Context.Database.Connection.CreateCommand();
                dbCmd.CommandText = "ssp_GetAuthorizeToken";
                dbCmd.CommandType = CommandType.StoredProcedure;
                dbCmd.CommandTimeout = 60 * 5;
                dbCmd.Parameters.AddRange(new SqlParameter[] {
                       new SqlParameter("@UserId", UserID),
                       //new SqlParameter("@PageSize", objEducationContentSearchArea.PageSize),
                      // new SqlParameter("@CurrentPage", objEducationContentSearchArea.CurrentPage)
                  });
                reader = dbCmd.ExecuteReader();
                while (reader.Read())
                {
                    tokenID = reader["TokenID"].ToString();
                }
                return tokenID;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (Context.Database.Connection != null && Context.Database.Connection.State != ConnectionState.Closed) Context.Database.Connection.Close();
            }
        }
        #endregion
    }

}

