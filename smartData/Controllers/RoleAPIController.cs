using Core.Domain;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace smartData.Controllers
{
    public class RoleAPIController : ApiController
    {
        ServiceLayer.Interfaces.IRolesService _roleService;
        public RoleAPIController(IRolesService rolesService)
        {
            _roleService = rolesService;
            //System.Net.Http.Headers.contContentType = new MediaTypeHeaderValue("application/json");
        }


        [HttpPost]
        [AllowAnonymous]
        public IEnumerable<Roles> GetRole(RoleListFilters filter)
        {
            try
            {
                int total;
                RoleReturnVal re = new RoleReturnVal();
                filter.order = "asc";
                filter.limit = 10000;
                re.data = _roleService.GetAllRoles(filter.limit, filter.offset, filter.order, filter.sort, filter.RoleName, out total);
                //var result = new
                //{
                //    total = total,
                //    rows = re.data,
                //};
                return re.data;
            }
            catch (Exception)
            {
                throw new HttpException(404, "Some Error Occured");
            }
        }
    }
}
