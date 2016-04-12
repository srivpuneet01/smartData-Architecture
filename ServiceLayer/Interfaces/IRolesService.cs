using Core.Domain;
using CoreEntities.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
   public interface IRolesService
    {
       ResponseModel CreateRole(Roles role);
       List<Roles> GetAllRoles(int limit, int offset, string order, string sort, string RoleName, out int total);
       void EditRole(Roles role);
       bool DeleteRole(int id);
       void MassUpdate(string roleNames, string UnCheckIds, int ModifyBy);
    }
}
