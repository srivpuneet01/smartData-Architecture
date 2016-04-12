using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;

namespace ServiceLayer.Interfaces
{
    public interface IActionAccessPermissionService
    {
        List<ScreenPermissionList> GetScreenPermissionByUserIdAndScreenId(string UserId, String ScreenName);
        void UpdateLoginAuthentication(string UserId);
        void UpdateLastActivityTime(string TokenId);
    }
}
