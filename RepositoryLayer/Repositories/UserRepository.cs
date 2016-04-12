using AppInterfaces.Infrastructure;
using AppInterfaces.Interfaces;
using Core.Domain;
using Core.enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using WebMatrix.WebData;

namespace RepositoryLayer.Repositories
{
    public class UserRepository : BaseRepository<Users>, IUserRepository
    {
        #region ctor
        public UserRepository(IAppUnitOfWork uow)
            : base(uow)
        {
        }
        #endregion

        #region public methods
        public Users CreateUser(UserInsert user)
        {
            //UOW.StartTransaction();
            var scopeOption = new TransactionOptions();
            scopeOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, scopeOption))
            {
                var userObject = AddUser(user);

                if (userObject != null && !string.IsNullOrEmpty(userObject.RoleIDs.ToString()))
                    AddUserRoles(userObject.UserId, userObject.RoleIDs.ToString());
                //WebSecurity.CreateAccount(user.Email, user.Password);
                //UOW.CommitTransaction();
                scope.Complete();
                return userObject;
            }
        }
        public void CreateUserRoles(int userID, string roleIds)
        {
            AddUserRoles(userID, roleIds);
        }
        public void EditUserRoles(int userId, string roleIds)
        {
            UOW.StartTransaction();
            UpdateUserRoles(userId, roleIds);
            UOW.CommitTransaction();
        }
        public String EditUser(Users user)
        {
            UOW.StartTransaction();
            var result = UpdateUser(user);
            UOW.CommitTransaction();

            return result;
        }
        public List<Users> GetAllUsers(int limit, int offset, string order, string sort, string firstName, string lastName, string email, out int total)
        {
            var data = Context.Set<Users>().Where(x => x.IsDeleted == false);
            if (!string.IsNullOrEmpty(firstName))
            {
                data = data.Where(x => x.FirstName.StartsWith(firstName));
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                data = data.Where(x => x.LastName.StartsWith(lastName));
            }
            if (!string.IsNullOrEmpty(email))
            {
                data = data.Where(x => x.Email.StartsWith(email));
            }

            GetSortedData(ref data, sort + order.ToUpper());

            total = data.Count();
            return data.Skip(offset).Take(limit).ToList();
        }
        public bool UpdatePassword(string email, string passwordVerificationToken)
        {
            return (from i in Context.Set<Users>()
                    join w in Context.Set<webpages_Membership>() on i.UserId equals w.UserId
                    where i.Email == email
                    && w.PasswordVerificationToken == passwordVerificationToken
                    && i.IsDeleted == false
                    select i).Any();

        }
        public void GetSortedData(ref IQueryable<Users> userQuery, string sortingOrder)
        {
            switch (sortingOrder)
            {
                case "FirstNameASC":
                    userQuery = userQuery.OrderBy(x => x.FirstName.ToLower());
                    break;
                case "LastNameASC":
                    userQuery = userQuery.OrderBy(x => x.LastName.ToLower());
                    break;
                case "EmailASC":
                    userQuery = userQuery.OrderBy(x => x.Email.ToLower());
                    break;
                case "FirstNameDESC":
                    userQuery = userQuery.OrderByDescending(x => x.FirstName.ToLower());
                    break;
                case "LastNameDESC":
                    userQuery = userQuery.OrderByDescending(x => x.LastName.ToLower());
                    break;
                case "EmailDESC":
                    userQuery = userQuery.OrderByDescending(x => x.Email.ToLower());
                    break;
                case "UserIdDESC":
                    userQuery = userQuery.OrderByDescending(x => x.UserId);
                    break;
                case "UserIdASC":
                    userQuery = userQuery.OrderBy(x => x.UserId);
                    break;
                default:
                    userQuery = userQuery.OrderByDescending(x => x.UserId);
                    break;
            }
        }
        public Users GetUserById(int? id)
        {
            var objUser = Get(x => x.UserId == id && !x.IsDeleted && x.Active);
            return objUser;
        }
        public bool DeleteUser(int id)
        {

            Users user = Get(x => x.UserId == id && !x.IsDeleted);
            user.IsDeleted = true;
            user.DeletedBy = 1;//TODO
            user.DeletedDate = DateTime.UtcNow;
            Context.SaveChanges();
            return true;

        }
        public IQueryable<T> SprocExecuteList<T>(string storeprocName, params object[] sqlDbParam) where T : BaseEntity, new()
        {
            return Context.ExecuteStoredProcedureList<T>(storeprocName, sqlDbParam).AsQueryable();
        }
        public int AuthenticateUser(string email, string password, ref Core.Domain.Users model)
        {
            int status = 0;
            var userData = Get(x => x.Email == email && x.Password == password);
            if (userData != null)
            {
                if (!userData.Active)
                {
                    //User is Inactive for Now
                    status = (int)UserLoginMessage.InActive;
                }

                if (userData.IsDeleted)
                {
                    //User has been deleted
                    status = (int)UserLoginMessage.IsDeleted;
                }

                else
                {
                    //User is valid to Login
                    status = (int)UserLoginMessage.ValidUser;
                    model.Email = email;
                    model.FirstName = userData.FirstName;
                    model.LastName = userData.LastName;
                    model.UserId = userData.UserId;
                }
            }

            return status;
        }
        public List<Users> GetAllUsers()
        {
            return GetAll().Where(x => x.IsDeleted == false).OrderByDescending(x => x.UserId).ToList();
        }
        public List<Roles> GetAllRoles()
        {
            return Context.Set<Roles>().Where(x => x.IsDeleted == false && x.Active == true).ToList();
        }
        public List<UserRoles> GetUserRolesById(int userId)
        {
            return Context.Set<UserRoles>().Where(x => x.UserId == userId && x.IsDeleted == false).ToList();

        }
        public bool DeleteUserRolesById(int userId, string ids)
        {
            string[] roleIds = ids.Split(',');
            if (roleIds.Length > 0)
            {
                for (int i = 0; i < roleIds.Length; i++)
                {
                    if (roleIds[i].Length > 0)
                    {
                        var uid = int.Parse(roleIds[i]);

                        var userRoles = Context.Set<UserRoles>().Where(s => s.RoleId == uid && s.UserId == userId && s.IsDeleted == false).FirstOrDefault();

                        if (userRoles != null)
                        {
                            userRoles.IsDeleted = true;
                            userRoles.DeletedBy = 1;
                            userRoles.DeletedDate = DateTime.UtcNow;
                        }
                    }
                }
                Context.SaveChanges();
            }
            return true;
        }
        public string GetIsSuperAdmin(string tokenID)
        {

            var query = (from U in Context.Set<Users>().AsEnumerable()
                         join log in Context.Set<LoginAuthentication>().AsEnumerable() on U.UserId equals log.UserId
                         where log.Active == false && log.TokenId == Guid.Parse(tokenID.ToString())
                         select new { IsSuperAdmin = U.IsSuperAdmin }).Single();
            return query.IsSuperAdmin.ToString();

        }
        public void MassUpdateUser(string userNames, string unCheckIds, int modifyBy)
        {
            List<Roles> rolesList = new List<Roles>();
            if (!string.IsNullOrEmpty(userNames))
            {
                string tokenID = "";
                DbDataReader _reader;
                Context.Database.Initialize(force: false);
                Context.Database.Connection.Open();
                var _dbCmd = Context.Database.Connection.CreateCommand();
                _dbCmd.CommandText = "ssp_MassUpdateUser";
                _dbCmd.CommandType = CommandType.StoredProcedure;
                _dbCmd.CommandTimeout = 60 * 5;
                _dbCmd.Parameters.AddRange(new SqlParameter[] {
                       new SqlParameter("@UserNames", userNames),
                       new SqlParameter("@unCheckIds", unCheckIds),
                       new SqlParameter("@active", modifyBy)
                      // new SqlParameter("@CurrentPage", objEducationContentSearchArea.CurrentPage)
                  });
                _reader = _dbCmd.ExecuteReader();
               while (_reader.Read())
                {
                    tokenID = _reader["flag"].ToString();
                }
            }
        }
        public List<Users> GetUsersByUserID(int userId)
        {
            return GetAll(x => x.IsDeleted == false && x.UserId == userId).ToList();
        }
        public bool DeleteUserByID(int id)
        {
            bool flag = false;

            if (!string.IsNullOrEmpty(id.ToString()))
            {
                string TokenID = "";
                DbDataReader _reader;
                Context.Database.Initialize(force: false);
                Context.Database.Connection.Open();
                var _dbCmd = Context.Database.Connection.CreateCommand();
                _dbCmd.CommandText = "ssp_DeleteUserByID";
                _dbCmd.CommandType = CommandType.StoredProcedure;
                _dbCmd.CommandTimeout = 60 * 5;
                _dbCmd.Parameters.AddRange(new SqlParameter[] {
                       new SqlParameter("@id", id)
                      
                       //new SqlParameter("@active", ModifyBy)
                      // new SqlParameter("@CurrentPage", objEducationContentSearchArea.CurrentPage)
                  });
                _reader = _dbCmd.ExecuteReader();
                while (_reader.Read())
                {
                    TokenID = _reader["flag"].ToString();
                    flag = true;
                }
           }
            return flag;
        }
        public bool IsUserPermitedtoDelete(long userID)
        {
            return Context.Set<Users>().Where(x => x.UserId == userID).Select(x => x.IsSuperAdmin).FirstOrDefault();
        }
        #endregion


        #region private methods
        private Users AddUser(UserInsert user)
        {
            if (!Context.Set<Users>().Any(x => x.Email.ToLower() == user.Email.ToLower() && x.IsDeleted == false))
            {
                user.CreatedBy = 0;
                user.CreatedDate = DateTime.UtcNow;
                user.IsDeleted = false;

                user.UserType = 1;//TODO

                Users userObject = new Users();
                userObject.Active = user.Active;
                userObject.AuthFacebookId = user.AuthFacebookId;
                userObject.ConfirmPassword = user.ConfirmPassword;
                userObject.CreatedBy = user.CreatedBy;
                userObject.CreatedDate = user.CreatedDate;
                userObject.DeletedBy = user.DeletedBy;
                userObject.DeletedDate = user.DeletedDate;
                userObject.Email = user.Email;
                userObject.FirstName = user.FirstName;
                userObject.IsDeleted = user.IsDeleted;
                userObject.LastName = user.LastName;
                userObject.Password = user.Password;
                userObject.RoleIDs = user.RoleIDs;
                userObject.UserId = user.UserId;
                userObject.UserType = user.UserType;
                userObject.IsSuperAdmin = user.IsSuperAdmin;
                Context.Set<Users>().Add(userObject);
                Context.SaveChanges();
                return userObject;
            }
            return null;
        }

        private void AddUserRoles(int userID, string roleIds)
        {
            List<UserRoles> userRoleList = new List<UserRoles>();
            string[] ids = roleIds.Split(',');
            if (ids.Length > 0)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if (ids[i].Length > 0)
                    {
                        var uid = userID;
                        var rid = int.Parse(ids[i]);
                        if (!Context.Set<UserRoles>().Any(x => x.UserId == uid && x.RoleId == rid && x.IsDeleted == false))
                        {
                            UserRoles userRoles = new UserRoles();
                            userRoles.RoleId = int.Parse(ids[i]);
                            userRoles.UserId = userID;
                            userRoles.CreatedBy = 0;//TODO
                            userRoles.CreatedDate = DateTime.UtcNow;
                            userRoles.IsDeleted = false;
                            userRoleList.Add(userRoles);
                        }

                    }
                }
                Context.Set<UserRoles>().AddRange(userRoleList);
                Context.SaveChanges();
            }
        }

        private void UpdateUserRoles(int userId, string roleIds)
        {
            List<UserRoles> userRoleList = new List<UserRoles>();
            var alreadyAssignedRoles = Context.Set<UserRoles>().Where(x => x.UserId == userId && x.IsDeleted == false).ToList();

            foreach (var item in alreadyAssignedRoles)
            {
                Context.Set<UserRoles>().Remove(item);
            }
            Context.SaveChanges();

            string[] ids = roleIds.Split(',');
            if (ids.Length > 0)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if (ids[i].Length > 0)
                    {
                        var uid = userId;
                        var rid = int.Parse(ids[i]);
                        if (!Context.Set<UserRoles>().Any(x => x.UserId == uid && x.RoleId == rid && x.IsDeleted == false))
                        {
                            UserRoles userRoles = new UserRoles();
                            userRoles.RoleId = int.Parse(ids[i]);
                            userRoles.UserId = userId;
                            userRoles.CreatedBy = 0;
                            userRoles.CreatedDate = DateTime.UtcNow;
                            userRoles.IsDeleted = false;
                            userRoleList.Add(userRoles);
                        }

                    }
                }
                Context.Set<UserRoles>().AddRange(userRoleList);
                Context.SaveChanges();
            }
        }


        private String UpdateUser(Users user)
        {
            var userExist = GetAll(x => x.Email.ToLower() == user.Email.ToLower() && x.UserId != user.UserId && !x.IsDeleted && x.Active).Any();
            if (userExist)
            {
                return "Error Email already exists";
            }

            var userObject = Get(x => x.UserId == user.UserId);
            userObject.FirstName = user.FirstName;
            userObject.LastName = user.LastName;
            userObject.Email = user.Email;
            userObject.Active = user.Active;
            userObject.CreatedDate = user.CreatedDate;
            userObject.ModifiedBy = user.ModifiedBy;
            userObject.ModifiedDate = user.CreatedDate;
            userObject.RoleIDs = user.RoleIDs;
            userObject.RolesList = user.RolesList;
            Context.SaveChanges();

            if (userObject.RoleIDs!=null)
            {
                UpdateUserRoles(userObject.UserId, userObject.RoleIDs.ToString());
            }

            //if (!string.IsNullOrEmpty((userObject.RoleIDs.ToString())))
            //{
            //    UpdateUserRoles(userObject.UserId, userObject.RoleIDs.ToString());
            //}
            return "true";
        }


        public void AddPasswordHistory(SecUserPasswordHistory secUserPasswordHistory)
        {
            Context.Set<SecUserPasswordHistory>().Add(secUserPasswordHistory);
        }

        public List<Users> GetUsersByEmail(string email)
        {
            return GetAll().Where(x => x.IsDeleted == false && x.Email.ToLower() == email.ToString().ToLower() && x.Active == true).ToList();
        }

        #endregion
    }

}
