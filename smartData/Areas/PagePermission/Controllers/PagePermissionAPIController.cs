using smartData.Infrastructure;
using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using smartData.Filter;
using ServiceLayer.Interfaces;
using ServiceLayer.Services;

namespace smartData
{
     [HandleException]
    public class PagePermissionAPIController : ApiController, IPagePermissionAPIController
    {
        #region Initialize
        ServiceLayer.Interfaces.IPagePermissionService _pagePermissionService;
        #endregion

        #region constructor
        public PagePermissionAPIController(IPagePermissionService pagePermissionService)
        {
            _pagePermissionService = pagePermissionService;// new PagePermissionService();
            //System.Net.Http.Headers.contContentType = new MediaTypeHeaderValue("application/json");
            //ServiceLayer.Interfaces.IPagePermissionService _PagePermissionService
        }
        #endregion

        #region Public Methods 
       public  string GetAllowPermission(string TokenId, string ScreenName, string ActionName)
        {
            return _pagePermissionService.GetAllowPermission(TokenId, ScreenName, ActionName);
        }
        #endregion
    }
}