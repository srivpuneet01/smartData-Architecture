using Core.enums;
using Core.Exceptions;
using System;
using System.Web.Mvc;

namespace smartData.Admin
{
    public class RoleBasedAccess : AuthorizeAttribute
    {
        public String NotInRole { get; set; }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // added by rachna/SA on 03 feb 14 w.r.t #380 TFS
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated && !filterContext.HttpContext.Request.IsAjaxRequest())
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    throw new LoginException(ExceptionTypeEnum.LogoutWithAjaxRequest.ToString());
                }
                else if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    base.HandleUnauthorizedRequest(filterContext);
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new System.Web.Routing.RouteValueDictionary(new { Controller = "Login", Action = "Logout", Area = "" })));
                }
            }
        }
    }
}