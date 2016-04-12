using Core.Domain;
using CoreEntities.CustomModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartData
{
    public interface IRolesAPIController
    {
        ResponseModel CreateRole(Roles role);
        void EditRole(Roles role);
        bool DeleteRole(int id);

    }
}
