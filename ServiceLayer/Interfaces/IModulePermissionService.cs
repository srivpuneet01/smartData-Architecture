using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using CoreEntities.CustomModels;
namespace ServiceLayer.Interfaces
{
  public interface IModulePermissionService
    {
      List<ScreenPermissionList> GetAllScreenPermission(int moduleId, int roleId);
      ResponseModel CreateFullPermission(ModulePermission modulepermission);
      List<ModulePermission> GetModulePermission(int moduleId, int roleId);
      bool DeleteModulePermission(int moduleId, int roleId);
    }
}
