using Core.Domain;
using System.Collections.Generic;
using AppInterfaces.Interfaces;
using ServiceLayer.Interfaces;
using AppInterfaces.Repository;

namespace ServiceLayer.Services
{
    public class ActionAccessPermissionService : IActionAccessPermissionService
    {
        #region Global Variables
        IActionAccessPermissionRepository _actionAccessPermissionRepository = null;
        #endregion

        #region  Public Methods

        public ActionAccessPermissionService(IActionAccessPermissionRepository actionAccessPermissionRepository)
        {
            _actionAccessPermissionRepository = actionAccessPermissionRepository;
        }
        public List<ScreenPermissionList> GetScreenPermissionByUserIdAndScreenId(string userId, string screenName)
        {
            return _actionAccessPermissionRepository.GetScreenPermissionByUserIdAndScreenId(userId, screenName);
        }
        public void UpdateLoginAuthentication(string userId)
        {
            _actionAccessPermissionRepository.UpdateLoginAuthentication(userId);
        }
        public void UpdateLastActivityTime(string tokenId)
        {
            _actionAccessPermissionRepository.UpdateLastActivityTime(tokenId);
        }
        #endregion
    }
}
