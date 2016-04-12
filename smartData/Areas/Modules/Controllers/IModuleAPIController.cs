using Core.Domain;
using CoreEntities.CustomModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartData
{
   public interface IModuleAPIController
    {
     ResponseModel CreateModule(Module module);
    // List<Module> GetAllModules(int limit, int offset, string order, string sort, string ModuleName, out int total);
     dynamic GetAllModules(JObject Obj);
     void EditModule(Module role);
     bool DeleteModule(int id);
    }
}
