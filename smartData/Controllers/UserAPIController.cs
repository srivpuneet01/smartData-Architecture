using Core.Domain;
using Newtonsoft.Json.Linq;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
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
        public IEnumerable<Users> GetAllUser(UserListFilters filter)
        {

            //UserListFilters filter = Obj.ToObject<UserListFilters>();
            int total;
            filter.order = "asc";
            filter.limit = 10000;
            UserReturnVal re = new UserReturnVal();
            re.data = _userService.GetAllUsers(filter.limit, filter.offset, filter.order, filter.sort, filter.FirstName, filter.LastName, filter.Email, out total);
            return re.data;
        }


        [HttpPost]
        public string InsertUser(UserInsert user)
        {

            if (ModelState.IsValid)
            {
                user.Active = true;
                Core.Domain.Users _usersObject = null;
                try
                {
                    //user.RolesList = _userService.GetAllRoles();
                    var validateRole = (user.RolesList.Select(x => x.Id).ToArray()).Select(x => x.ToString()).ToArray();
                    string _roles = "";
                    foreach (var item in validateRole)
                    {
                        _roles = _roles + "," + item;
                    }
                    user.RoleIDs = _roles;
                    _usersObject = _userService.CreateUser(user);

                    WebSecurity.CreateAccount(_usersObject.Email, _usersObject.Password);
                    //return RedirectToAction("Index");
                    return "";
                }
                catch (Exception ex)
                {
                    //ViewBag.logUser = WebSecurity.CurrentUserId;
                    UserInsert obj = new UserInsert();
                    obj.RolesList = _userService.GetAllRoles();

                    List<Core.Domain.Users> list = _userService.GetUsersByEmail(user.Email.ToString());
                    if (list.Count > 0)
                    {
                        //_userService.DeleteUser(_usersObject.UserId);
                        return "This email is already registered";
                    }
                    return "";
                }

            }
            else
            {
                var avc = ModelState.Values.Select(x => x.Errors);
                return "";
            }


        }

        [HttpGet]
        [AllowAnonymous]
        public Users Edit(int? id)
        {

            Core.Domain.Users user = _userService.GetUserById(id);
            if (user == null)
            {
                throw new HttpException(404, "user not found");

            }
            return user;
        }
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<UserRoles> GetUserRolesById(int? id)
        {
            if (id == null)
            { throw new HttpException(404, "Not found"); }
            int a = Convert.ToInt32(id);
            var data = from c in _userService.GetUserRolesById(a)
                       select c;
            return data.ToList();
        }
        [HttpPost]
        [AllowAnonymous]
        public string EditUser(UserInsert user)
        {
            var validateRole = (user.RolesList.Select(x => x.Id).ToArray()).Select(x => x.ToString()).ToArray();
            string _roles = "";
            foreach (var item in validateRole)
            {
                _roles = _roles + "," + item;
            }
            user.RoleIDs = _roles;
            return _userService.EditUser(user);

        }

        [HttpGet]
        [AllowAnonymous]
        public bool DeleteUser(int id)
        {
            bool deleteAcntRes;
            Core.Domain.Users user = _userService.GetUserById(id);
            var res = _userService.DeleteUser(id);
            // Delete user account from webpages_Membership table
            if (res)
            {
                if (user != null)
                {
                    if (((WebMatrix.WebData.SimpleMembershipProvider)Membership.Provider).HasLocalAccount(user.UserId))
                    {
                        deleteAcntRes = ((WebMatrix.WebData.SimpleMembershipProvider)Membership.Provider).DeleteAccount(user.Email);
                    }
                }
            }
            return res;
        }


    }
}
