using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartData
{
  public  interface IPagePermissionAPIController
    {
      string GetAllowPermission(string TokenId, string ScreenName, string ActionName);
    }
}
