using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.Script.Serialization;
using smartData.Admin;
using smartData.Controllers;

namespace smartData.Areas.Users.Controllers
{
    public class ElmahController : BaseController
    {

        public ActionResult Index(string type)
        {
            return new ElmahResult(type);
        }
    }

    class ElmahResult : ActionResult
    {
        private string _resouceType;

        public ElmahResult(string resouceType)
        {
            _resouceType = resouceType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var factory = new Elmah.ErrorLogPageFactory();
            if (!string.IsNullOrEmpty(_resouceType))
            {
                var pathInfo = "." + _resouceType;
                HttpContext.Current.RewritePath(HttpContext.Current.Request.Path, pathInfo, HttpContext.Current.Request.QueryString.ToString());
            }

            var handler = factory.GetHandler(HttpContext.Current, null, null, null);

            handler.ProcessRequest(HttpContext.Current);
        }
    }

    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);

            }
            else
            {
                if (!filterContext.HttpContext.User.IsInRole("admin"))
                    filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Home", action = "index" }));
            }
        }

    }
}