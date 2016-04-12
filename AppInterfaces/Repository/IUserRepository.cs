using Core.Domain;
using System;
using System.Collections.Generic;

namespace AppInterfaces.Interfaces
{
    public interface IUserRepository
    {
        Users CreateUser(UserInsert user);

        List<Users> GetAllUsers(int limit, int offset, string order, string sort, string firstName, string lastName, string email, out int total);

        Users GetUserById(int? id);

        String EditUser(Users user);

        bool DeleteUser(int id);

        List<Users> GetAllUsers();
       
        List<Roles> GetAllRoles();
        
        void CreateUserRoles(int userId,string roles);
        
        List<UserRoles> GetUserRolesById(int userId);
        
        bool DeleteUserRolesById(int userId, string ids);
        
        string  GetIsSuperAdmin(string tokenID);

        void MassUpdateUser(string userNames, string unCheckIds, int modifyBy);
        
        List<Users> GetUsersByUserID(int userId);
        bool IsUserPermitedtoDelete(long userID);

        bool UpdatePassword(string email, string passwordVerificationToken);

        void AddPasswordHistory(SecUserPasswordHistory secUserPasswordHistory);
        List<Users> GetUsersByEmail(string email);
    }
}
