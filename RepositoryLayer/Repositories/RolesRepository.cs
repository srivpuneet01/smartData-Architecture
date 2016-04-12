using Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Entity;
using System.Data.SqlClient;
using CoreEntities.CustomModels;
using AppInterfaces.Infrastructure;
using AppInterfaces.Interfaces;

namespace RepositoryLayer.Repositories
{
    public class RolesRepository : BaseRepository<Roles>, IRolesRepository
    {

        #region Initialize

        public RolesRepository(IAppUnitOfWork uow)
             : base(uow)
        {
        }
        #endregion

        #region Crud
        public ResponseModel CreateRole(Roles role)
        {
            ResponseModel result = new ResponseModel();

            if (!Any(x => x.RoleName.ToLower() == role.RoleName.ToLower() && x.IsDeleted == false))
            {
                role.CreatedBy = 0;
                role.CreatedDate = DateTime.UtcNow;
                role.IsDeleted = false;
                Add(role);
                SaveChanges();

                result.ResponseStatus = true;
                result.ResponseText = "Role created Successfully.";
            }
            else
            {
                result.ResponseStatus = false;
                result.ResponseText = "Sorry Duplicate Records exists.";
            }
            return result;

        }

        public List<Roles> GetAllRoles(int limit, int offset, string order, string sort, string roleName, out int total)
        {
            var roleSet = GetAll(x => !x.IsDeleted).AsQueryable();
            if (!string.IsNullOrEmpty(roleName))
            {
                roleSet = roleSet.Where(x => x.RoleName.StartsWith(roleName));
            }

            GetSortedData(ref roleSet, sort + order.ToUpper());

            total = roleSet.Count();
            return roleSet.Skip(offset).Take(limit).ToList();
        }

        public void GetSortedData(ref IQueryable<Roles> roleQuery, string sortingOrder)
        {
            switch (sortingOrder)
            {
                case "RoleNameASC":
                    roleQuery = roleQuery.OrderBy(x => x.RoleName.ToLower());
                    break;
                case "RoleNameDESC":
                    roleQuery = roleQuery.OrderByDescending(x => x.RoleName.ToLower());
                    break;
                case "RoleIdESC":
                    roleQuery = roleQuery.OrderByDescending(x => x.RoleId);
                    break;
                case "RoleIdASC":
                    roleQuery = roleQuery.OrderBy(x => x.RoleId);
                    break;
                default:
                    roleQuery = roleQuery.OrderByDescending(x => x.RoleId);
                    break;
            }
        }
        public void EditRole(Roles role)
        {
            var roleObject = Get(x => x.RoleId == role.RoleId);
            roleObject.RoleName = role.RoleName;
            roleObject.Active = role.Active;
            roleObject.ModifiedBy = role.ModifiedBy;
            roleObject.ModifiedDate = DateTime.UtcNow;
            roleObject.CreatedDate = role.CreatedDate;
            SaveChanges();
        }

        public bool DeleteRole(int id)
        {
            var roleObject = Get(x => x.RoleId == id && x.IsDeleted == false);
            if (roleObject != null)
            {
                // Roles role = uow.Repository<Roles>().AsQuerable().FirstOrDefault(x => x.RoleId == id && !x.IsDeleted && x.Active);

                roleObject.IsDeleted = true;
                roleObject.DeletedBy = 1;
                roleObject.DeletedDate = DateTime.UtcNow;
                SaveChanges();
                return true;
            }
            return false;
        }

        public void MassUpdate(string roleNames, string unCheckIds, int modifyBy)
        {
            List<Roles> rolesList = new List<Roles>();
            if (!string.IsNullOrEmpty(roleNames))
            {
                Context.Database.Initialize(force: false);
                Context.Database.Connection.Open();
                var dbCmd = Context.Database.Connection.CreateCommand();
                dbCmd.CommandText = "ssp_MassUpdate";
                dbCmd.CommandType = CommandType.StoredProcedure;
                dbCmd.CommandTimeout = 60 * 5;
                dbCmd.Parameters.AddRange(new SqlParameter[] {
                       new SqlParameter("@rolenames", roleNames),
                       new SqlParameter("@unCheckIds", unCheckIds),
                        new SqlParameter("@active", modifyBy)
                  });
                dbCmd.ExecuteNonQuery();

            }
        }
        #endregion
    }
}
