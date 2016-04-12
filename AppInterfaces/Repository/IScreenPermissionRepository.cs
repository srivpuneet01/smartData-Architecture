using Core.Domain;
using System.Collections.Generic;

namespace AppInterfaces.Interfaces
{
    public interface IScreenPermissionRepository
    {
        List<ScreenPermission> GetAllScreenPermission();
        List<Screen> GetAllScreen();
        void AddScreenPermission(string str, int roleId, int moduleID);
        bool DeleteScreenPermission(string str, int roleId, int moduleID);
        List<ScreenPermission> GetPermissionList(int roleId, int moduleID);
        List<Roles> GetAllRoles();
        ScreenPermission GetDropDown();
        string GetAuthorizeToken(int userID);

        List<ScreenPermissionList> GetAllScreenPermissionl();
    }
}
