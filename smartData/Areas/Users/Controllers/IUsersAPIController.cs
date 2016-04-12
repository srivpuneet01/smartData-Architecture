using Core.Domain;
using Newtonsoft.Json.Linq;
using System;

namespace smartData
{
    public interface IUsersAPIController
    {
        Users Create(UserInsert user);
        Users GetUserByID(int? id);
        String EditUser(Users user);
        bool DeleteUser(int id);
        dynamic GetUsers(JObject Obj);
    }
}