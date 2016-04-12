using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;

namespace AppInterfaces.Interfaces
{
  public  interface IPagePermissionRepository
    {
      string GetAllowPermission(string TokenId, string ScreenName, string ActionName);
    }
}
