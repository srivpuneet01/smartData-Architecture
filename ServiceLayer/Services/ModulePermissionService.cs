using AppInterfaces.Interfaces;
using Core.Domain;
using CoreEntities.CustomModels;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class ModulePermissionService : IModulePermissionService
    {
        #region Global Variables

        IModulePermissionRepository _modulePermissionRepository = null;

        #endregion

          #region constructor

        public ModulePermissionService(IModulePermissionRepository modulePermissionRepository)
        {
            _modulePermissionRepository = modulePermissionRepository;
        }


        #endregion

        #region Public Methods
        public  List<ScreenPermissionList> GetAllScreenPermission(int moduleId, int roleId)
        {
            return _modulePermissionRepository.GetAllScreenPermission(moduleId, roleId);
        }
       public ResponseModel CreateFullPermission(ModulePermission modulepermission)
        {
            return _modulePermissionRepository.CreateFullPermission(modulepermission);
        }
       public List<ModulePermission> GetModulePermission(int moduleId, int roleId)
       {
           return _modulePermissionRepository.GetModulePermission(moduleId, roleId);
       }
       public bool DeleteModulePermission(int moduleId, int roleId)
       {
           return _modulePermissionRepository.DeleteModulePermission(moduleId, roleId);
       }
        #endregion
    }
}
