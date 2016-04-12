using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using CoreEntities.CustomModels;

namespace ServiceLayer.Interfaces
{
   public interface IPagePermissionService
    {
       string GetAllowPermission(string TokenId, string ScreenName, string ActionName);
    }
}
