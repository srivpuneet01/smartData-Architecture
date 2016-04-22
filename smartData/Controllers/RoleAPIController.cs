using Core.Domain;
using CoreEntities.CustomModels;
using ServiceLayer.Interfaces;
using smartData.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace smartData.Controllers
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
        [HttpPost]
        [AllowAnonymous]
        public ResponseModel CreateRole(Roles role)
        {
            ResponseModel _result = new ResponseModel(); ;
            try
            {
                if (ModelState.IsValid)
                {
                    // role.Active = true;
                    _result = _roleService.CreateRole(role);
                }
                return _result;

            }
            catch (Exception)
            {
                return new ResponseModel { ResponseStatus = false, ResponseText = "There are some error please try again." };
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ResponseModel DeleteRole(int id)
        {
            ResponseModel oResponseModel = new ResponseModel();
            if (_roleService.DeleteRole(id) == true)
            {
                oResponseModel.ResponseText = "Deleted Sucessfully";
                oResponseModel.ResponseStatus = true;
            }
            else
            {
                oResponseModel.ResponseText = "Error";
                oResponseModel.ResponseStatus = false;
            }
            return oResponseModel;
        }

        [HttpPost]
        [AllowAnonymous]
        public ResponseModel EditRole(Roles oRoles)
        {

            ResponseModel oResponseModel = new ResponseModel();
            try
            {
                _roleService.EditRole(oRoles);
                oResponseModel.ResponseText = "Role Updated Sucessfully";
                oResponseModel.ResponseStatus = true;
            }
            catch (Exception ex)
            {
                oResponseModel.ResponseText = "Error! in updating role";
                oResponseModel.ResponseStatus = false;
            }

            return oResponseModel;
        }
    }
}
