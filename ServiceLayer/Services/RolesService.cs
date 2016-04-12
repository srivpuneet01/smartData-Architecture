using Core.Domain;
using System.Collections.Generic;
using CoreEntities.CustomModels;
using AppInterfaces.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer.Services
{
    public class RolesService : IRolesService
    {  
       
        #region Global Variables
        IRolesRepository _rolesRepository = null;
        IUserRepository _userRepository = null;
         #endregion

        #region constructor
     
        public RolesService(IUserRepository userRepository, IRolesRepository roleRepository)
        {
            _userRepository = userRepository;
            _rolesRepository = roleRepository;
        }
        #endregion

        #region  Public Methods
       
        public ResponseModel CreateRole(Roles role)
        {
            return _rolesRepository.CreateRole(role);

        }
        public List<Roles> GetAllRoles(int limit, int offset, string order, string sort, string roleName, out int total)
        {
            return _rolesRepository.GetAllRoles(limit, offset, order, sort, roleName, out  total);
        }
        public void EditRole(Roles role)
        {

            role.CreatedDate = System.DateTime.Now;
            _rolesRepository.EditRole(role);
        }

        public bool DeleteRole(int id)
        {
            return _rolesRepository.DeleteRole(id);
        }

        public void MassUpdate(string roleNames, string unCheckIds, int modifyBy)
        {
             _rolesRepository.MassUpdate(roleNames,unCheckIds, modifyBy);
        }
        #endregion
    }
}
