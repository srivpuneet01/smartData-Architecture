using Core.Domain;
using System;
using System.Collections.Generic;

namespace ServiceLayer.Interfaces
{
    public interface IUserService
    {
        Users CreateUser(UserInsert user);
        List<Users> GetAllUsers(int limit, int offset, string order, string sort, string FirstName, string LastName, string Email, out int total);
        Users GetUserById(int? id);
        String EditUser(Users user);
        bool DeleteUser(int id);
        List<Users> GetAllUsers();
        List<Roles> GetAllRoles();
        List<UserRoles> GetUserRolesById(int UserId);
        bool DeleteUserRolesById(int UserId, string Ids);
        void CreateUserRoles(int UserId, string RoleIds);
        string GetIsSuperAdmin(string TokenID);
        void MassUpdateUser(string UserNames, string UnCheckIds, int ModifyBy);
        List<Users> GetUsersByUserID(int UserId);
        bool IsUserPermitedtoDelete(long UserID);
        bool UpdatePassword(string un, string rt);
        void AddPasswordHistory(SecUserPasswordHistory secUserPasswordHistory);
        List<Users> GetUsersByEmail(string email);
    }
}
