using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using CoreEntities.CustomModels;

namespace AppInterfaces.Interfaces
{
    public interface IModulePermissionRepository
    {
        List<ScreenPermissionList> GetAllScreenPermission(int moduleID, int roleId);
        ResponseModel CreateFullPermission(ModulePermission modulepermission);
        List<ModulePermission> GetModulePermission(int moduleID, int roleId);
        bool DeleteModulePermission(int moduleID, int roleId);
    }
}
