using Core.Domain;
using Newtonsoft.Json.Linq;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebMatrix.WebData;

namespace smartData.Controllers
{
    public class UserAPIController : ApiController
    {

        IUserService _userService;

        public UserAPIController(IUserService _UserService)
        {
            _userService = _UserService;
        }

        [HttpPost]
        public IEnumerable<Users> GetAllUser(JObject Obj)
        {

            UserListFilters filter = Obj.ToObject<UserListFilters>();
            int total;
            UserReturnVal re = new UserReturnVal();
            //re.data = _userService.GetAllUsers(filter.limit, filter.offset, filter.order, filter.sort, filter.FirstName, filter.LastName, filter.Email, out total);
            return _userService.GetAllUsers();
        }


        [HttpPost]
        public string InsertUser(UserInsert user)
        {

            if (ModelState.IsValid)
            {
                user.Active = true;
                Core.Domain.Users _usersObject = null;
                _usersObject = _userService.CreateUser(user);
                WebSecurity.CreateAccount(_usersObject.Email, _usersObject.Password);
                return "Sucess";

            }
            else { return "Fail"; }

        }


    }
}
