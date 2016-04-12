using Core.Domain;
using System;
using System.Collections.Generic;
using AppInterfaces.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer.Services
{
    public class UserService : IUserService
    {
        IUserRepository _userRepository = null;
        #region ctor
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion

        #region public methods
        public Users CreateUser(UserInsert user)
        {
            Users _usersObject = _userRepository.CreateUser(user);
            return _usersObject;
        }
        public List<Users> GetAllUsers(int limit, int offset, string order, string sort, string firstName, string lastName, string email, out int total)
        {
            return _userRepository.GetAllUsers(limit, offset, order, sort, firstName, lastName, email, out total);
        }
        public Users GetUserById(int? id)
        {
            return _userRepository.GetUserById(id);
        }
        public String EditUser(Users user)
        {
            user.Active = true;
            user.CreatedDate = DateTime.Now;
            return _userRepository.EditUser(user);
        }
        public bool DeleteUser(int id)
        {
            return _userRepository.DeleteUser(id);
        }
        public List<Users> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }
        public List<Roles> GetAllRoles()
        {
            return _userRepository.GetAllRoles();
        }

        public List<UserRoles> GetUserRolesById(int userId)
        {
            return _userRepository.GetUserRolesById(userId);
        }

        public bool DeleteUserRolesById(int userId, string Ids)
        {
            return _userRepository.DeleteUserRolesById(userId, Ids);
        }
        public void CreateUserRoles(int userId, string roleIds)
        {
            _userRepository.CreateUserRoles(userId, roleIds);
        }
        public string GetIsSuperAdmin(string tokenId)
        {
            return _userRepository.GetIsSuperAdmin(tokenId);
        }
        public void MassUpdateUser(string userNames, string unCheckIds, int modifyBy)
        {
            _userRepository.MassUpdateUser(userNames, unCheckIds, modifyBy);
        }
        public List<Users> GetUsersByUserID(int userId)
        {
            return _userRepository.GetUsersByUserID(userId);
        }
        public bool IsUserPermitedtoDelete(long userID)
        {
            return _userRepository.IsUserPermitedtoDelete(userID);
        }
        public bool UpdatePassword(string un, string rt)
        {
            return _userRepository.UpdatePassword(un, rt);
        }
        public void AddPasswordHistory(SecUserPasswordHistory secUserPasswordHistory)
        {
            _userRepository.AddPasswordHistory(secUserPasswordHistory);
        }
        public List<Users> GetUsersByEmail(string email)
        {
            return _userRepository.GetUsersByEmail(email);
        }
        #endregion

    }
}
