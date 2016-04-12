using AppInterfaces.Interfaces;

namespace ServiceLayer.Services
{
    public  class PagePermissionService:Interfaces.IPagePermissionService
    {
      #region Global Variables
      IPagePermissionRepository _pagePermissionRepository = null;
      #endregion

      #region constructor

      public PagePermissionService(IPagePermissionRepository pagePermissionRepository)
        {
            //Bind. RepositoryLayer.Interfaces.IPagePermissionRepository _PagePermissionRepository
            _pagePermissionRepository = pagePermissionRepository;// new RepositoryLayer.Repositories.PagePermissionRepository();
        }
        #endregion

      #region Public Methods
      public string GetAllowPermission(string tokenId, string screenName, string actionName)
      {
          return _pagePermissionRepository.GetAllowPermission(tokenId, screenName, actionName);
      }
        #endregion
    }
}
