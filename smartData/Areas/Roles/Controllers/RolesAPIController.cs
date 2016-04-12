using smartData.Infrastructure;
using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using smartData.Filter;
using ServiceLayer.Services;
using CoreEntities.CustomModels;
using ServiceLayer.Interfaces;

namespace smartData
{
    public class RoleListFilters : GridFilter
    {
        public string RoleName { get; set; }

    }

    public class RoleReturnVal
    {
        public IEnumerable<Roles> data;
        public int total;
    }
    [HandleException]
    public class RolesAPIController : ApiController, IRolesAPIController
    {
        ServiceLayer.Interfaces.IRolesService _roleService;
        public RolesAPIController(IRolesService rolesService)
        {
            _roleService = rolesService;
            //System.Net.Http.Headers.contContentType = new MediaTypeHeaderValue("application/json");
        }

        [HttpPost]
        [HandleAPIExceptionAttribute]
        public ResponseModel CreateRole(Roles role)
        {
            // _roleService = new RolesService();
            return _roleService.CreateRole(role);
        }
        [HttpPost]
        [ActionName("GetUsers")]
        [AntiforgeryValidate]
        public dynamic GetUsers(JObject Obj)
        {
            RoleListFilters filter = Obj.ToObject<RoleListFilters>();
            int total;
            RoleReturnVal re = new RoleReturnVal();
            re.data = _roleService.GetAllRoles(filter.limit, filter.offset, filter.order, filter.sort, filter.RoleName, out total);

            var result = new
            {
                total = total,
                rows = re.data,
            };
            return result;
        }

        [HttpPost]
        [ActionName("EditRole")]
        [AntiforgeryValidate]
        public void EditRole(Roles user)
        {
            _roleService.EditRole(user);
        }
        [HttpPost]
        [ActionName("DeleteRole")]
        [AntiforgeryValidate]
        public bool DeleteRole(int id)
        {
            return _roleService.DeleteRole(id);
        }

    }


    // [HandleException]
    //public class UsersAPIController : ApiController, IUsersAPIController
    //{
}