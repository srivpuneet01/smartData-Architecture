using Core.Domain;
using Newtonsoft.Json.Linq;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
            re.data = _userService.GetAllUsers(filter.limit, filter.offset, filter.order, filter.sort, filter.FirstName, filter.LastName, filter.Email, out total);
            return _userService.GetAllUsers();
        }
    }
}
