using Core.Domain;
using System.Collections.Generic;
using AppInterfaces.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer.Services
{
    public class ScreenPermissionService : IScreenPermissionService
    {
        #region Global Variables
 
        IScreenPermissionRepository _screenPermissionRepository = null;
        // ServiceLayer.Interfaces.IScreenPermissionService _screenRepository = null;
        #endregion

        #region constructor
        
        public ScreenPermissionService(IScreenPermissionRepository screenPermissionRepository)
        {
            _screenPermissionRepository = screenPermissionRepository;// new RepositoryLayer.Repositories.ScreenPermissionRepository();
        }
        #endregion

        #region  Public Methods
        public List<ScreenPermission> GetAllScreenPermission()
        {
            return _screenPermissionRepository.GetAllScreenPermission();
        }
        public List<Screen> GetAllscreen()
        {
            return _screenPermissionRepository.GetAllScreen();
        }

        public void AddScreenPermission(string str, int roleId, int moduleID)
        {
            _screenPermissionRepository.AddScreenPermission(str, roleId, moduleID);
        }
        public bool DeleteScreenPermission(string str, int roleId, int moduleId)
        {
            return _screenPermissionRepository.DeleteScreenPermission(str, roleId, moduleId);
        }
        public List<ScreenPermission> GetPermissionList(int roleId, int moduleID)
        {
            return _screenPermissionRepository.GetPermissionList(roleId, moduleID);

        }
        public List<Roles> GetAllRoles()
        {
            return _screenPermissionRepository.GetAllRoles();
        }
        public ScreenPermission GetDropDown()
        {
            return _screenPermissionRepository.GetDropDown();
        }
        public List<ScreenPermissionList> GetAllScreenPermissionl()
        {
            return _screenPermissionRepository.GetAllScreenPermissionl();
        }

        public string GetAuthorizeToken(int userId)
        {
            return _screenPermissionRepository.GetAuthorizeToken(userId);
        }
        #endregion
    }
}
