using Core.Domain;
using CoreEntities.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
  public   interface IScreenPermissionService
    {
        List<ScreenPermission> GetAllScreenPermission();
        List<Screen> GetAllscreen();
        void AddScreenPermission(string str, int RoleId, int ModuleID);
        bool DeleteScreenPermission(string str, int RoleId, int ModuleID);
        List<ScreenPermission> GetPermissionList(int RoleId, int ModuleID);
        List<Roles> GetAllRoles();
        ScreenPermission GetDropDown();
        List<ScreenPermissionList> GetAllScreenPermissionl();
        string GetAuthorizeToken(int UserID);
    }
}
