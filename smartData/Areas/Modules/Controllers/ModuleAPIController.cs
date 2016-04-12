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
    public class ModuleListFilters : GridFilter
    {
        public string ModuleName { get; set; }

    }

    public class ModuleReturnVal
    {
        public IEnumerable<Module> data;
        public int total;
    }
    [HandleException]
    public class ModuleAPIController : ApiController, IModuleAPIController
    {
        ServiceLayer.Interfaces.IModuleService _moduleService;
        public ModuleAPIController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }
       public  ResponseModel CreateModule(Module module)
        {
            return _moduleService.CreateModule(module);
        }
       [HttpPost]
       [ActionName("GetAllModules")]
       [AntiforgeryValidate]
       public dynamic GetAllModules(JObject Obj)
       {
           ModuleListFilters filter = Obj.ToObject<ModuleListFilters>();
           int total;
           ModuleReturnVal re = new ModuleReturnVal();
           re.data = _moduleService.GetAllModules(filter.limit, filter.offset, filter.order, filter.sort, filter.ModuleName, out total);

           var result = new
           {
               total = total,
               rows = re.data,
           };
           return result;
       }
       [HttpPost]
       [ActionName("EditModule")]
       [AntiforgeryValidate]
       public void EditModule(Module module)
       {
           _moduleService.EditModule(module);
       }

      
       [HttpPost]
       [ActionName("DeleteModule")]
       [AntiforgeryValidate]
       public bool DeleteModule(int id)
       {
           return _moduleService.DeleteModule(id);
       }
    }
}