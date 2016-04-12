using Core.Domain;
using CoreEntities.CustomModels;
using System.Collections.Generic;

namespace ServiceLayer.Interfaces
{
    public interface IModuleService
    {
        ResponseModel CreateModule(Module module);
        List<Module> GetAllModules(int limit, int offset, string order, string sort, string moduleName, out int total);
        void EditModule(Module module);
        bool DeleteModule(int id);
    }
}
