using Core.Domain;
using CoreEntities.CustomModels;
using System.Collections.Generic;

namespace AppInterfaces.Interfaces
{
    public interface IRolesRepository
    {
        ResponseModel CreateRole(Roles role);
        List<Roles> GetAllRoles(int limit, int offset, string order, string sort, string roleName, out int total);
        void EditRole(Roles role);
        bool DeleteRole(int id);
        void MassUpdate(string roleNames, string unCheckIds, int modifyBy);
    }
}
