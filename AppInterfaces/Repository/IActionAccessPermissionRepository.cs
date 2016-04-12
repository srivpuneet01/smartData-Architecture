using Core.Domain;
using System.Collections.Generic;

namespace AppInterfaces.Interfaces
{
    public interface IActionAccessPermissionRepository
    {
      List<ScreenPermissionList> GetScreenPermissionByUserIdAndScreenId(string userId, string screenName);
      void UpdateLoginAuthentication(string userId);
      void UpdateLastActivityTime(string tokenId);
    }
}
