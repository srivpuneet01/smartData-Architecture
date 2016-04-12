using Core.Domain;
using System.Collections.Generic;
using CoreEntities.CustomModels;
using AppInterfaces.Interfaces;

namespace ServiceLayer.Services
{
    public class ModuleService:Interfaces.IModuleService
    {
       #region Global Variables

        IModuleRepository _moduleRepository = null;//new RepositoryLayer.Repositories.RolesRepository();
         #endregion

       #region constructor

        public ModuleService(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }
        #endregion

        #region public methods
        public ResponseModel CreateModule(Module module)
        {
            return _moduleRepository.CreateModule(module);

        }

       public  List<Module> GetAllModules(int limit, int offset, string order, string sort, string moduleName, out int total)
        {
            return _moduleRepository.GetAllModules(limit, offset, order, sort, moduleName,out total);
        }

       public void EditModule(Module module)
       {
           _moduleRepository.EditModule(module); 
       }

       public bool DeleteModule(int id)
       {
          return  _moduleRepository.DeleteModule(id);
       }
        #endregion
    }
}
