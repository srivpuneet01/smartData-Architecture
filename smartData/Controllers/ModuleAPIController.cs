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
    public class ModuleAPIController : ApiController
    {

        public class ModuleListFilters : GridFilter
        {
            public string ModuleName { get; set; }

        }

        public class ModuleReturnVal
        {
            public IEnumerable<Module> data;
            public int total;
        }
        ServiceLayer.Interfaces.IModuleService _moduleService;
        public ModuleAPIController(IModuleService _moduleService)
        {
            this._moduleService = _moduleService;
            //System.Net.Http.Headers.contContentType = new MediaTypeHeaderValue("application/json");
        }


        [HttpPost]
        [AllowAnonymous]
        public IEnumerable<Module> GetModule(ModuleListFilters filter)
        {
            try
            {
                int total;
              
                filter.order = "asc";
                filter.limit = 10000;
                ModuleReturnVal re = new ModuleReturnVal();
                re.data = _moduleService.GetAllModules(filter.limit, filter.offset, filter.order, filter.sort, filter.ModuleName, out total);
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
        public ResponseModel CreateModule(Module module)
        {
            ResponseModel _result = new ResponseModel(); ;
            try
            {
                if (ModelState.IsValid)
                {
                    // role.Active = true;
                    _result = _moduleService.CreateModule(module);
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
        public ResponseModel DeleteModule(int id)
        {
            ResponseModel oResponseModel = new ResponseModel();
            if (_moduleService.DeleteModule(id) == true)
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
        public ResponseModel EditModule(Module oModule)
        {

            ResponseModel oResponseModel = new ResponseModel();
            try
            {
                _moduleService.EditModule(oModule);
                oResponseModel.ResponseText = "Module Updated Sucessfully";
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
